using CardGame.Game.GameEvents;
using CardGame.Game.GameTerms.Units;
using CardGame.GameEvents.UnitEvents;
using Godot;
using RatDuck.Script.Game.GameTerms.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardGame.Game.GameTerms.Abilities
{
	public class PoliceArrest : UnitEvents
	{
		public Unit arrested;
		public GameNode prison;
		public PoliceArrest(Unit police, Unit arrested, GameNode prison) : base(police)
		{
			this.arrested = arrested;
			this.prison = prison;
			Global.getEventManager().emitEvent(this);
		}
	}
	public class PoliceControl : UnitEvents
	{
		public GameNode loc;
		public PoliceControl(Token police, GameNode loc) : base(police)
		{ this.loc = loc;
			Global.getEventManager().emitEvent(this);
		}
	}
	public class CONSTANT_ABILITY_POLICE
	{
		public const int ARREST_THRESHOLD = 2;
		public const int ARREST_WEIGHT = 1000;
		public const int CONTROL_WEIGHT = 1000;
	}
	public class AbilityPolice : EventSystem
	{
		AbilityMove abilityMove;
		AbilityDiss abilityDiss;
		AbilityMoney abilityMoney;
		AbilityChaoticMeasure abilityChaoticMeasure;
		AbilityCarry abilityCarry;
		Map gameMap;
		eventDictionary<Data> data = new eventDictionary<Data>();
		TrigArrest trigArrest;
		TrigControl trigControl;

		public AbilityPolice(Game game,AbilityMove abilityMove, 
			AbilityDiss abilityDiss, 
			AbilityMoney abilityMoney, 
			AbilityChaoticMeasure abilityChaoticMeasure, 
			AbilityCarry abilityCarry,
			AbilityLoc abilityLoc,
			AbilityGameNode abilityGameNode
			) : base(game)
		{
			this.abilityMove = abilityMove;
			this.abilityDiss = abilityDiss;
			this.abilityMoney = abilityMoney;
			this.abilityChaoticMeasure = abilityChaoticMeasure;
			this.abilityCarry = abilityCarry;
			this.gameMap = game.map;
			trigArrest = new TrigArrest(game, this,abilityMove,abilityDiss,abilityLoc,abilityMoney, abilityGameNode);
			trigControl = new TrigControl(game,this, abilityChaoticMeasure);
		}

		public void init(Unit unit)
		{
			data.Add(unit, new Data());
		}

		public bool hasAbility(Unit unit) { return data.ContainsKey(unit); }

		public void link(Unit unit, Body body)
		{
			abilityCarry.link(unit, body);
		}
		public void unlink(Unit unit)
		{
			abilityCarry.remove(unit);
		}

		class Data
		{
		}

		void earn(Token unit, GameNode gameNode)
		{
			if(abilityChaoticMeasure.tryGetValue(gameNode,out var chao))
			{
				abilityMoney.addValue(unit, chao);
			}
		}

		bool tryToControlCond(Token unit, GameNode gameNode, out string text)
		{
			if (!abilityChaoticMeasure.tryGetValue(gameNode, out var chao))
			{
				text = "Not a chaotic measure.";
				return false;
			}
			if (!(chao > 0))
			{
				text = "Measure is 0";
				return false;
			}
			text = "";
			return true;
		}
		public bool tryToControl(Token Unit,  GameNode gameNode,out string text)
		{
			if (tryToControlCond(Unit, gameNode, out text))
			{
				new PoliceControl(Unit, gameNode);
				return true;
			}
			return false;
		}
		public bool tryToMove(Token unit, GameNode toWhere, out string text)
		{
			return abilityMove.tryMove(unit, toWhere, out text);
		}

		public bool tryArrest(Unit police, Unit arrested, out string text)
		{
			if (!this.hasAbility(police))
			{
				text = "The unit is not a police";
				return false;
			}
			if (abilityDiss.tryGetWanted(arrested, out var wanted))
			{
				if (wanted >= CONSTANT_ABILITY_POLICE.ARREST_THRESHOLD)
				{
					new PoliceArrest(police, arrested, gameMap.prison);
					text = "";
					return true;
				}
				text = "The wanted is lower than 2";
				return false;
			}
			text = "The unit is not a diss";
			return false;
		}

		
		class TrigArrest : AbilityTrig<PoliceArrest,AbilityPolice>
		{
			AbilityMove abilityMove;
			AbilityDiss abilityDiss;
			AbilityLoc abilityLoc;
			AbilityMoney abilityMoney;
			AbilityGameNode abilityGameNode;

			public TrigArrest(Game game,AbilityPolice ability,
				AbilityMove abilityMove,AbilityDiss abilityDiss, 
				AbilityLoc abilityLoc,AbilityMoney abilityMoney,AbilityGameNode abilityGameNode) : base(ability, CONSTANT_ABILITY_POLICE.ARREST_WEIGHT)
			{
				this.abilityMove = abilityMove;
				this.abilityLoc = abilityLoc;
				this.abilityMoney = abilityMoney;
				this.abilityDiss = abilityDiss;
				this.abilityGameNode = abilityGameNode;

			}

			protected override void act(PoliceArrest aevent)
			{
				var police = aevent.getTriggerUnit();
				var arrested = aevent.arrested;
				var loc = aevent.prison;
				var threshold = CONSTANT_ABILITY_POLICE.ARREST_THRESHOLD;
				if (abilityDiss.tryGetWanted(arrested, out var wanted))
				{
					if(wanted >= threshold)
					{
						abilityDiss.addWanted(arrested, -threshold);
						new UnitEnter(arrested, abilityLoc[arrested], loc);
					}
				}
			}
		}
		class TrigControl : AbilityTrig<PoliceControl, AbilityPolice>
		{
			AbilityChaoticMeasure abilityChaoticMeasure;

			public TrigControl(Game game, AbilityPolice ability,AbilityChaoticMeasure abilityChaoticMeasure) : base(ability, CONSTANT_ABILITY_POLICE.CONTROL_WEIGHT)
			{
				this.abilityChaoticMeasure = abilityChaoticMeasure;
			}

			protected override void act(PoliceControl aevent)
			{
				var police = aevent.getTriggerUnit() as Token;
				var loc = aevent.loc;
				if(ability.tryToControlCond(police, loc, out var text))
				{
					ability.earn(police, loc);
					abilityChaoticMeasure.addMeasure(loc, -1);
				}
			}
		}

	}
}
