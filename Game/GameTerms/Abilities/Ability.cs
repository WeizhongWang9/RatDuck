using CardGame.Game.GameEvents;
using CardGame.Game.GameTerms.Units;
using CardGame.GameEvents.UnitEvents;
using CardGame.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardGame.Game.GameTerms.Abilities
{
	public class eventDictionary<TVALUE> : Dictionary<Unit,TVALUE>
	{
		public eventDictionary()
		{
			GameTrigger<UnitRemoved> onRemoved = new GameTrigger<UnitRemoved>(onUnitRemoved, 900);
		}
		
		public void onUnitRemoved(UnitRemoved e)
		{
			Remove(e.unit );
		}
	}

	public class eventBidictionary<TVALUE> : Bidictionary<Unit, TVALUE>
	{
		public eventBidictionary()
		{
			GameTrigger<UnitRemoved> onRemoved = new GameTrigger<UnitRemoved>(onUnitRemoved, 900);
		}

		public void onUnitRemoved(UnitRemoved e)
		{
			remove(e.unit);
		}

	}
	public class DelTrigger<Event>
	{
		Action<Event> act;
		protected GameTrigger<Event> trig;
		public DelTrigger(Action<Event> act, int weight)
		{
			trig = new GameTrigger<Event>(act, weight);
		}
		public void setActive(bool setOn)
		{
			trig.setActive(setOn);
		}
	}

	public abstract class AbilityTrig<Event, Ability>
	{
		protected GameTrigger<Event> trig;
		protected Ability ability;
		public AbilityTrig(Ability ability, int weight)
		{
			trig = new GameTrigger<Event>(act,weight);
			this.ability = ability;
		}
		protected abstract void act(Event unitEvent);
	}

	public class AbilityTrigDel<Event,Ability> : DelTrigger<Event>
	{
		Action<Event> act;
		protected Ability ability;
		public AbilityTrigDel(Ability ability, Action<Event> act, int weight) :base(act, weight) 
		{
			this.ability = ability;
		}
	}



}
