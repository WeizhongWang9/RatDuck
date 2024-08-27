using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using static Godot.OpenXRInterface;

namespace CardGame.Lib.Deck
{
	public class Shuffler
	{
		public Shuffler()
		{
			_rng = new Random();
		}

		/// <summary>Shuffles the specified array.</summary>
		/// <typeparam name="T">The type of the array elements.</typeparam>
		/// <param name="array">The array to shuffle.</param>

		public void Shuffle<T>(IList<T> array)
		{
			for (int n = array.Count; n > 1;)
			{
				int k = _rng.Next(n);
				--n;
				T temp = array[n];
				array[n] = array[k];
				array[k] = temp;
			}
		}

		private System.Random _rng;
	}

	public class Deck<T> : List<T>
	{
		public void AddFirst(T item)
		{
			this.Add(item);
			var i = this.Count;
			while (i > 1)
			{
				var index = i - 2;
				var curItem = this[index];
				this[i-1] = curItem;
				i--;
			}
			this[0] = item;
		}
		public bool moveTo(Deck<T> deck, T card, bool isLast)
		{
			if (Remove(card))
			{
				if (isLast) { deck.Add(card); } else { deck.AddFirst(card); }
				return true;
			}
			else return false;
		}
		public T pass(bool isFromFirst)
		{
			T card;
			if (isFromFirst) { card = this.First(); } else { card = this.Last(); }
			Remove(card);
			return card;
		}
		public List<T> pass(bool isFromFirst, int amount)
		{
			amount = Math.Min(amount, this.Count);
			var passDeck = new List<T>();
			if (amount > 0)
			{
				for (int i = 0; i < amount; i++)
				{
					var card = pass(isFromFirst);
					passDeck.Add(card);
				}
			}
			passDeck.Reverse();
			return passDeck;
		}
		public List<T> pass(bool isFromFirst, Deck<T> deck, int amount, bool isToFirst)
		{
			amount = Math.Min(amount, this.Count);
			var passDeck = new List<T>();
			if (amount > 0)
			{
				for (int i = 0; i < amount; i++)
				{
					var card = pass(isFromFirst);
					if (!isToFirst) { deck.Add(card); } else { deck.AddFirst(card); }
					passDeck.Add(card);
				}
			}
			passDeck.Reverse();
			return passDeck;
		}
	}

	public class DeckWithShowcase<T>
	{
		public Deck<T> deck;
		public Deck<T> showcase;
		public Deck<T> waste;
		public int showcaseCap = -1;
		public DeckWithShowcase(Deck<T> deck, Deck<T> showcase, Deck<T> waste, int showcaseCap)
		{
			this.deck = deck;
			this.showcase = showcase;
			this.waste = waste;
			this.showcaseCap = showcaseCap;
		}
		public DeckWithShowcase(Deck<T> deck, int showcaseCap) : this(deck, new Deck<T>(), new Deck<T>(), showcaseCap) { }
		public DeckWithShowcase(int showcaseCap) : this(new Deck<T>(), showcaseCap) { }
		public DeckWithShowcase() : this(-1) { }
		public Deck<T> drawToShowcase(bool isFromDeckFirst, bool isToShowcaseFirst, bool isFromShowcaseFirst = true, bool isToWasteFirst= true)
		{
			deck.pass(isFromDeckFirst, showcase, 1, isToShowcaseFirst);
			if (showcase.Count > showcaseCap && showcaseCap != -1)
			{
				showcase.pass(isFromShowcaseFirst, waste, 1, isToWasteFirst);
			}
			return showcase;
		}


		public void emptyShowcase(bool isFromShowcaseFirst, bool isToWasteFirst)
		{
			showcase.pass(isFromShowcaseFirst, waste, showcase.Count, isToWasteFirst);
		}
		public Deck<T> refillDeckWithWaste()
		{
			var shuffler = new Shuffler();
			shuffler.Shuffle<T>(waste);
			deck = waste;
			waste = new Deck<T>();
			return deck;
		}
		public void discard(T card)
		{
			showcase.Remove(card);
			waste.Add(card);
		}
	}
}
