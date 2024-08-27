using CardGame.Game;
using CardGame.Game.GameEvents;
using CardGame.Game.GameTerms;
using CardGame.Game.GameTerms.Abilities;
using CardGame.Game.GameTerms.PlayerUtility;
using CardGame.Game.GameTerms.Units;
using CardGame.Lib.Deck;
using Godot;
using RatDuck.Script.Godot;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Xml.Schema;
using static CardGame.Game.GameTerms.TurnManager;
using static RatDuck.Script.Godot.GodotGlobal;
public partial class DisplayMap : Control
{
	// Called when the node enters the scene tree for the first time.
	GodotGlobal godotGlobal;
	List<GodotGlobal.PlayerInfo> playerInfos;
	AbilityHandle abilityHandle;
	Game game;

	public override void _Ready()
	{
		godotGlobal = GodotGlobal.godotGlobal;
		playerInfos = godotGlobal.playerInfos;
		abilityHandle = Global.getAbilityHandle();
		game = Global.getGame();
		init_button();
		initPathNodeToGameNode(game);
		initGamePlayer();
		initRegisterSignal();
		initTurnManager();
		initGameStart();
		
	}
	[Export]
	Label curPlayerLabel;


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	void initTurnManager()
	{
		var first = game.playerHandle.getGameFirstPlayer();
		var players = new List<Player>();
		var allplayers = game.playerHandle.allPlayer.getPlayers();
		foreach (var player in allplayers)
		{
				players.Add(player);
		}
		Global.getTurnManager().init(players, first);
	}

	void initRegisterSignal()
	{
		WorldMapView.Connect("node_gui_input", Callable.From<InputEvent,NodePath,long,Variant>(_on_worldmap_view_node_gui_input));
		endTurnButton.Connect("pressed", Callable.From(on_endTurn_pop));
		endTurnMenuButton.GetPopup().Connect("id_pressed", Callable.From<int>(on_endTurn_pressed));
		target_button.Connect("pressed", Callable.From(on_target_pressed));
		kidnap_button.Connect("pressed", Callable.From(on_kidnap_pressed));
		research_button.Connect("pressed", Callable.From(on_research_pressed));
		link_button.Connect("pressed",Callable.From(on_link_pressed));
		unlink_button.Connect("pressed", Callable.From(on_unlink_pressed));
		skip_move_button.Connect("pressed", Callable.From(on_skip_move_pressed));
		control_button.Connect("pressed", Callable.From(on_control_pressed));
		speech_button.Connect("pressed", Callable.From(on_speech_pressed));
		release_button.Connect("pressed", Callable.From(on_release_pressed));
		arrest.Connect("pressed", Callable.From(_on_arrest_pressed));


	}
	[Export]
	ItemList _MapNodeTokenList;
	[Export]
	Label _mapNodeLabel;
	[Export]
	Node WorldMapView;
	[Export]
	Node WorldMapGraph;
	[Export]
	Button research_button;
	[Export]
	Button target_button;
	[Export]
	Button kidnap_button;

	void initGamePlayer()
	{
		foreach (var playerInfo in playerInfos)
		{
			var player = new Player(Global.getGame(), playerInfo.role);
			playerInfo.player = player;
			godotGlobal.addPlayerInfo(playerInfo.player, playerInfo);
		}
	}
	void initActiveWorldMap()
	{
		var nodeDatas = WorldMapGraph.Get("node_datas").AsGodotArray();
	}
	Dictionary<GameNode, TokenLabelPreset.CardInfo > dic_gameNode_to_info = new();
	void initPathNodeToGameNode(Game game)
	{
		var nodeDatas = WorldMapGraph.Get("node_datas").AsGodotArray();

		var map = game.map;
		var placeCounter = 0;
		foreach (var nodeData in nodeDatas)
		{
			var gamenode = new GameNode(game);
			var tags = nodeData.AsGodotObject().Get("tags").AsGodotArray<StringName>();
			
			string name;
			switch (tags[0])
			{
				case "lab": 
					map.lab = gamenode;
					name = godotGlobal.tokenLabelPreset.rollLabPresets();
					break;
				case "policeStation": 
					map.policeStation = gamenode;
					name = godotGlobal.tokenLabelPreset.rollPoliceStationPresets();
					break;
				case "prison":
					map.prison = gamenode;
					name = godotGlobal.tokenLabelPreset.rollPrisonPresets();
					break;
				default: 
					map.add(gamenode);
					var preset = godotGlobal.tokenLabelPreset.bodyPresets[placeCounter];
					name = preset.nice_name;
					dic_gameNode_to_info.Add(gamenode, preset);
					addNewBodyPlacement(gamenode,preset);
					placeCounter++;
					break;
			}
			_MapNodes.Add(gamenode);
			var info = new GameNodeInfo(name);
			godotGlobal.addGameNodeInfo(gamenode, info);
			game.abilityHandle.abilityScientist.init(BodyPlaceDecks, 2);
		}


		var connections = WorldMapGraph.Get("connection_nodes").AsGodotArray<Vector2I>();

		foreach(var connection in connections)
		{
			var gamenodeA = _MapNodes[connection[0]];
			var gameNodeB = _MapNodes[connection[1]];
			map.neighbor(gamenodeA, gameNodeB);
		}
	}

