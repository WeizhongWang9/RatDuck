using CardGame.Game.GameEvents;
using CardGame.Game.GameTerms.Units;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardGame.Game.GameTerms.Abilities
{
    public class ChaoticMeasureAdd : Event
    {
        public GameNode gameNode;
        public int value;
        public ChaoticMeasureAdd(GameNode gameNode, int value)
        {
            this.value = value;
            this.gameNode = gameNode;
            Global.getEventManager().emitEvent(this);
        }
    }
    public class CONST_ABILITY_CHAOTICMEASURE
    {
        public const int MAXMEASURE = 3;
        public const int WEIGHT = 0;
    }
    public class AbilityChaoticMeasure : EventSystem
    {
        eventDictionary<Data> data = new eventDictionary<Data>();
        DelTrigger<ChaoticMeasureAdd> gameTrigger;
        public AbilityChaoticMeasure(Game game) : base(game) {
            gameTrigger = new DelTrigger<ChaoticMeasureAdd>(on_chaotic_measure_add, CONST_ABILITY_CHAOTICMEASURE.WEIGHT);
        }

        

        void on_chaotic_measure_add(ChaoticMeasureAdd @event)
        {
            var value = @event.value;
            var gameNode = @event.gameNode;
            _addMeasure(gameNode, value);
        }

        public void init(GameNode unit)
        {
            data.Add(unit, new Data());
        }

        public bool hasAbility(GameNode unit) { return data.ContainsKey(unit); }

        public bool tryGetValue(GameNode unit,out int value)
        {
            if(data.TryGetValue(unit,out var dt))
            {
                value = dt.property;
                return true;
            }
            value = -1;
            return false;
        }

        void _addMeasure(GameNode gameNode, int value)
        {
            if (data.TryGetValue(gameNode, out var dt))
            {
                dt.property += value;
            }
        }
        public void addMeasure(GameNode gameNode, int value)
        {
            new ChaoticMeasureAdd(gameNode, value);
        }

        public int this[GameNode unit]
        {
            get { if (tryGetValue(unit, out var value)) return value; return -1; }
        }

        class Data
        {
            public const int MAXMEASURE = CONST_ABILITY_CHAOTICMEASURE.MAXMEASURE;
            int _property;
            public int property
            {
                get { return _property; }
                set
                {
                    if (value < 0) _property = 0;
                    else if (value > MAXMEASURE) _property = MAXMEASURE;
                    else _property = value;
                }
            }
        }
    }
}
