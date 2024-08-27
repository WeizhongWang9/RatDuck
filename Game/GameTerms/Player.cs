//using CardGame.Game.GameTerms.Cards.Utility;
using CardGame.Game.GameTerms.Abilities;
using CardGame.Game.GameTerms.PlayerUtility;
using CardGame.Game.GameTerms.Units;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardGame.Game.GameTerms
{
	public enum Roles { Police, Scientist, Dissent}
	public class Player : Unit
	{
		Team _team;
		public Team team => _team;
		Role _controlUnit;
		public Units.Role controlUnit { 
			get 
			{ 
				return _controlUnit; 
			} 
			set 
			{ 
				if(_controlUnit != null)
				controlUnitToPlayer.Remove(_controlUnit); 
				controlUnitToPlayer.Add(value, this); 
				_controlUnit = value; 
			} 
		}

		static Dictionary<Role, Player> controlUnitToPlayer = new Dictionary<Role, Player>();

		public static Player getPlayer(Role controlUnit)
		{
			return controlUnitToPlayer[controlUnit];
		}

		public Player(Game game,Roles role):base(game)
		{
			var govTeam = game.playerHandle.govTeam;
			var dissTeam = game.playerHandle.dissTeam;
			var allPlayer = game.playerHandle.allPlayer;
			switch (role)
			{
				case Roles.Police:
					controlUnit = new Police(game, this);
					govTeam.AddPlayer(this);
					new TokenCreate(controlUnit,game.map.policeStation);
					_team = govTeam;
					break;
				case Roles.Scientist:
					controlUnit = new Scientist(game, this);
					new TokenCreate(controlUnit, game.map.lab);
					govTeam.AddPlayer(this);
					_team= govTeam;
					break;
				case Roles.Dissent:
					controlUnit = new Diss(game, this);
					new TokenCreate(controlUnit, game.map.lab);
					dissTeam.AddPlayer(this);
					_team = dissTeam;
					break;
				default:
					throw new ArgumentException("Unexpected Role types");
			}
			allPlayer.AddPlayer(this);
		}
		void init(Game game)
		{
			var handle = game.abilityHandle;
			handle.abilityCardSlot.init(this);

		}


	}

	

}
