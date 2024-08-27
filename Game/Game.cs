using CardGame.Game.GameEvents;
using CardGame.Game.GameTerms;
using CardGame.Game.GameTerms.Abilities;
using CardGame.Game.GameTerms.Abilities.Bases;
using CardGame.Game.GameTerms.Abilities.Cards;
using CardGame.Game.GameTerms.Units;
using CardGame.Lib.Deck;
using CardGame.Lib.EventSystem;
using RatDuck.Script.Game.GameTerms.Units;

namespace CardGame.Game
{
	public class Game
	{
		UnitHandle _handleManagerUnit;
		EventManager _eventManager;
		AbilityHandle _abilityHandle;
		Map _map;
		EndGame _endGame;
		PlayerHandle _playerHandle;
		TurnManager _turnManager;

		public UnitHandle unitHandle => _handleManagerUnit;
		public EventManager eventManager => _eventManager;
		public AbilityHandle abilityHandle => _abilityHandle;
		public Map map => _map;
		public EndGame endGame => _endGame;
		public PlayerHandle playerHandle => _playerHandle;
		public TurnManager turnManager => _turnManager;

		public Game()
		{

		}
		public void init() {
			_eventManager = new EventManager();
			_handleManagerUnit = new UnitHandle(this);
			_map = new Map(this);
			_abilityHandle = new AbilityHandle(this);
			_endGame = new EndGame(map);
			_playerHandle = new PlayerHandle();
			_turnManager = new TurnManager(this);
			//MapBuilder mapBuilder = new MapBuilder(_map, this);
			map.init(this);
		}
	}
	public class EndGame
	{
		public enum WINS { GOV, DISS,TIE, NOWINNER}
		Map map;
		public class CONST_ENDGAME
		{
			public const int GOV_WIN_BIO_NEEDED = 4;
			public const int DISS_WIN_NEEDED = 15;
			public const int TURN_ENDS = 25;
			public const int GOV_WIN_POINT_MUL = 5;
			public const int DISS_WIN_POINT_MUL = 2;
		}
		public EndGame(Map map)
		{
			this.map = map;
		}

		public WINS getWinner()
		{
			if (isReachingDissEndGameCon())
			{
				return WINS.DISS;
			}
			if (isReachingGovEndGameCon())
			{
				return WINS.GOV;
			}
			if (isReachingTurnEndsCon())
			{
				return pointWinner();
			}
			else
				return WINS.NOWINNER;
		}

		public WINS pointWinner()
		{
			var dissPoint = map.getTotalChaoticMeasure() * CONST_ENDGAME.DISS_WIN_POINT_MUL;
			var govPoint = map.bioProgress * CONST_ENDGAME.GOV_WIN_POINT_MUL;
			if (dissPoint > govPoint)
				return WINS.DISS;
			else if (dissPoint == govPoint)
				return WINS.TIE;
			else
				return WINS.GOV;
		}

		
		public bool isReachingEndGame()
		{
			return isReachingDissEndGameCon() || isReachingGovEndGameCon() || isReachingTurnEndsCon();
		}

		public bool isReachingTurnEndsCon()
		{
			return map.turns >= CONST_ENDGAME.TURN_ENDS;
		}

		public bool isReachingGovEndGameCon()
		{
			return map.bioProgress >= CONST_ENDGAME.GOV_WIN_BIO_NEEDED;
		}
		public bool isReachingDissEndGameCon()
		{
			return map.getTotalChaoticMeasure() >= CONST_ENDGAME.DISS_WIN_NEEDED;
		}
	}

