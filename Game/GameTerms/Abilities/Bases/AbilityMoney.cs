using CardGame.Game.GameEvents;
using CardGame.Game.GameTerms.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardGame.Game.GameTerms.Abilities
{
    
    public class AbilityMoney : EventSystem
    {
        public AbilityMoney(Game game) : base(game) { }

        eventDictionary<Data> data = new eventDictionary<Data>();

        public void init(Unit unit)
        {
            data.Add(unit, new Data());
        }

        public bool hasAbility(Unit unit) { return data.ContainsKey(unit); }

        public bool tryGetValue(Unit unit, out int value)
        {
            if (data.TryGetValue(unit, out var dt))
            {
                value = dt.property;
                return true;
            }
            value = -1;
            return false;
        }
        public void addValue(Unit unit, int value)
        {
            if (data.TryGetValue(unit, out var dt))
            {
                dt.property += value;
            }
        }
        public void setValue(Unit unit, int value)
        {
            if (data.TryGetValue(unit, out var dt))
            {
                dt.property = value;
            }
        }

        class Data
        {
            int _property;
            public int property { 
                get { return _property; }
                set
                {
                    if (value >= 0)
                    {
                        _property = value;
                    }
                    _property = 0;
                }
            }
        }
    }
}
