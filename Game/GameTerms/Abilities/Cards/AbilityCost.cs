using CardGame.Game.GameTerms.PlayerUtility;
using CardGame.Game.GameTerms.Units;
using System;
using CardGame.Game.GameTerms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using CardGame.Lib.EventSystem.Order;
using CardGame.Game.GameTerms.Abilities.Bases;

namespace CardGame.Game.GameTerms.Abilities.Cards
{

	public class AbilityCost
	{
		AbilityMoney abilityMoney;
		AbilityCardSlot slot;
		AbilityPlayer abilityPlayer;
		eventDictionary<Data> data = new eventDictionary<Data>();
		public void init(Unit card, Team team ,int cost = 0)
		{
			data.Add(card, new Data(cost,team));
		}
		public AbilityCost(AbilityMoney abilityMoney, AbilityCardSlot abilityCardSlot, AbilityPlayer abilityPlayer)
		{
			this.abilityMoney = abilityMoney;
			this.abilityPlayer = abilityPlayer;
			this.slot = abilityCardSlot;
		}
		public bool hasAbility(Card card)
		{ return data.ContainsKey(card); }
		public bool canBuy(Unit unit, Card card)
		{
			if(tryGetCost(card,out var cost)&& abilityMoney.tryGetValue(unit, out var money))
			{
				if (money > cost)
					return true;
				return false;
			}
			return false;
		}
		public bool tryGetCost(Card card,out int cost)
		{
			if(data.TryGetValue(card,out var dt))
			{
				cost = dt.cost;
				return true;
			}
			cost = -1;
			return false;
		}
		public bool tryBuy(Unit unit, Card card)
		{
			if (tryGetCost(card, out var cost) && abilityMoney.tryGetValue(unit, out var money))
			{
				if (money > cost)
				{
					abilityMoney.addValue(unit, -cost);
					slot.addCard(unit, card);
					data[unit].owner = abilityPlayer[unit];
					return true;
				}
				return false;
			}
			return false;
		}

		
		class Data
		{
			public int cost;
			public Player owner;
			public Data(int cost, Team team)
			{
				this.cost = cost;
				owner = null;
			}
		}
	}
}
