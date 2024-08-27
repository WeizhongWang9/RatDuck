using CardGame.Game.GameEvents;
using CardGame.Game.GameTerms.PlayerUtility;
using CardGame.Lib.EventSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace CardGame.Game.GameTerms
{
	public class PlayerHandle:HandleManager<Player>
	{
		public Team allPlayer;
		public Team govTeam;
		public Team dissTeam;

		public Team getOppositeTeam(Team team)
		{
			if (team == govTeam) return dissTeam;
			if (team == dissTeam) return govTeam;
			else throw new NotImplementedException();
		}

		public PlayerHandle()
		{
			govTeam = new Team();
			dissTeam = new Team();
			allPlayer = new Team();
		}
		public Player getGameFirstPlayer()
		{
			if (dissTeam.getPlayers().Count > 0)
				return dissTeam.getPlayers()[0];
			else
				return allPlayer.getPlayers()[0];
		}
	}
}
