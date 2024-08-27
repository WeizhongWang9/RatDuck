using CardGame.Game.GameTerms.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardGame.Game.GameTerms.Units
{

	public struct gameNodeRelation
	{
		public GameNode gameNode1;
		public GameNode gameNode2;
		public gameNodeRelation(GameNode gameNode1, GameNode gameNode2)
		{
			this.gameNode2 = gameNode2;
			this.gameNode1 = gameNode1;
		}
	}
	
	public class GameNode : Unit
	{
		AbilityGameNode abilityGameNode;

		public GameNode(Game game):base(game)
		{
			abilityGameNode = game.abilityHandle.abilityGameNode;
			init();

		}

		void init()
		{
			abilityGameNode.initGameNode(this);
		}
	   
	}
}
