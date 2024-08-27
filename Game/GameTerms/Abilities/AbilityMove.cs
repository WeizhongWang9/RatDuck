using CardGame.Game.GameEvents;
using CardGame.Game.GameTerms.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CardGame.Game.GameTerms.Units;
using CardGame.GameEvents.UnitEvents;
namespace CardGame.Game.GameTerms.Abilities
{
	public class CONSTANT_ABILITY_MOVE
	{
	   public const int WEIGHT = 1000;
	}
	public class UnitMove:UnitEvents
	{
		public GameNode fromWhere;
		public GameNode toWhere;
		public UnitMove(Unit whichUnit, GameNode fromWhere, GameNode toWhere) : base(whichUnit)
		{
			this.fromWhere = fromWhere;
			this.toWhere = toWhere;
			Global.getEventManager().emitEvent(this);
		}
	}
	public class AbilityMove : EventSystem
	{
		class Data
		{
			public Attribute attribute;
		}
		Trig trig;
		AbilityLoc abilityLoc;
		eventDictionary<Data> datas = new eventDictionary<Data>();
		Game game;

		public AbilityMove(Game game, AbilityLoc abilityLoc) : base(game)
		{
			this.game = game;
			trig = new Trig(this);
			this.abilityLoc = abilityLoc;
		}

		class Trig
		{
			public GameTrigger<UnitMove> trig;
			public GameTrigger<Turn> newTurnTrig;
			public AbilityMove abilityMove;
			public Trig(AbilityMove abilityMove)
			{
				trig = new GameTrigger<UnitMove>(move, CONSTANT_ABILITY_MOVE.WEIGHT);
				newTurnTrig = new GameTrigger<Turn>(onNewTurn, CONSTANT_ABILITY_MOVE.WEIGHT);

				this.abilityMove = abilityMove;
			}

			void onNewTurn(Turn turn)
			{
				var players = turn.game.playerHandle.allPlayer;
				foreach(var player in players.getPlayers())
				{
					abilityMove.recoverMovePoint(player.controlUnit);
				}
			}

			void move(UnitMove unitMove)
			{
				var unit = unitMove.getTriggerUnit();
				var toloc = unitMove.toWhere;
				var fromloc = unitMove.fromWhere;
				if (abilityMove.tryGetAttribute(unit,out var attribute))
				{
					if (attribute.movePoint > 0)
					{
						attribute.movePoint--;
						new UnitEnter(unit, fromloc, toloc);
					}
				}
			}
		}
		public void init(Unit unit, Attribute attribute)
		{
			var data = new Data();
			data.attribute = attribute;
			datas.Add(unit,data);
		}
		public bool tryGetAttribute(Unit unit,out Attribute attribute)
		{
			if (datas.TryGetValue(unit, out var dt))
			{
				attribute = dt.attribute;
				return true;
			}
			attribute = null;
			return false;
		}
		public Attribute this[Unit unit]
		{
			get { if (tryGetAttribute(unit, out var attribute)) return attribute; return null; }
			set { datas[unit].attribute = value; }
		}

		public bool isMovable(Unit unit, GameNode gameNode, out string errorTxt )
		{
			var gameNodeAbility = game.abilityHandle.abilityGameNode;
			var loc = abilityLoc[unit];
			if (!tryGetAttribute(unit, out var attribute))
			{
				errorTxt = "Unit Ability Move uninitialized";
				return false;
			}
			if (!(attribute.movePoint > 0))
			{
				errorTxt = "movePoint is zero";
				return false;
			}
			if(!gameNodeAbility.isNeighbor(gameNode, loc))
			{
				errorTxt = "This is not a neighbor.";
				return false;
			}

			errorTxt = "";
			return true;
		}

		public bool recoverMovePoint(Token unit)
		{
			if (datas.TryGetValue(unit, out var data))
			{
				var attribute = data.attribute;
				attribute.movePoint = attribute.movePointRecover;
				return true;
			}
			return false;
		}

		public bool tryMove(Token token, GameNode toWhere, out string errorTxt)
		{
			if(isMovable(token, toWhere, out errorTxt))
			{
				move(token, toWhere);
				return true;
			}
			return false;
		}

		bool move(Token unit, GameNode toWhere )
		{
			if (tryGetAttribute(unit, out var attribute))
			{
				new UnitMove(unit, abilityLoc[unit], toWhere);
				return true;
			}
			return false;
		}

		public class Attribute
		{
			public int movePoint;
			public int movePointRecover;
			public Attribute(int movePoint = 0, int movePointRecover = 0)
			{
				this.movePoint = movePoint;
				this.movePointRecover = movePointRecover;
			}
		}



	}
}
