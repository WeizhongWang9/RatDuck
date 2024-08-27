using CardGame.Game.GameEvents;
using CardGame.Game.GameTerms.Abilities;
using CardGame.Game.GameTerms.Units;
using CardGame.GameEvents.UnitEvents;
using CardGame.Lib.EventSystem;
using Godot;
using RatDuck.Script.Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace CardGame.Game.GameTerms
{
	public class CONST_ABILITY_DISS
	{
		public const int RELEASE_WEIGHT = 1000;
		public const int RELEASE_CHAOTIC_INCRE = 1;
		public const int RELEASE_CHAOTIC_SPREAD_INCRE = 1;
		public const int SPEECH_WEIGHT = 1000;
		public const int DONATED_WEIGHT = 1000;
		public const int NEWTURN_WEIGHT = 500;
	}

	public class DissRelease:UnitEvents
	{
		public Body body;
		public DissRelease(Unit unit, Body body) : base(unit)
		{
			this.body = body;
			Global.getEventManager().emitEvent(this);
		}
	}
	public class DissSpeech : UnitEvents
	{
		public GameNode gameNode;
		public DissSpeech(Unit unit,  GameNode gameNode) : base(unit)
			{ this.gameNode = gameNode;
			Global.getEventManager().emitEvent(this);
		}
	}
	public class DissDonated: UnitEvents
	{
		public GameNode gameNode;
		public DissDonated(Unit unit, GameNode gameNode) : base(unit)
		{ this.gameNode = gameNode;
			Global.getEventManager().emitEvent(this);
		}
	}
	public class DissMask : UnitEvents
	{
		public Unit toUnit;
		public DissMask(Unit unit,Unit toUnit) : base(unit)
		{
			this.toUnit = toUnit;
			Global.getEventManager().emitEvent(this);
		}
	}
	public class AbilityDiss : EventSystem
	{
		AbilityTrigDel<DissRelease, AbilityDiss> trigRelease;
		AbilityTrigDel<DissSpeech, AbilityDiss> trigSpeech;
		AbilityTrigDel<DissDonated, AbilityDiss> trigDonated;
		eventDictionary<Data> data = new eventDictionary<Data>();
		eventDictionary<GameNodeData> gameNodeData = new eventDictionary<GameNodeData>();
		AbilityCarry abilityCarry;
		AbilityLoc abilityLoc;
		AbilityChaoticMeasure abilityChaoticMeasure;
		AbilityGameNode abilityGameNode;
		AbilityMoney abilityMoney;
		GameTrigger<Turn> newTurn;
		
		public AbilityDiss(Game game,AbilityCarry abilityCarry, AbilityLoc abilityLoc, AbilityChaoticMeasure abilityChaoticMeasure, AbilityGameNode abilityGameNode,
			AbilityMoney abilityMoney) : base(game) {
			this.abilityCarry = abilityCarry;
			this.abilityLoc = abilityLoc;
			this.abilityMoney = abilityMoney;
			this.abilityChaoticMeasure = abilityChaoticMeasure;
			this.abilityGameNode = abilityGameNode;
			trigRelease = new AbilityTrigDel<DissRelease, AbilityDiss>(
					this, actRelease, CONST_ABILITY_DISS.RELEASE_WEIGHT);
			trigSpeech = new AbilityTrigDel<DissSpeech, AbilityDiss>(
				this, actSpeech, CONST_ABILITY_DISS.SPEECH_WEIGHT);
			trigDonated = new AbilityTrigDel<DissDonated, AbilityDiss>(
				this, actDonated, CONST_ABILITY_DISS.DONATED_WEIGHT);
			newTurn = new(actNewTurn, CONST_ABILITY_DISS.NEWTURN_WEIGHT);
		}

		public void init(Unit unit)
		{
			var dt = new Data();
			data.Add(unit, dt);
		}

		public void init(GameNode gameNode)
		{
			var dt = new GameNodeData();
			gameNodeData.Add(gameNode, dt);
		}
		public bool hasAbility(GameNode unit) { return gameNodeData.ContainsKey(unit); }
		public bool hasAbility(Unit unit) { return data.ContainsKey(unit); }
		public bool tryGetDonated(GameNode unit, out bool value)
		{
			if (gameNodeData.TryGetValue(unit, out var dt))
			{
				value = dt.hasDonated;
				return true;
			}
			value = false;
			return false;
		}
		public bool setDonated(GameNode gameNode, bool value)
		{
			if(gameNodeData.TryGetValue(gameNode, out var dt))
			{ 
				dt.hasDonated = value;
				return true;
			}
			return false;
		}
		public bool tryGetWanted(Unit unit, out int value)
		{
			if (data.TryGetValue(unit, out var dt))
			{
				value = dt.wanted;
				return true;
			}
			value = -1;
			return false;
		}
		public bool isDiss(Unit unit)
		{
			if (unit != null)
				return data.ContainsKey(unit) ;
			return false;
		}
		public bool isReleasiable(Unit unit, Body body, out string txt)
		{
			if(!isDiss(unit))
			{
				txt = "Not a dissendent";
				return false;
			}
			txt = "";
			return true;
		}
		public bool tryRelease(Unit unit, Body body, out string txt)
		{
			if(isReleasiable(unit,body,out txt))
			{
				new DissRelease(unit, body);
				return true;
			}
			return false;
		}
		public void addWanted(Unit unit, int value)
		{
			if(data.TryGetValue(unit, out var dt))
			{
				dt.wanted += value;
			}
		}
		public void setWanted(Unit unit, int value)
		{
			if (data.TryGetValue(unit, out var dt))
			{
				dt.wanted = value;
			}
		}
		void actRelease(DissRelease dissRelease)
		{
			var spreadIncre = CONST_ABILITY_DISS.RELEASE_CHAOTIC_SPREAD_INCRE;
			var normalIncre = CONST_ABILITY_DISS.RELEASE_CHAOTIC_INCRE;
			var maxMeasure = CONST_ABILITY_CHAOTICMEASURE.MAXMEASURE;
			var unit = dissRelease.getTriggerUnit();
			var body = dissRelease.body;
			if(isReleasiable(unit,body,out var txt))
			{
				var gameNode = abilityLoc[unit];
				var measure = abilityChaoticMeasure[gameNode];
				if (measure >= maxMeasure)
				{
					if (abilityGameNode.tryGetNeighbors(gameNode, out var neighbors))
					{
						foreach (var neighbor in neighbors)
						{
							abilityChaoticMeasure.addMeasure(neighbor, spreadIncre);
						}
					}
				}
				else
					abilityChaoticMeasure.addMeasure(gameNode, normalIncre);
				addWanted(unit, 1);
				new UnitLeaveMap(body, abilityLoc[body]);
				new BodyRelease(body);
			}
		}
		void actSpeech(DissSpeech dissSpeech)
		{
			var unit = dissSpeech.getTriggerUnit();
			var loc = dissSpeech.gameNode;
			if(isDiss(unit) && abilityGameNode.isGameNode(loc))
			{
				abilityChaoticMeasure.addMeasure(loc,1);
				addWanted(unit, 1);
			}
		}
		int getDonatedMoney(GameNode gameNode)
		{
			return Math.Min(1,abilityChaoticMeasure[gameNode]);
		}
		void actDonated(DissDonated dissDonated)
		{
			var unit = dissDonated.getTriggerUnit();
			var loc = dissDonated.gameNode;
			if(isDiss(unit) && gameNodeData.TryGetValue(loc,out var dt))
			{
				// todo:NEW round act
				dt.hasDonated = true;
				abilityMoney.addValue(unit, getDonatedMoney(loc));
			}
		}
		void actNewTurn(Turn turn)
		{
			foreach (var gamenode in gameNodeData)
			{
				gamenode.Value.hasDonated = true;
			}
		}
		void actMask(DissMask dissMask)
		{
			var unit = dissMask.getTriggerUnit();
			var toUnit = dissMask.toUnit;
			if(hasAbility(unit) && hasAbility(toUnit))
			{
				addWanted(toUnit, -1);
			}
		}
		public bool isMask(Unit unit, Unit toUnit, out string txt)
		{
			if(!hasAbility(unit))
			{
				txt = "User not a diss";
				return false;
			}
			if(!hasAbility(toUnit))
			{
				txt = "Target not a diss";
				return false;
			}
			txt = "";
			return true;
		}
		public bool tryMask(Unit unit,Unit toUnit,out string txt)
		{
			if(isMask(unit, toUnit,out txt))
			{
				new DissMask(unit, toUnit);
				return true;
			}
			return false;
		}
		public bool tryGetIsBlocked(GameNode gameNode, out bool isBlocked)
		{
			if(gameNodeData.TryGetValue(gameNode,out var dt))
			{
				isBlocked = dt.isBlocked;
				return true;
			}
			isBlocked = false;
			return false;
		}
		public void setIsBlocked(GameNode gameNode, bool isBlocked)
		{
			if (gameNodeData.TryGetValue(gameNode, out var dt))
			{
				dt.isBlocked = isBlocked;
				return;
			}
		}
		public bool canMove(GameNode gameNode)
		{
			return !gameNodeData[gameNode].isBlocked;
		}
		public bool tryMove(Unit unit, GameNode gameNode)
		{
			if(canMove(gameNode))
			{
				new UnitMove(unit, abilityLoc[unit] ,gameNode);
				return true;
			}
			return false;
		}

		bool tryToSpeechCond(Token unit, GameNode gameNode, out string text)
		{
			if (!abilityChaoticMeasure.tryGetValue(gameNode, out var chao))
			{
				text = "Not a chaotic measure.";
				return false;
			}
			if (!(chao < 3))
			{
				text = "Measure is more than or equal to 3";
				return false;
			}
			text = "";
			return true;
		}
		public bool trySpeech(Token unit, GameNode gameNode, out string text)
		{
			if(tryToSpeechCond(unit, gameNode, out text))
			{
				new DissSpeech(unit, gameNode);
				return true;
			}
			return false;
		}


		class Data
		{
			public int _wanted = 0;
			public int wanted
			{
				get { return _wanted; }
				set { if (value >= 0) _wanted = value;else _wanted = 0; }
			}
			public Data()
			{
				wanted = 0;
			}
		}
		class GameNodeData
		{
			public bool hasDonated = false;
			public bool isBlocked = false;
			public GameNodeData() 
			{ 
				hasDonated = false;
				isBlocked = false;
			}
		}

	}
}