	public class AbilityHandle
	{
		Game game;
		public AbilityMove abilityMove { get; }
		public AbilityLoc abilityLoc { get; }
		public AbilityGameNode abilityGameNode { get; }
		public AbilityDiss abilityDiss { get; }
		public AbilityMoney abilityMoney { get; }
		public AbilityChaoticMeasure abilityChaoticMeasure { get; }
		public AbilityCarry abilityCarry { get; }
		public AbilityPolice abilityPolice { get; }
		public AbilityPlayer abilityPlayer { get; }
		public AbilityVisibiliy abilityVisibiliy { get; }
		public AbilityCardSlot abilityCardSlot { get; }
		public AbilityCost abilityCost { get; }
		public CONST_TOKEN.AbilityToken abilityToken { get; }
		public AbilityScientist abilityScientist { get; }
		public AbilityHandle(Game game)
		{
			this.game = game;

			abilityPlayer = new AbilityPlayer(game);
			abilityLoc = new AbilityLoc(game);
			abilityMove = new AbilityMove(game, abilityLoc);
			abilityGameNode = new AbilityGameNode(game, abilityLoc);
			abilityCarry = new AbilityCarry(game,abilityLoc);
			abilityMoney = new AbilityMoney(game);
			abilityChaoticMeasure = new AbilityChaoticMeasure(game);
			abilityDiss = new AbilityDiss(game,abilityCarry,abilityLoc,abilityChaoticMeasure,abilityGameNode,abilityMoney);
			abilityPolice = new AbilityPolice(game,abilityMove,abilityDiss,abilityMoney,abilityChaoticMeasure,abilityCarry,abilityLoc,abilityGameNode);
			abilityPlayer = new AbilityPlayer(game);
			abilityVisibiliy = new AbilityVisibiliy();
			abilityCardSlot = new AbilityCardSlot();
			abilityCost = new AbilityCost(abilityMoney, abilityCardSlot, abilityPlayer);
			abilityToken = new CONST_TOKEN.AbilityToken(abilityLoc,abilityGameNode);
			abilityScientist = new(game,this);

		}
		
	}
	/*
	public class MapBuilder
	{
		public MapBuilder(Map map,Game game)
		{
			var abilityGn = game.abilityHandle.abilityGameNode;
			var lab = new GameNode(game);
			var polStation = new GameNode(game);
			var prison = new GameNode(game);
			var p11 = new GameNode(game);
			var p12 = new GameNode(game);
			var p13 = new GameNode(game);
			var p14 = new GameNode(game);
			var p21 = new GameNode(game);
			var p22 = new GameNode(game);
			var p23 = new GameNode(game);
			var p24 = new GameNode(game);
			var p31 = new GameNode(game);
			var p32 = new GameNode(game);
			var p33 = new GameNode(game);
			var p34 = new GameNode(game);

			abilityGn.addNeighbor(prison, polStation);
			abilityGn.addNeighbor(polStation, p11);
			abilityGn.addNeighbor(polStation, p21);
			abilityGn.addNeighbor(polStation, p31);
			abilityGn.addNeighbor(p11,p12);
			abilityGn.addNeighbor(p12, p13);
			abilityGn.addNeighbor(p13, p14);
			abilityGn.addNeighbor(p21, p22);
			abilityGn.addNeighbor(p22, p23);
			abilityGn.addNeighbor(p23, p24);
			abilityGn.addNeighbor(p24, lab);
			abilityGn.addNeighbor(p31, p32);
			abilityGn.addNeighbor(p32, p33);
			abilityGn.addNeighbor(p33, p34);
			abilityGn.addNeighbor(p11, p21);
			abilityGn.addNeighbor(p21, p31);
			abilityGn.addNeighbor(p12, p22);
			abilityGn.addNeighbor(p22, p32);
			abilityGn.addNeighbor(p13, p23);
			abilityGn.addNeighbor(p23, p33);
			abilityGn.addNeighbor(p14, p24);
			abilityGn.addNeighbor(p24, p34);

			map.lab = lab;
			map.policeStation = polStation;
			map.prison = prison;
			map.add(p11,p12,p13,p14,p21,p22,p23,p24,p31,p32,p33,p34);

		}
	}

	public class BodyCardDeckGenerator
	{
		Game game;
		Map map;
		
		public BodyCardDeckGenerator(Game game,Map map) { 
			this.game = game;
			this.map = map;
		}
		public Deck<BodyPlaceCard> generate()
		{
			Deck<BodyPlaceCard> cards = new Deck<BodyPlaceCard>();
			foreach (var gameNode in map.getNodes())
			{
				if (!map.isSpecialNode(gameNode))
				{
					cards.AddLast(new BodyPlaceCard(game, gameNode));
				}
			}
			cards.shuffle();
			return cards;
		}

	}*/
}
