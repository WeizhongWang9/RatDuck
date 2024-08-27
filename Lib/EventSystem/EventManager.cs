using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardGame.Lib.EventSystem
{
	public class EventManager
	{
		readonly Dictionary<Type, EventTypeManager> EventToTriggerManager = new Dictionary<Type, EventTypeManager>();

		public void addTrigger<T>(Trigger<T> trig)
		{
			if (!EventToTriggerManager.TryGetValue(typeof(T), out var manager))
			{
				manager = new EventTypeManager();
				EventToTriggerManager.Add(typeof(T), manager);
			}
			manager.addTrigger(trig);
		}
		public void removeTrigger<T>(Trigger<T> trig)
		{
			if (EventToTriggerManager.TryGetValue(typeof(T), out var manager))
			{
				manager.removeTrigger(trig);
				if (manager.isEmpty())
				{
					EventToTriggerManager.Remove(typeof(T));
				}
			}
		}
		public void emitEvent<T>(T e)
		{
			if (EventToTriggerManager.TryGetValue(typeof(T), out var manager))
			{
				manager.emitEvent(e);
			}

		}

		public void emitEventAfter<T>(T e, int priority)
		{
			if (EventToTriggerManager.TryGetValue(typeof(T), out var manager))
			{
				manager.emitEventAfter(e, priority);
			}
		}
		public void emitEventBefore<T>(T e, int priority)
		{
			if (EventToTriggerManager.TryGetValue(typeof(T), out var manager))
			{
				manager.emitEventBefore(e, priority);
			}
		}
	}
	internal class EventTypeManager
	{
		readonly SortedDictionary<int, List<ITrigger>> Triggers = new SortedDictionary<int, List<ITrigger>>();
		public bool isEmpty()
		{
			return Triggers.Count == 0;
		}
 
		public void addTrigger<T>(Trigger<T> trig)
		{
			var priority = trig.getPriority();
			if (!Triggers.TryGetValue(priority, out List<ITrigger> list))
			{
				list = new List<ITrigger>();
				Triggers.Add(priority, list);
			}
			list.Add(trig);
		}

		public void removeTrigger<T>(Trigger<T> trig)
		{
			var priority = trig.getPriority();
			if (Triggers.TryGetValue(priority, out List<ITrigger> list))
			{
				list.Remove(trig);
				if (list.Count == 0)
				{
					Triggers.Remove(priority);
				}
			}
		}
		public void emitEvent<T>(T e)
		{
			foreach (var ele in Triggers)
			{
				var triggerList = ele.Value;
				foreach (var Itrig in triggerList)
				{
					var trigger = Itrig as Trigger<T>;
					if (trigger.isActive()) 
					{ 
						trigger.execute(e); }
				}
			}
		}

		public void emitEventAfter<T>(T e, int priority)
		{
			foreach (var ele in Triggers)
			{
				var triggerList = ele.Value;
				var firstTrig = triggerList.First() as Trigger<T>;
				if (firstTrig.getPriority() >= priority)
				{
					foreach (var Itrigger in triggerList)
					{
						var trigger = Itrigger as Trigger<T>;
						if (trigger.isActive()) { trigger.execute(e); }
					}
				}
			}
		}
		public void emitEventBefore<T>(T e, int priority)
		{
			foreach (var ele in Triggers)
			{
				var triggerList = ele.Value;
				var firstTrig = triggerList.First() as Trigger<T>;
				if (firstTrig.getPriority() < priority)
				{
					foreach (var Itrigger in triggerList)
					{
						var trigger = Itrigger as Trigger<T>;
						if (trigger.isActive()) { trigger.execute(e); }
					}
				}
			}
		}
	}

	
}
