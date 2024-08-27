using CardGame.Game.GameTerms.Abilities;
using CardGame.Game.GameTerms.PlayerUtility;
using CardGame.GameEvents.UnitEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardGame.Game.GameTerms.Units
{
	public class Card : Unit
	{
		public Card(Game game, Team team) : base(game)
		{
			var handle = game.abilityHandle;
			handle.abilityVisibiliy.init(this, team);
		}
	}

	public class BodyRelease : UnitEvents
	{
		public BodyRelease(Body unit) : base(unit)
		{
			
		}
	}
	

	public class BodyPlaceCard : Card
	{
		public GameNode gameNode;
		public BodyPlaceCard(Game game, GameNode gameNode) : base(game, game.playerHandle.govTeam)
		{
			var handle = game.abilityHandle;
			this.gameNode = gameNode;
		}
	}

	public class MartketCard : Card
	{
		public MartketCard(Game game, Team team, int cost) : base(game,team)
		{
			var handle = game.abilityHandle;
			handle.abilityCost.init(this, team, cost);
		}
	}
}
