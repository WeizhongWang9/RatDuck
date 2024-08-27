using System;
using System.Collections.Generic;
using CardGame.Game.GameTerms;
using CardGame.Game.GameEvents;
using CardGame.Game;
using CardGame.Game.GameTerms.Abilities;

namespace CardGame.GameEvents.UnitEvents
{
	public abstract class UnitEvents: Event
	{ 
		protected ulong objID;
		protected UnitEvents(ulong objID)
		{
			this.objID = objID;
			Global.getEventManager().emitEvent(this);
		}
		public UnitEvents(Unit objID) : this(objID.getHandle()) { }
		public Unit getTriggerUnit() => UnitHandles.getObject(objID);
	}



	/// <summary>
	/// A target selector quotes a list of units based on some rules.
	/// </summary>


	/*
	A unitOrder event takes a list of targets and an IOrder.



		*/



	/*
	 A skill has the function of :
	1. send a 
	 */
	/*
	public class UnitOrder : UnitEvents
	{
		public List<OrderData> data;
		public string filterError = null;
		public string activeError = null;
		int curProcessingIndex;

		public int getCurProcessingIndex()=> curProcessingIndex;
		public void setCurProcessingIndex(int curProcessingIndex) => this.curProcessingIndex = curProcessingIndex;
		public UnitOrder(Unit triggerUnit, List<OrderData> data) :base(triggerUnit)
		{
			this.data = data;
			curProcessingIndex = -1;
		}
	}*/
	/*
	public class UnitEvents
	{
		public static GenericEvent<InfoUnitMove> UnitMove = new GenericEvent<InfoUnitMove>();
		public static GenericEvent<InfoObjectOn<Unit, int>> UnitGetMoney = new GenericEvent<InfoObjectOn<Unit, int>>();
		public static GenericEvent<InfoObjectEvent<Unit>> BeingTargeted = new GenericEvent<InfoObjectEvent<Unit>>();

	}
	public class DissEvents
	{
		public static GenericEvent<InfoObjectOn<Dissident, Place>> SpreadUnrest = new GenericEvent<InfoObjectOn<Dissident, Place>>();
		//public static GenericEvent<InfoObjectEvent<Dissident>> DissAddToken = new GenericEvent<InfoObjectEvent<Dissident>>();
		public static GenericEvent<InfoObjectOn<Dissident, int>> IncreaseWanted = new GenericEvent<InfoObjectOn<Dissident, int>>();
	}

	public class GovEvents
	{// undone
		public static GenericEvent<InfoArrest> Arrest = new GenericEvent<InfoArrest>();
		public static GenericEvent<InfoObjectOn<Police, Place>> PlaceRest = new GenericEvent<InfoObjectOn<Police, Place>>();
		public static GenericEvent<InfoObjectOn<Gov, Body>> GovPickupBody = new GenericEvent<InfoObjectOn<Gov, Body>>();
		public static GenericEvent<InfoObjectEvent<Scientist>> ObtainBodyPlaceCard = new GenericEvent<InfoObjectEvent<Scientist>>();
		public static GenericEvent<InfoKidnap> Kidnap = new GenericEvent<InfoKidnap>();
  
	}

	public class GameNodeEvents
	{
		public static GenericEvent<InfoUnrestIncrease> IncreasePlaceUnrest = new GenericEvent<InfoUnrestIncrease>();

	}

	public class CardEvents
	{
		/// <summary>
		/// Player plays a card.
		/// </summary>
		public static GenericEvent<InfoCardPlay> CardPlay = new GenericEvent<InfoCardPlay>();
		public static GenericEvent<InfoCardPlayOnTarget<Player>> CardPlayOnTargetPlayer = new GenericEvent<InfoCardPlayOnTarget<Player>>();
		public static GenericEvent<InfoCardPlayOnTarget<Unit>> CardPlayOnTargetUnit = new GenericEvent<InfoCardPlayOnTarget<Unit>>();
		public static GenericEvent<InfoCardPlayOnTarget<GameNode>> CardPlayOnMap = new GenericEvent<InfoCardPlayOnTarget<GameNode>>();
		public static GenericEvent<InfoObjectOn<Player,Deck<Card>>> ObtainCardFromDecks = new GenericEvent<InfoObjectOn<Player, Deck<Card>>>();
	}

	public class PlayerEvents
	{
		public static GenericEvent<InfoPlayerBuy> BuyCard = new GenericEvent<InfoPlayerBuy>();
		public static GenericEvent<InfoObjectOn<Player, Card>> PlayerObtainCard = new GenericEvent<InfoObjectOn<Player, Card>>();
	}

	*/
}