	Deck<BodyPlaceCard> BodyPlaceDecks = new();
	private void addNewBodyPlacement(GameNode gameNode, TokenLabelPreset.CardInfo cardInfo)
	{
		var card = new BodyPlaceCard(game, gameNode);
		BodyPlaceDecks.Add(card);
		godotGlobal.link_card_to_cardInfo(card, cardInfo);
	}


	List<GameNode> _MapNodes = new List<GameNode>();
	List<Unit> _currntMapNodeTokenList;
	private GameNode getGameNode(long node_idx)
	{
		return _MapNodes[(int)node_idx];
	}
	void addInfoToMapNodeList(Unit unit,string info)
	{
		_currntMapNodeTokenList.Add(unit);
		_MapNodeTokenList.AddItem(info);
		
	}
	void addPlayerInfoToMapNodeList(Role unit, PlayerInfo playerInfo)
	{
		var str = playerInfo.getPlayerInfoString();
		addInfoToMapNodeList(unit, str);
	}
	void addBodyInfoToMapNodeList(Body body)
	{
		var str = body.name;
		addInfoToMapNodeList(body, str);
	}
	void cleanInfo()
	{
		_currntMapNodeTokenList = new List<Unit>();
		var count = _MapNodeTokenList.ItemCount;
		for (int i = 0; i < count; i++)
		{
			_MapNodeTokenList.RemoveItem(0);
		}
	}

	void addPlayerInfoToList(ItemList itemList, PlayerInfo playerInfo)
	{
		itemList.AddItem(playerInfo.getPlayerInfoString());
	}
	Unit getUnit(GameNode gameNode,long idx)
	{
		abilityHandle.abilityGameNode.tryGetUnits(gameNode, out var list);
		var unit = _currntMapNodeTokenList[(int)idx];

		return unit;
	}

	
	private void _on_worldmap_view_node_gui_input(InputEvent @event, NodePath path, long node_in_path, Variant resource)
	{
		if (@event is InputEventMouseButton && path != null)
		{
			var @inputEvent = @event as InputEventMouseButton;

			if (inputEvent.ButtonIndex == MouseButton.Left)
			{
				GameNode gameNode = getGameNode(node_in_path);
				updateGameNodeList(gameNode);
				return;
			}
			
			if(inputEvent.ButtonIndex == MouseButton.Right && 
				game.turnManager.getPlayerStates() == PlayerStates.Premove)
			{
				var curPlayer = getCurPlayer();
				var unit = curPlayer.controlUnit;
				var abilityMove = game.abilityHandle.abilityMove;
				GameNode gameNode = getGameNode(node_in_path);
				if (abilityMove.tryMove(unit, gameNode, out var errorTxt))
				{
					printToMonitor(godotGlobal.getActionString(curPlayer, Terms.move, gameNode));
					if (abilityMove.tryGetAttribute(unit, out var att))
					{
						if (att.movePoint == 0)
						{
							setTurnState(PlayerStates.PreAction);
						}
					}
					else { setTurnState(PlayerStates.Premove); }
					updateGameNodeList(gameNode);
				}
				else
				{
					printToMonitor(errorTxt);
				}
			}
		}
	}

