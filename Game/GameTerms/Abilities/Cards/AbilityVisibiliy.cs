using CardGame.Game.GameTerms.PlayerUtility;
using CardGame.Game.GameTerms.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardGame.Game.GameTerms.Abilities.Cards
{
	public class AbilityVisibiliy
	{
		eventDictionary<Data> data = new eventDictionary<Data>();

		public AbilityVisibiliy() { }
		/// <summary>
		/// A card with null team is visible to anyone.
		/// </summary>
		/// <param name="card"></param>
		/// <param name="team"></param>
		public void init(Card card, Team team)
		{
			data.Add(card,new Data(team));
		}
		public bool hasAbility(Card card)
			{ return data.ContainsKey(card); }
		public bool isVisible(Card card, Team team)
		{
			if (hasAbility(card))
			{
				var cardTeam = data[card].team;
				if (cardTeam != null)
					return data[card].team == team;
				return true;
			}
			return false;
		}
		public bool isVisible(Card card, Player player)
		{
			return (isVisible(card, player.team)); 
		}

		class Data
		{
			public Team team;
			public Data(Team team)
			{
				this.team = team;
			}
		}
	}
}
