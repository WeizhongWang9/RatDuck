using CardGame.Game.GameEvents;
using CardGame.Game.GameTerms.Units;
using CardGame.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardGame.Game.GameTerms.Abilities
{
	/// <summary>
	/// Carry moves after the unit move.
	/// </summary>
	public class CONSTANT_ABILITY_CARRY
	{
		public const int WEIGHT = 1 + CONSTANT_ABILITY_MOVE.WEIGHT;
	}
	public class AbilityCarry:EventSystem
	{
		AbilityLoc abilityLoc;
		DelTrigger<UnitMove> trig;
		public AbilityCarry(Game game,AbilityLoc abilityLoc) : base(game) {
			trig = new DelTrigger<UnitMove>(carryMove, CONSTANT_ABILITY_CARRY.WEIGHT);
			this.abilityLoc = abilityLoc;
		}

		eventDictionary<bool> containsSkill = new eventDictionary<bool>();
		eventBidictionary<Body> data = new eventBidictionary<Body>();

		public void init(Unit unit)
		{
			containsSkill.Add(unit, true);
		}
		public void init(Body body)
		{
			containsSkill.Add(body, true);
		}

		public void link(Unit unit, Body body)
		{
			data.add(unit,body);
		}

		public bool isCarrying(Unit unit) { return containsSkill.ContainsKey(unit); }
		public bool isCarried(Body body) { return containsSkill.ContainsKey(body); }
		public bool tryGetValue(Unit unit, out Body body)
		{
			if (data.tryGetValue(unit, out body))
				return true;
			return false;
		}
		public bool tryGetValue(Body body, out Unit unit)
		{
			if (data.tryGetValue(body, out unit))
				return true;
			return false;
		}

		public void remove(Unit unit)
		{
			data.remove(unit);
		}
		public void remove(Body body)
		{
			data.remove(body);
		}

		public void carryMove(UnitMove move)
		{
			var unit = move.getTriggerUnit() as Token;
			var toLoc = move.toWhere;
			if (tryGetValue(unit, out var body))
			{
				new UnitEnter(body, abilityLoc[body], toLoc);
			}
		}

	}
}