	void updateGameNodeList(GameNode gameNode)
	{
		abilityHandle.abilityGameNode.tryGetUnits(gameNode, out var list);
		cleanInfo();
		_mapNodeLabel.Text = godotGlobal.GetGameNodeInfo(gameNode).name;
		foreach (var unit in list)
		{
			if (unit.GetType().BaseType.Equals(typeof(Role))
				|| unit.GetType().BaseType.Equals(typeof(Gov)))
			{
				var player = abilityHandle.abilityPlayer[unit];
				var info = godotGlobal.getPlayerInfo(player);
				addPlayerInfoToMapNodeList(unit as Role, info);
			}
			if (unit.GetType() == typeof(Body))
			{
				addBodyInfoToMapNodeList(unit as Body);
			}
		}
		var unrestLevel = abilityHandle.abilityChaoticMeasure[gameNode];
		if(unrestLevel >= 0) { updatePlaceUnrestText(unrestLevel, true); }
		else { updatePlaceUnrestText(0, false); }
	}

	void updatePlaceUnrestText(int unrestLevel, bool locValid)
	{
		if(locValid)
		{
			place_unrest.Text = "Place Unrest Level = " + unrestLevel + " / " + CONST_ABILITY_CHAOTICMEASURE.MAXMEASURE;
		}
		else
		{
			place_unrest.Text = "Place Unrest Level invalid in this place.";
		}
	}

	
	[Export]
	Button arrest;
	private Unit getSelection()
	{
		var selected = _MapNodeTokenList.GetSelectedItems();
		if (selected.Length >0  && _currntMapNodeTokenList.Count > 0)
		{
			return _currntMapNodeTokenList[selected[0]];
		}
		else { return null; }
	}
	
	Player getCurPlayer()
	{
		return game.turnManager.curPlayer;
	}
	GameNode getLoc(Unit unit)
	{
		var abilityLoc = abilityHandle.abilityLoc;
		return abilityLoc[unit];
	}
	private void _on_arrest_pressed()
	{
		var policeAbility = abilityHandle.abilityPolice;
		var controlPlayer = getCurPlayer();
		var control = controlPlayer.controlUnit;
		var selected = getSelection();
		
		if (selected != null)
		{
			if(policeAbility.tryArrest(control, selected, out var errorMesg))
			{
				printToMonitor(godotGlobal.getActionString(controlPlayer, Terms.arrest, Player.getPlayer(selected as Role)));
				setTurnState(PlayerStates.AfterAction);
				updateGameNodeList(getLoc(control));

			}
			else { printToMonitor(errorMesg); }

		}
	}
	[Export]
	TextEdit Movefeedback;
	private void printToMonitor(string newText)
	{
		var s ="\n" + newText + Movefeedback.Text;
		Movefeedback.Text = s;
	}

	void addToPlayerLabelTag(string newText)
	{
		curPlayerLabel.Text = newText;
	}

	void onBegin()
	{
		nextTurnState();
		updateEndTurnPlayer();
	}
	void initGameStart()
	{
		initEndTurnButton(endTurnMenuButton);
		onBegin();
	}
	void setTurnState(PlayerStates playerStates)
	{
		game.turnManager.playerStates = playerStates;
		addToPlayerLabelTag(godotGlobal.getPlayerState(getCurPlayer(), game.turnManager.playerStates));
		disableButtons(godotGlobal.getPlayerInfo(getCurPlayer()).role, playerStates);
	}

	[Export]
	MenuButton endTurnMenuButton;
	[Export]
	Button endTurnButton;
	void on_endTurn_pop()
	{
		//go to next turn if everyone is played.
		game.turnManager.endTurn(getCurPlayer());
		onBegin();
		
	}
	void on_endTurn_pressed(int id)
	{
		var pop = endTurnMenuButton.GetPopup();
		game.turnManager.endTurn(intToPlayer[id]);
		onBegin();
	}
	Dictionary<Player,int> playerToInt = new();
	List<Player> intToPlayer = new();
	void initEndTurnButton(MenuButton button)
	{
		var i = 0;
		var pop = button.GetPopup();
		foreach (var player in game.playerHandle.allPlayer.getPlayers())
		{
			pop.AddItem(godotGlobal.getPlayerInfo(player).name, i);
			playerToInt.Add(player, i);
			intToPlayer.Add(player);
			i++;
		}
	}
	void updateEndTurnPlayer()
	{
		var curPlayer = getCurPlayer();
		var dissTeam = game.playerHandle.dissTeam;
		var govTeam = game.playerHandle.govTeam;
		var allTeam = game.playerHandle.allPlayer;

		var turnManager = game.turnManager;
		var pop = endTurnMenuButton.GetPopup();
		var false_count = 0;
		var true_count = 0;
		foreach (var player in allTeam.getPlayers())
		{
			if (turnManager.unplayers.Contains(player) && player.team == game.playerHandle.getOppositeTeam(curPlayer.team))
			{
				pop.SetItemDisabled(playerToInt[player], false);
				false_count++;

			}
			else
			{
				pop.SetItemDisabled(playerToInt[player], true);
				true_count++;
			}
		}
		if(turnManager.unplayers.Count >0 && false_count == 0)
		{
			foreach (var player in turnManager.unplayers)
			{
				pop.SetItemDisabled(playerToInt[player], false);
			}
		}
		else if (turnManager.unplayers.Count == 0)
		{
			endTurnButton.Visible = true;
			endTurnMenuButton.Visible = false;
		}
		else
		{
			endTurnButton.Visible = false;
			endTurnMenuButton.Visible = true;
		}
	}
	void nextTurnState()
	{
		setTurnState(game.turnManager.playerStates + 1);
	}

