using CardGame.Game.GameTerms.PlayerUtility;
using CardGame.Game.GameTerms.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardGame.Game.GameTerms.Abilities
{
	public class AbilityCardSlot
	{
		eventDictionary<Data> data = new eventDictionary<Data>();
		public AbilityCardSlot() { }
		public void init(Unit unit)
		{
			data.Add(unit, new Data());
		}
		public bool hasAbility(Units.Card card)
		{ return data.ContainsKey(card); }
		
		public bool tryGetCards(Unit unit, out List<Card> cards)
		{
			if (data.TryGetValue(unit, out var dt))
			{
				cards = dt.cards;
				return true;
			}
			cards = null;
			return false;
		}
		public bool addCard(Unit unit,Card card)
		{
			if(tryGetCards(unit,out var cards))
			{
				cards.Add(card);
				return true;
			}
			return false;
		}
		public bool removeCard(Unit unit, Card card)
		{
			if(tryGetCards(unit, out var cards))
			{
				cards.Remove(card);
				return true;
			}
			return false;
		}

		class Data
		{
			public List<Card> cards;
			public Data()
			{
				cards = new List<Card>();
			}
		}
	}
}
