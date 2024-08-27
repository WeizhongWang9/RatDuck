using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardGame.Lib.EventSystem.Order
{
	/// <summary>
	/// An order is a validation. 
	/// </summary>
	public abstract class Order<T>
	{
		EventManager eveSys;
		public abstract bool check(T e, out string errorText);
		public Order(EventManager eveSys) 
		{ 
			this.eveSys = eveSys;
		}
		public bool tryCall(T item, out string errorText)
		{
			if (check(item,out errorText))
			{
				eveSys.emitEvent(item);
				return true;
			}
			return false;
		}
	}
	


}