	[Export]
	ItemList CardSlot;
	void on_target_pressed()
	{
		var deckWithShowcase = game.abilityHandle.abilityScientist.deck;
		var current = deckWithShowcase.showcase;
		if(current.Count >= 2)
		{
			printToMonitor(godotGlobal.getLocationCardMaxWarning());
			return;
		}
		else
		{
			deckWithShowcase.drawToShowcase(true, true);
			updateCardSlotList(current);
		}
	}

	void on_kidnap_pressed()
	{
		var unit = getCurPlayer().controlUnit;
		var gameNode = abilityHandle.abilityLoc[unit];
		var deck = abilityHandle.abilityScientist.deck;
		var currentCards = deck.showcase;
		var found = false;
		BodyPlaceCard bodyPlaceCard = null;
		foreach (var card in currentCards)
		{
			if (card.gameNode == gameNode)
			{
				found = true;
				bodyPlaceCard = card;
				break;
			}
		}
		if(found)
		{
			currentCards.Remove(bodyPlaceCard);
			updateCardSlotList(currentCards);
			var body = new Body(game);
			body.name = "Research Object";
			new TokenCreate(body, gameNode);
			setTurnState(PlayerStates.AfterAction);
			updateGameNodeList(getLoc(unit));
			printToMonitor(godotGlobal.getActionString(getCurPlayer(), Terms.kidnap, gameNode));

		}
		else
		{
			printToMonitor("Not at a target place.");
		}
	}

	void on_research_pressed()
	{
		var unit = getCurPlayer().controlUnit;
		var unitLoc = abilityHandle.abilityLoc[unit];
		abilityHandle.abilityGameNode.tryGetUnits(unitLoc, out var gameNodeUnits);
		bool isfound = false;
		Body body = null;
		foreach(Unit token in gameNodeUnits)
		{
			if(token.GetType() == typeof(Body) && unitLoc == game.map.lab)
			{
				isfound = true;
				body = (Body)token;
				break;
			}
		}
		if(isfound)
		{
			new ScientistResearch(unit, body);
			setTurnState(PlayerStates.AfterAction);
			updateGameNodeList(getLoc(unit));
			printToMonitor(godotGlobal.getActionString(getCurPlayer(), Terms.research, unitLoc));
		}
	}


	void updateCardSlotList(Deck<BodyPlaceCard> bodyPlaceCards)
	{
		CardSlot.Clear();
		string cardText = "";
		foreach (var card in bodyPlaceCards)
		{
			var info = godotGlobal.get_cardInfo(card);
			cardText = info.get_name();
			CardSlot.AddItem(cardText);
		}
	}

	[Export]
	Button link_button;
	[Export]
	Button unlink_button;

	void on_link_pressed()
	{
		var control = getCurPlayer().controlUnit;
		var selectedUnit = getSelection();
		if (selectedUnit == null) { printToMonitor("No body selected."); return; }
		if (!(selectedUnit.GetType() == typeof(Body))) { printToMonitor("Not a body."); return; }
		abilityHandle.abilityCarry.link(control, selectedUnit as Body);
	}
	void on_unlink_pressed()
	{
		var control = getCurPlayer().controlUnit;
		abilityHandle.abilityCarry.remove(control);
	}
	[Export]
	Button speech_button;
	[Export]
	Button release_button;

	List<Button>[] buttons =  new List<Button>[3];
	List<Button> govbuttons;

