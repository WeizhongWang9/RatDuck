using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardGame.Lib
{
	public class Bidictionary<T, U>
	{
		Dictionary<T, U> TtoU = new Dictionary<T, U>();
		Dictionary<U, T> UtoT = new Dictionary<U, T>();

		public void remove(T t)
		{
			if (TtoU.TryGetValue(t, out var tu))
			{
				UtoT.Remove(tu);
			}
			TtoU.Remove(t);
		}
		public void remove(U u)
		{
			if (UtoT.TryGetValue(u, out var ut))
			{
				TtoU.Remove(ut);
			}
			UtoT.Remove(u);
		}

		public void add(U u, T t)
		{
			add(t, u);
		}
		public void add(T t, U u)
		{
			if (TtoU.TryGetValue(t, out var tu))
			{
				UtoT.Remove(tu);
			}
			if (UtoT.TryGetValue(u, out var ut))
			{
				TtoU.Remove(ut);
			}
			TtoU.Remove(t);
			UtoT.Remove(u);
			TtoU.Add(t, u);
			UtoT.Add(u, t);
		}
		public T this[U u]
		{
			get { return UtoT[u]; }
		}
		public U this[T t]
		{
			get { return TtoU[t]; }
		}
		public bool ContainsKey(T t)
		{
			return (TtoU.ContainsKey(t));
		}
		public bool ContainsKey(U u)
		{
			return UtoT.ContainsKey(u);
		}
		public bool tryGetValue(T t, out U u)
		{
			if (TtoU.TryGetValue(t, out u))
			{
				return true;
			}
			return false;
		}
		public bool tryGetValue(U u, out T t)
		{
			if (UtoT.TryGetValue(u, out t))
			{
				return true;
			}
			return false;
		}
	}
}
