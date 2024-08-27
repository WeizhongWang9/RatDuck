using CardGame.Game.GameEvents;
using CardGame.Game.GameTerms.Units;
using CardGame.GameEvents.UnitEvents;
using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CardGame.Game.GameTerms.Abilities
{

	public class UnitEnter : UnitEvents
	{
		public GameNode toWhere;
		public GameNode fromWhere;
		public UnitEnter(Unit unit, GameNode fromWhere,GameNode toWhere) : base(unit)
		{
			this.fromWhere = fromWhere;
			this.toWhere = toWhere;
			Global.getEventManager().emitEvent(this);
		}
	}

	public class UnitLeaveMap : UnitEvents
	{
		public GameNode fromWhere;
		public UnitLeaveMap(Unit unit,GameNode fromWhere) : base(unit)
		{
			this.fromWhere = fromWhere;
			Global.getEventManager().emitEvent(this);
		}
	}
	public class AbilityGameNode : EventSystem
	{
		Trig trig;
		AbilityLoc abilityLoc;
		eventDictionary<GameNodeData> gameNodeDatas = new eventDictionary<GameNodeData>();
		class GameNodeData
		{
			public List<GameNode> neighbors = new List<GameNode>();
			public LinkedList<Unit> units = new LinkedList<Unit>();
		}
		
		public AbilityGameNode(Game game, AbilityLoc abilityLoc) : base(game)
		{
			this.abilityLoc = abilityLoc;
			trig = new Trig(this);
		}

		public void initGameNode(GameNode gameNode)
		{
			gameNodeDatas.Add(gameNode, new GameNodeData());
		}
		public bool isGameNode(GameNode gameNode)
		{
			if (gameNode != null)
				return gameNodeDatas.ContainsKey(gameNode);
			return false;
		}

		public bool tryGetNeighbors(GameNode node, out List<GameNode> neighbors)
		{
			if(gameNodeDatas.TryGetValue(node,out var data))
			{
				neighbors = data.neighbors;
				return true;
			}
			neighbors = null;
			return false;
		}
		public bool tryGetUnits(GameNode node, out LinkedList<Unit> units)
		{
			if(gameNodeDatas.TryGetValue(node, out var data))
			{
				units = data.units;
				return true;
			}
			units = null;
			return false;
		}

		public bool addNeighbor(GameNode nodeA, GameNode nodeB)
		{
			if(tryGetNeighbors(nodeA,out var neighbors))
			{
				if(!neighbors.Contains(nodeB))
				{
					neighbors.Add(nodeB);
					if(tryGetNeighbors(nodeB,out var Bneighbors))
					{ 
						Bneighbors.Add(nodeA);
					}
					return true;
				}
				return false;
			}
			return false;
		}

		public void unitEnter(Unit unit, GameNode towhere)
		{
			new UnitEnter(unit, abilityLoc[unit],towhere);
		}
		public void unitEnterByCreating(Unit unit, GameNode towhere)
		{
			new UnitEnter(unit, null, towhere);
		}

		void addUnit(GameNode gameNode, Unit unit)
		{
			if (unit == null || gameNode == null) { return; }
			if (tryGetUnits(gameNode, out var units))
			{
				units.AddLast(unit);
				abilityLoc.changeLocation(unit,gameNode);
			}
		}
		void removeUnit(GameNode gameNode, Unit unit)
		{
			if (unit == null || gameNode == null) { return; }
			if (tryGetUnits(gameNode, out var units))
			{
				units.Remove(unit);
			}
		}

		public bool isNeighbor(GameNode unit, GameNode unitChecked)
		{
			if (gameNodeDatas.TryGetValue(unit, out var data))
			{
				return data.neighbors.Contains(unitChecked);
			}
			return false;
		}
		class Trig
		{
			GameTrigger<UnitEnter> trigger;
			GameTrigger<UnitLeaveMap> triggerunitLeave;
			AbilityGameNode abilityGameNode;

			public Trig(AbilityGameNode abilityGameNode)
			{
				trigger = new GameTrigger<UnitEnter>(unitEnterGameNode,1000);
				triggerunitLeave = new GameTrigger<UnitLeaveMap> (unitLeaveGameNode,0);
				this.abilityGameNode = abilityGameNode;
			}

			void unitLeaveGameNode(UnitLeaveMap unitLeaveMap)
			{
				var unit = unitLeaveMap.getTriggerUnit();
				var gameNode = unitLeaveMap.fromWhere;
				abilityGameNode.removeUnit(gameNode, unit);
			}

			void unitEnterGameNode(UnitEnter unitEnter)
			{
				var unit = unitEnter.getTriggerUnit();
				var fromWhere = unitEnter.fromWhere;
				var toWhere = unitEnter.toWhere;
				abilityGameNode.addUnit(toWhere, unit);
				abilityGameNode.removeUnit(fromWhere, unit);
			}
		}
	}
}