	void disableButtons(Roles role, PlayerStates playerStates)
	{
		switch(playerStates)
		{
			case PlayerStates.Begin:
			case PlayerStates.Premove:
				{
					skip_move_button.Disabled = false;
					ableActionButtonsByRoles(role);
					break;
				}
			case PlayerStates.PreAction:
				{
					skip_move_button.Disabled = true;
					ableActionButtonsByRoles(role);
					break;

				}
			case PlayerStates.AfterAction:
				{
					skip_move_button.Disabled = true;
					disableAllFunctioningButtons();
					break;
				}
			default:
				throw new NotImplementedException();
		}
	}


	void ableActionButtonsByRoles(Roles role, bool flip=false,bool disableOthers = true)
	{
		if (disableOthers)
		{
			foreach (var list_button in buttons)
			{
				if (buttons[(int)role] == list_button)
					foreach (var button in list_button) button.Disabled = flip;
				else
					foreach (var button in list_button) button.Disabled = !flip;
			}

		}
		else
		{
			foreach (var button in buttons[(int)role])
			{
				button.Disabled = !flip;
			}
		}
		if(role == Roles.Police || role == Roles.Scientist)
		{
			foreach(var button in govbuttons) button.Disabled = false;
		}
		else
		{
			foreach (var button in govbuttons) button.Disabled = true;
		}

		target_button.Disabled = !(role == Roles.Scientist);

	}
	void disableAllFunctioningButtons()
	{
		foreach (var list_button in buttons)
		{
			foreach (var button in list_button) button.Disabled = true;
		}
	}

	void init_button()
	{
		init_police_buttons();
		init_diss_buttons();
		init_scientist_buttons();
		init_gov_buttons();
	}
	void init_gov_buttons()
	{
		govbuttons = new List<Button>();
		govbuttons.Add(link_button);
		govbuttons.Add(unlink_button);
	}
	void init_police_buttons()
	{
		buttons[(int)Roles.Police] = new List<Button>();
		var list = buttons[(int)Roles.Police];
		list.Add(arrest);
		list.Add(control_button);

	}
	void init_scientist_buttons()
	{
		buttons[(int)Roles.Scientist] = new List<Button>();
		var list = buttons[(int)Roles.Scientist];
		list.Add(research_button);
		list.Add(kidnap_button);
		list.Add(target_button);
	}
	void init_diss_buttons()
	{
		buttons[(int)Roles.Dissent] = new List<Button>();
		var list = buttons[(int)Roles.Dissent];
		list.Add(release_button);
		list.Add(speech_button);
	}
	[Export]
	Button skip_move_button;
	[Export]
	Button control_button;

	void on_control_pressed()
	{
		var unit = getCurPlayer().controlUnit;
		var curplace = abilityHandle.abilityLoc[unit];
		if(abilityHandle.abilityPolice.tryToControl(unit, curplace, out var str))
		{
			setTurnState(PlayerStates.AfterAction);
			updateGameNodeList(getLoc(unit));
			printToMonitor(godotGlobal.getActionString(getCurPlayer(), Terms.control, curplace));
		}
		else
		{

		}
	}

	void on_skip_move_pressed()
	{
		setTurnState(PlayerStates.PreAction);
	}

	void on_speech_pressed()
	{
		var unit = getCurPlayer().controlUnit;
		var curplace = abilityHandle.abilityLoc[unit];
		if (abilityHandle.abilityDiss.trySpeech(unit,curplace, out var str))
		{
			setTurnState(PlayerStates.AfterAction);
			updateGameNodeList(curplace);
			printToMonitor(godotGlobal.getActionString(getCurPlayer(), Terms.speech, curplace));
		}
	}
	void on_release_pressed()
	{
		var unit = getCurPlayer().controlUnit;
		var curplace = abilityHandle.abilityLoc[unit];
		foreach(var token in _currntMapNodeTokenList)
		{
			if (!(abilityHandle.abilityLoc[token] == curplace))
			{
				printToMonitor("No body found.");
				return;
			}
			if(token.GetType() == typeof(Body))
			{
				var body = (Body)token;
				if (abilityHandle.abilityDiss.tryRelease(unit, body, out var str))
				{
					setTurnState(PlayerStates.AfterAction);
					printToMonitor(godotGlobal.getActionString(getCurPlayer(), Terms.release, curplace));
					updateGameNodeList(curplace);
					return;
				}
			}
		}
	}

	[Export]
	Label place_unrest;
}
