using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CardGame.Game.GameTerms.Skills;
/*
namespace CardGame.Game.GameTerms.Skills
{
    public abstract class Selector : ITargetSelector
    {
        protected LinkedList<Target> targets;

        protected virtual LinkedList<Target> interGetTargets()
        {
            return targets;
        }

        public LinkedList<Target> getTargets()
        {
            return interGetTargets();
        }
    }

    public class ListSelect : Selector
    {
        public ListSelect(params Unit[] targets)
        {
            foreach (Unit unit in targets)
            {
                this.targets.AddLast(new Target(unit));
            }
        }
        public ListSelect(params Player[] targets)
        {
            foreach (Player unit in targets)
            {
                this.targets.AddLast(new Target(unit));
            }
        }
        public ListSelect(params GameNode[] targets)
        {
            foreach (GameNode unit in targets)
            {
                this.targets.AddLast(new Target(unit));
            }
        }

        public LinkedList<Target> Add(params Unit[] targets)
        {
            foreach (Unit unit in targets)
            {
                this.targets.AddLast(new Target(unit));
            }
            return this.targets;
        }
        public LinkedList<Target> Add(params Player[] targets)
        {
            foreach (Player unit in targets)
            {
                this.targets.AddLast(new Target(unit));
            }
            return this.targets;
        }
        public LinkedList<Target> Add(params GameNode[] targets)
        {
            foreach (GameNode unit in targets)
            {
                this.targets.AddLast(new Target(unit));
            }
            return this.targets;
        }

    }

}
*/