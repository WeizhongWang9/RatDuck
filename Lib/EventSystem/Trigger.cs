using System;
namespace CardGame.Lib.EventSystem
{
	public interface ITrigger
	{
		bool isActive();
	}
	public class Trigger<T> : ITrigger
	{
		int priority;
		bool active;
		Action<T> action;
		public Trigger(Action<T> action, int priority)
		{
			active = true;
			this.action = action;
			this.priority = priority;
		}

		public void setActive(bool active) { this.active = active; }
		public bool isActive() { return active; }
		public void execute(T info) { action?.Invoke(info); }
		public int getPriority() { return priority; }
	}

}
