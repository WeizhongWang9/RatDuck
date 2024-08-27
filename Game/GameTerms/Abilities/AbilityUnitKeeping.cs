using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CardGame.Game.GameEvents;
namespace CardGame.Game.GameTerms.Abilities
{
    public class AbilityUnitKeeping : EventSystem
    {
        protected eventDictionary<Data> datas = new eventDictionary<Data>();

        public AbilityUnitKeeping(Game game) : base(game)
        {
        }

        protected class Data
        {
            public Attribute attribute;
        }

        public Attribute getAttribute(Unit unit)
        {
            if(datas.TryGetValue(unit,out Data data))
            {
                return data.attribute;
            }
            return null;
        }
        public void setData(Unit unit, Attribute attribute)
        {
            var data = new Data();
            data.attribute = attribute;
            datas.Add(unit, data);
        }
        public void setData(Unit unit)
        {
            setData(unit, new Attribute());
        }

        public class Attribute
        {
            public List<Unit> units;
            public Attribute(List<Unit> units)
            {
                this.units = units;
            }
            public Attribute() : this(new List<Unit>()) { }
            public void addUnit(Unit unit)
            {
                units.Add(unit);
            }
            public void removeUnit(Unit unit)
            {
                units.Remove(unit);
            }
        }

    }
}
