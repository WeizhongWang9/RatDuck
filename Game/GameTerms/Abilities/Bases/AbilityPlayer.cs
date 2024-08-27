using CardGame.Game.GameEvents;
using CardGame.Game.GameTerms.Units;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CardGame.Game.GameTerms.Abilities.Bases
{
    public class AbilityPlayer : EventSystem
    {
        public AbilityPlayer(Game game) : base(game) { }

        eventDictionary<Data> data = new eventDictionary<Data>();

        public Player this[Unit unit]
        {
            get { if (tryGetValue(unit, out var value)) return value; return null; }
            set { trySetValue(unit, value); }
        }

        public void init(Unit unit, Player player)
        {
            var dt = new Data();
            dt.player = player;
            data.Add(unit, dt);
        }

        public bool trySetValue(Unit unit, Player value)
        {
            if(hasAbility(unit))
            {
                data[unit].player = value;
                return true;
            }
            return false;
        }

        public bool hasAbility(Unit unit) { return data.ContainsKey(unit); }

        public bool tryGetValue(Unit unit, out Player value)
        {
            if (data.TryGetValue(unit, out var dt))
            {
                value = dt.player;
                return true;
            }
            value = null;
            return false;
        }

        class Data
        {
            public Player player;
        }
    }
}
