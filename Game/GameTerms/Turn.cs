using CardGame.Game.GameEvents;
using CardGame.Game.GameTerms.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardGame.Game.GameTerms
{
	public class GameEvent:Event
	{
		public Game game { get; }
		public GameEvent(Game game)
		{ 
			this.game = game;
			Global.getEventManager().emitEvent(this);
		}
	}
	public class Turn : GameEvent
	{
		public Player nextPlayer;

		public Turn(Game game, Player nextPlayer) : base(game)
		{
			this.nextPlayer = nextPlayer;
			Global.getEventManager().emitEvent(this);
		}
	}

	public class PlayerAct : GameEvent
	{
		public Player curPlayer;
		public PlayerAct(Game game,Player curPlayer) : base(game)
		{
			this.curPlayer = curPlayer;
			Global.getEventManager().emitEvent(this);
		}
	}

	public class PlayerTurnManager
	{
		Player curPlayer;
		List<Player> played = new List<Player>();
		List<Player> unplayed;
		public PlayerTurnManager(List<Player> unplayed, Player first)
		{
			this.unplayed = unplayed;
			this.curPlayer = first;
			played.Add(first);
			unplayed.Remove(first);

		}
		void PlayerPlayed(Player player) { played.Add(player); unplayed.Remove(player); }

		public void newTurn(Player first) {
			if (isNewTurn())
			{
				unplayed = played;
				played = new List<Player>();
				newPlayer(first);
			}
			else { throw new Exception("some players are unplayed."); }
		}
		public bool isNewTurn() { return unplayed.Count == 0; }
		public List<Player> getUnplayed() { return unplayed; }
		public void newPlayer(Player first)
		{
			PlayerPlayed(first);
			curPlayer = first;
		}
		public Player getCurPlayer() { return curPlayer; }

		


	}

	public class TurnManager
	{
		Game game;
		public PlayerStates playerStates;
		PlayerTurnManager playerTurnManager;
		public TurnManager(Game game)
		{
			this.game = game;
		}
		public void init(List<Player> unplayed, Player first)
		{
			this.playerTurnManager = new PlayerTurnManager(unplayed, first);
			new Turn(game, first);
		}


		public enum PlayerStates { Begin, Premove, PreAction, AfterAction }

		public Player curPlayer { get { return playerTurnManager.getCurPlayer(); } }
		public List<Player> unplayers { get { return playerTurnManager.getUnplayed(); } }

		public PlayerStates getPlayerStates()
		{
			return playerStates;
		}
		public bool isReadyEndTurn() { return playerStates == PlayerStates.AfterAction; }
		public void endTurn(Player first)
		{
			if(playerTurnManager.isNewTurn())
				playerTurnManager.newTurn(first);
			else playerTurnManager.newPlayer(first);
			playerStates = PlayerStates.Begin;
			new Turn(game, first);
		}
		public PlayerStates nextStates()
		{
			if(playerStates != PlayerStates.AfterAction)
				playerStates = (PlayerStates)playerStates + 1;
			return playerStates;
		}

	}
}
