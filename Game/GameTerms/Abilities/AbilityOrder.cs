/*
 using CardGame.Game.GameEvents;
using CardGame.Game.Utilities;
using CardGame.GameEvents.UnitEvents;
using CardGame.Lib.EventSystem.Order;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Windows.Forms;

namespace CardGame.Game.GameTerms.Abilities
{
/*
/*
 A abilityOrder takes an skill(or interface related) and initiate a unitOrder event.

 At time=Priority, it 
 1.checks the targets and filter out units through filters
    1.1 if the remains does not satisfy activation condition, terminate. print.
    1.2 multiple filters may perform on it, 
 2.for remains, apply the skill effect on them.
 */
/*

    public class OrderData
    {
        public LinkedList<Target> targets;
        public IOrder<UnitOrder> order;
        public OrderData(LinkedList<Target> targets, IOrder<UnitOrder> order)
        {
            this.targets = targets;
            this.order = order;
        }
    }
    public class AbilityOrder : EventSystem
    {
        eventDictionary<Data> data = new eventDictionary<Data>();
        
        public void setAbility(Unit unit)
        {
            data.Add(unit, new Data());
        }

        public AbilityOrder(Game game) : base(game)
        {
            int priority = 1000;
            GameTrigger<UnitOrder> trig = new GameTrigger<UnitOrder>(onUnitOrder, priority);
        }

        public void Order(Unit unit, List<OrderData> data)
        {
            new UnitOrder(unit, data);
        }

        void onUnitOrder(UnitOrder unitOrder)
        {   
            var trigUnit = unitOrder.getTriggerUnit();
            var orderDatas = unitOrder.data;
            var curProcess = -1;
            foreach (OrderData orderData in orderDatas)
            {
                curProcess++;
                unitOrder.setCurProcessingIndex(curProcess);
                var orderTargets = orderData.targets;
                var order = orderData.order;
                string fillterErrorStr = "";
                if (!data.ContainsKey(trigUnit)) { return; }
                var cur = orderTargets.Last;
                while (cur != null)
                {
                    var prev = cur.Previous;
                    var target = cur.Value;
                    if (!order.filter(target, out fillterErrorStr))
                    {
                        orderTargets.Remove(prev);
                    }
                    cur = prev;
                }
                if (!order.activationCondition(orderTargets, out unitOrder.activeError))
                {
                    return;
                }
                order.skillEffect(unitOrder);
            }
        }
        class Data { }

    }
}
*/