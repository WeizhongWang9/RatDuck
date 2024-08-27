using CardGame.Game.GameEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardGame.Game.GameTerms.Abilities
{
    public class AbilitySingleProperty<T> : EventSystem
    {
        eventDictionary<Data> data = new eventDictionary<Data>();

        public AbilitySingleProperty(Game game) : base(game) { }

        public void setAbility(Unit unit)
        {
            setAbility(unit, new Attribute());
        }
        public void setAbility(Unit unit, Attribute attribute)
        {
            var info = new Data();
            info.attribute = attribute;
            data[unit] = info;
        }

        public bool hasAbility(Unit unit) { return data.ContainsKey(unit); }

        public Attribute getAttribute(Unit unit)
        {
            if (data.TryGetValue(unit, out var dt))
                return dt.attribute;
            return null;
        }
        class Data
        {
            public Attribute attribute;
        }
        public class Attribute
        {
            T property;
            public Attribute(T property) { this.property = property; }
            public Attribute() : this(default) { }
        }
    }

    public class AbilityNonNegativeSingle : EventSystem
    {
        public AbilityNonNegativeSingle(Game game) : base(game) { }

        eventDictionary<Data> data = new eventDictionary<Data>();

        public void setAbility(Unit unit)
        {
            setAbility(unit, new Attribute());
        }
        public void setAbility(Unit unit, Attribute attribute)
        {
            var info = new Data();
            info.attribute = attribute;
            data[unit] = info;
        }

        public bool hasAbility(Unit unit) { return data.ContainsKey(unit); }

        public Attribute getAttribute(Unit unit)
        {
            if (data.TryGetValue(unit, out var dt))
                return dt.attribute;
            return null;
        }
        class Data
        {
            public Attribute attribute;
        }
        public class Attribute
        {
            int _property;
            int property 
            { 
                get { return _property; } 
                set 
                { 
                    if (value < 0) _property = 0;
                    _property = value; 
                } 
            }
            public Attribute(int property) { this.property = property; }
            public Attribute() : this(0) { }
        }
    }
}
