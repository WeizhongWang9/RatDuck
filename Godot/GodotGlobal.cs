using CardGame.Game.GameTerms;
using CardGame.Game.GameTerms.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardGame.Game;
using static CardGame.Game.GameTerms.TurnManager;
using System.Runtime.CompilerServices;
using static RatDuck.Script.Godot.GodotGlobal;
using Godot;
using Newtonsoft.Json;
using static RatDuck.Script.Godot.TokenLabelPreset;
using CardGame.Lib.Deck;
namespace RatDuck.Script.Godot
{
	public class GodotGlobal
	{
		private static GodotGlobal _godotGlobal;
		public static GodotGlobal godotGlobal
		{
			get
			{
				if (_godotGlobal == null)
				{ _godotGlobal = new GodotGlobal(); }
				return _godotGlobal;
			}
		}

		GodotGlobal() { initTerm(); }
		public List<PlayerInfo> playerInfos = new List<PlayerInfo>();

		static string[] Term = new string[100];
		static string[] stateTerm = new string[4];
		public enum Terms { arrest, move, Premove, BeforeAction, AfterAction, speech, research, control,release, kidnap }

		void initTerm()
		{
			Term[(int)Terms.arrest] = "arrests";
			Term[(int)Terms.move] = "moves to";
			Term[(int)Terms.speech] = "speeches at";
			Term[(int)Terms.research] = "researches at";
			Term[(int)(Terms.control)] = "controls at";
			Term[(int)Terms.release] = "releases a civilian at";
			Term[(int)Terms.kidnap] = "kidnaps a civilian at";
			stateTerm[(int)TurnManager.PlayerStates.Premove] = "Premove";
			stateTerm[(int)TurnManager.PlayerStates.PreAction] = "BeforeAction";
			stateTerm[(int)TurnManager.PlayerStates.AfterAction] = "AfterAction";
		}
		public string getTerm(Terms term)
		{
			return Term[(int)term].ToString();
		}
		public string getStateTerm(PlayerStates playerStates)
		{
			return stateTerm[(int)playerStates].ToString();
		}


		public string getPlayerState(Player player, PlayerStates terms)
		{
			return "Current Player : " + playerToPlayerInfo[player].name + "| Current State : " + getStateTerm(terms);
		}

		public string getActionString(Player player1, Terms verb, Player player2)
		{
			var p1name = playerToPlayerInfo[player1].name;
			var p2name = playerToPlayerInfo[player2].name;
			var verbString = getTerm(verb);
			return p1name + " " + verbString + " " + p2name + "\n";
		}
		public string getActionString(Player player1, Terms verb, GameNode gameNode)
		{
			var p1name = playerToPlayerInfo[player1].name;
			var gameNodeName = gamenodeToInfo[gameNode].name;
			var verbString = getTerm(verb);
			return p1name + " " + verbString + " " + gameNodeName + "\n";
		}




		public class PlayerInfo
		{

			public Player player;
			public string name;
			public Roles role;
			static string[] roleString = new string[3];
			bool firstRun = false;

			public PlayerInfo(Player player, string name, Roles role)
			{
				this.player = player;
				this.name = name;
				this.role = role;
				if (!firstRun) { firstRun = true; init(); }

			}

			static void init()
			{
				roleString[(int)Roles.Police] = "Police";
				roleString[(int)Roles.Dissent] = "Dissent";
				roleString[(int)Roles.Scientist] = "Scientist";
			}
			public PlayerInfo(string name, Roles role) : this(null, name, role) { }
			public string getRoleString(Roles role)
			{
				return roleString[(int)role].ToString();
			}
			public string getPlayerInfoString()
			{
				var s = getRoleString(role) + " " + name;
				if (role == Roles.Dissent) { s += getWantedString(player.controlUnit); }
				Global.getAbilityHandle().abilityMove.tryGetAttribute(player.controlUnit, out var att);
				s += "| Movepoint : " + att.movePoint;
				return s;
			}

			public string getWantedString(Unit unit)
			{
				Global.getAbilityHandle().abilityDiss.tryGetWanted(unit, out var wanted);
				return " | Wanted = " + wanted;
			}

		}

		public class GameNodeInfo
		{
			public string name;
			public GameNodeInfo(string name)
			{
				this.name = name;
			}
		}
		Dictionary<Player, PlayerInfo> playerToPlayerInfo = new Dictionary<Player, PlayerInfo>();
		Dictionary<GameNode, GameNodeInfo> gamenodeToInfo = new();
		public PlayerInfo getPlayerInfo(Player player) { return playerToPlayerInfo[player]; }
		public GameNodeInfo GetGameNodeInfo(GameNode gameNode) { return gamenodeToInfo[gameNode]; }
		public void addPlayerInfo(Player player, PlayerInfo info) { playerToPlayerInfo.Add(player, info); }
		public void addGameNodeInfo(GameNode gameNode, GameNodeInfo info) { gamenodeToInfo.Add(gameNode, info); }


		public string getBodyInfo(Body body)
		{
			return "Body" + body.name;
		}

		public string getLocationCardMaxWarning()
		{
			return "Location cards reach maximum amount of 2";
		}


		public TokenLabelPreset tokenLabelPreset = new();


		Dictionary<Card, CardInfo> dic_card_to_cardInfo = new Dictionary<Card, CardInfo>();
		public void link_card_to_cardInfo(Card card, CardInfo cardInfo) { dic_card_to_cardInfo.Add(card, cardInfo); }
		public CardInfo get_cardInfo(Card card) { return dic_card_to_cardInfo[card]; }
		public bool remove_cardInfo(Card card) { return dic_card_to_cardInfo.Remove(card); }

	}
}
