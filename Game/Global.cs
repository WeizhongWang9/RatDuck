using CardGame.Game.GameEvents;
using CardGame.Game.GameTerms;
using CardGame.Game.GameTerms.Units;
using CardGame.Lib.EventSystem;
using RatDuck.Script.Game.GameTerms.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardGame.Game
{
	public class Global
	{
		static Global _global;
		public static Global global
		{
			get { 
				if (_global == null) { _global = new Global(); _global.init(); }
				return _global;
			}
		}

		public static Global Get() => global;
		public static Game getGame() => Get().game;
		public static EventManager getEventManager() => getGame().eventManager;
		public static UnitHandle getUnitHandle() => getGame().unitHandle;
		public static AbilityHandle getAbilityHandle() => getGame().abilityHandle;
		public static TurnManager getTurnManager() => getGame().turnManager;
		public static Map GetMap() => getGame().map;
		public static PlayerHandle playerHandle() => getGame().playerHandle;
		Game game;
		Global()
		{
		}

		void init()
		{
			game = new Game();
			game.init();
		}

		

	}
}
