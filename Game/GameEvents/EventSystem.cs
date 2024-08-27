using CardGame.Game.GameTerms;
using CardGame.Lib.EventSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardGame.Game.GameEvents
{
    public abstract class EventSystem
    {
        Game game;
        protected UnitHandle unitHandle => game.unitHandle;
        protected EventManager eventHandle => game.eventManager;

        public EventSystem(Game game) {
            this.game = game;
        }
    }
}
