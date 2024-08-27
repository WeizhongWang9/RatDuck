using CardGame.Game.GameEvents;
using CardGame.Game.GameTerms.Abilities;
using CardGame.GameEvents.UnitEvents;
using CardGame.Lib.EventSystem;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardGame.Game.GameTerms
{

	public abstract class UnitUnhandleEvents
	{
		public Unit unit;
		public UnitUnhandleEvents(Unit unit)
		{
			this.unit = unit;
		}
	}
	public class UnitCreated : UnitUnhandleEvents
	{
		public UnitCreated(Unit unit) : base(unit) { 
			Global.getEventManager().emitEvent(this); 
		}
	}
	public class UnitRemoved : UnitUnhandleEvents
	{
		public UnitRemoved(Unit unit) : base(unit) { 
			Global.getEventManager().emitEvent(this); 
		}
	}
	public class UnitHandle : HandleManager<Unit>
	{
		Game game;
		public UnitHandle(Game game) : base()
		{
			this.game = game;
			GameTrigger<UnitCreated> trigOnCreateUnit = new GameTrigger<UnitCreated>(onCreateUnit, -1);
			GameTrigger<UnitRemoved> trigOnRemoveUnit = new GameTrigger<UnitRemoved>(onRemoveUnit, -1);
		}
		void onCreateUnit(UnitCreated e)
		{
			var u = e.unit;
			u.handle = game.unitHandle.assignObjectID(u);
		}

		void onRemoveUnit(UnitRemoved e)
		{
			var u = e.unit;
			game.unitHandle.removeObject(u);
		}

	}
}
