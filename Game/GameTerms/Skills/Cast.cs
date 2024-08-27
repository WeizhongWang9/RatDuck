/*
using CardGame.Game.Utilities;
using CardGame.GameEvents.UnitEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CardGame.Lib.EventSystem.Order;
using CardGame.Game.GameTerms.Abilities;
using System.Windows.Forms;
namespace CardGame.Game.GameTerms.Skills
{
    public interface ITargetSelector
    {
        LinkedList<Target> getTargets();
    }

    public delegate bool ActiveCondition(LinkedList<Target> targets, out string errorStr);
    public delegate bool Filter(Target target, out string errorStr);
    public delegate bool Effect(UnitOrder order);
    internal class CastToOrder : IOrder<UnitOrder>
    {
        ActiveCondition castCondition;
        Filter delefilter;
        Effect effect;
        public virtual bool activationCondition(LinkedList<Target> targets, out string errorStr)
        {
            return castCondition(targets, out errorStr);
        }

        public bool filter(Target target, out string errorStr)
        {
            return delefilter(target, out errorStr);
        }

        public void skillEffect(UnitOrder order)
        {
            effect(order);
        }

        public CastToOrder(ActiveCondition castCondition, Filter delefilter, Effect effect)
        {
            this.castCondition = castCondition;
            this.delefilter = delefilter;
            this.effect = effect;
        }
    }
    


    public class Cast
    {
        //public abstract void onSkillBegin(UnitOrder order);
        OrderData beginSkill(ITargetSelector caster)
        {
            ActiveCondition orderTargets = beginActiveCondition;
            Filter filter = nullFilter;
            Effect effect = beginEffect;
            var skillToOrder = new CastToOrder(orderTargets, filter, effect);
            var targets = caster.getTargets();
            return new OrderData(targets, skillToOrder);
        }
        List<OrderData> TakeEffect(ITargetSelector targetSelector)
        {
            var targets = targetSelector.getTargets();
            var filters = getFilters();
            var filtersCount = filters.Count;
            var List = new List<OrderData>();
            Effect effect = passFilterTarget;
            foreach (var filter in filters)
            {
                var iOrder = new CastToOrder(nonZeroCondition, filter, effect);
                List.Add(new OrderData(targets, iOrder));
            }
            var takeEffect = new CastToOrder(effectActiveCondition, nullFilter, effectEffect);
            List.Add(new OrderData(targets, takeEffect));
            return List;
        }
        Unit triggerUnit;
        ITargetSelector caster;
        ITargetSelector targetSelector;
        List<OrderData> OrderCast()
        {
            List<OrderData> list = new List<OrderData>();
            list.Add(beginSkill(caster));
            list.AddRange(TakeEffect(targetSelector));
            return list;
        }
        bool nonZeroCondition(LinkedList<Target> targets, out string errorStr)
        {
            errorStr = null;
            return (targets.Count != 0);
        }
        bool passFilterTarget(UnitOrder order)
        {
            var curProcess = order.getCurProcessingIndex();
            var orderData = order.data;
            var targets = orderData[curProcess].targets;
            orderData[curProcess + 1].targets = targets;
            return true;
        }
        protected virtual bool beginActiveCondition(LinkedList<Target> targets, out string errorStr)
        {
            errorStr = null;
            return true;
        }
        protected virtual bool effectActiveCondition(LinkedList<Target> targets, out string errorStr)
        {
            errorStr = null;
            return true;
        }
        protected virtual bool beginEffect(UnitOrder order)
        {
            return true;
        }
        protected virtual bool effectEffect(UnitOrder order)
        {
            return true;
        }
        protected virtual List<Filter> getFilters()
        { 
            var list = new List<Filter>();
            return list;
        }
        protected bool nullFilter(Target target, out string errorStr)
        { errorStr = null; return true; }
        protected LinkedList<Target> getTargets(UnitOrder order)
        {
            var index = order.getCurProcessingIndex();
            return order.data[index].targets;
        }
        protected Cast(Unit triggerUnit, ITargetSelector caster, ITargetSelector targetSelector)
        {
            this.triggerUnit = triggerUnit;
            this.caster = caster;
            this.targetSelector = targetSelector;
            new UnitOrder(triggerUnit,OrderCast());
        }
    }




}
*/