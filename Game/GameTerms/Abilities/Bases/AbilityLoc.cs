using CardGame.Game.GameTerms.Units;
using CardGame.GameEvents.UnitEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CardGame.Game.GameEvents;
using System.IO.Pipes;

namespace CardGame.Game.GameTerms.Abilities
{
    public class UnitChangeLoc: UnitEvents
    {
        public GameNode toLoc;
        public UnitChangeLoc(Unit whichUnit, GameNode toLoc) : base(whichUnit)
        {
            this.toLoc = toLoc;
            Global.getEventManager().emitEvent(this);
        }

    }

    public class AbilityLoc : EventSystem
    {
        TrigLoc trigLoc;
        eventDictionary<Data> datas;

        class Data
        {
            public GameNode _loc;
        }

        public AbilityLoc(Game game):base(game)
        {
            trigLoc = new TrigLoc(this);
            datas = new eventDictionary<Data>();
        }
        public void initUnit(Unit unit)
        {
            var data = new Data();
            datas.Add(unit, data);
        }
        public GameNode this[Unit unit]
        {
            get { if (tryGetLoc(unit, out var loc)) return loc; return null; }
            set { changeLocation(unit, value); }
        }

        public void setActive(bool active) { trigLoc.setActive(active); }
        public void changeLocation(Unit whichUnit, GameNode toWhere) { new UnitChangeLoc(whichUnit, toWhere); }
        public bool tryGetLoc(Unit unit, out GameNode loc)
        {
            if(datas.TryGetValue(unit,out var data))
            {
                loc = data._loc;
                return true;
            }
            loc = null;
            return false;
        }
        class TrigLoc
        {
            GameTrigger<UnitChangeLoc> trig;
            GameTrigger<UnitLeaveMap> trigLeaveMap;

            AbilityLoc abilityLoc;
            public TrigLoc(AbilityLoc abilityLoc)
            {
                trig = new GameTrigger<UnitChangeLoc>(changeOfLoc, 1000);
                trigLeaveMap = new GameTrigger<UnitLeaveMap>(onUnitLeave, 1);
                this.abilityLoc = abilityLoc;
            }

            void onUnitLeave(UnitLeaveMap unitLeaveMap)
            {
                var unit = unitLeaveMap.getTriggerUnit();
                abilityLoc[unit] = null;
            }
            void changeOfLoc(UnitChangeLoc unitChangeLoc)
            {
                var unit = unitChangeLoc.getTriggerUnit();
                var loc = unitChangeLoc.toLoc;
                if (unit != null && abilityLoc.datas.TryGetValue(unit,out var data))
                {
                    data._loc = loc;
                }
            }
            public void setActive(bool active) { trig.setActive(active); }

        }
    }
}
