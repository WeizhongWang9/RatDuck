using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using CardGame.Game.GameTerms;

namespace GameProgress
{
	class GameStart
	{
		List<Player> players;
		List<Unit> units;
		Unit firstPlay;

		public GameStart(List<Player> players, List<Unit> units, Unit firstPlay)
		{
			this.players = players;
			this.units = units;
			this.firstPlay = firstPlay;
		}
	}
	/*
	class Game
	{
		int bankGov = 3;
		int bankDis = 3;
		public static void game()
		{
			GameNode gameNode =  new GameNode();
			Player playerA = new Player("A");
			Player playerB = new Player("B");
			Unit unitA = new Unit(null);
			Unit firstPlay = unitA;
			List<Player> players = new List<Player>();
			List<Unit> units= new List<Unit>();


			GameStart gameStart = new GameStart(players, units, firstPlay);

		}



	}*/

}
