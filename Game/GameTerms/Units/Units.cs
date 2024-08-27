using CardGame.Game.GameTerms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardGame.Game.GameTerms
{

    public class Unit : InGameObject
    {
        protected Game game;
        public Unit(Game game)
        {
            this.game = game;
        }
    }
  
}
