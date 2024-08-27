
using CardGame.Lib.EventSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace CardGame.Lib.EventSystem
{
    public class EventTrigger<T> : Trigger<T>
    {
        EventManager eveSys;
        bool isInit = false;
        public EventTrigger(EventManager eveSys, Action<T> Action, int priority) : base(Action, priority)
        {
            this.eveSys = eveSys;
        }

        public void init()
        {
            if (!isInit)
            {
                eveSys.addTrigger(this);
                isInit = true;
            }
        }
    }
}
