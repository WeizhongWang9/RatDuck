using CardGame.Game.GameTerms;
using CardGame.Lib.EventSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace CardGame.Game.GameEvents
{
    public abstract class Event
    {
        public Event() {
            Global.getEventManager().emitEvent(this);
        }
        protected UnitHandle UnitHandles => Global.getGame().unitHandle;
    }

    public class GameTrigger<T>
    {
        protected EventTrigger<T> eventTrigger;
        static EventManager eventManager { get { return Global.getEventManager(); } }
        public GameTrigger(Action<T> Action, int priority)
        {
            eventTrigger = new EventTrigger<T>(eventManager,Action, priority);
            eventTrigger.init();
        }

        public void setActive(bool active)
        {
            eventTrigger.setActive(active);
        }
    }
}

