/*using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection.Emit;
using System.Runtime.Remoting.Messaging;
using CardGame.Lib.EventSystem;
using CardGame.GameEvents.UnitEvents;
using CardGame.Game.GameTerms;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Windows.Forms;
using CardGame.Lib.Deck;
*/
/*
namespace CardGame.GameEvents.BasicEffects
{
	public class BasicEffects
	{
		static int GENERAL_PRIORITY = 1000;
		
		public static Move move = new Move(GENERAL_PRIORITY);
		public static BuyCard buyCard = new BuyCard(GENERAL_PRIORITY);
		public static CardPlayOn<Player> cardPlayOnPlayer = new CardPlayOn<Player>(GENERAL_PRIORITY,CardEvents.CardPlayOnTargetPlayer);
		public static CardPlayOn<Unit> cardPlayOnUnit = new CardPlayOn<Unit>(GENERAL_PRIORITY,CardEvents.CardPlayOnTargetUnit);
		public static CardPlayOn<GameNode> cardPlayOnMap = new CardPlayOn<GameNode>(GENERAL_PRIORITY,CardEvents.CardPlayOnMap);
		public static AddMoney addMoney = new AddMoney(UnitEvents.UnitGetMoney, GENERAL_PRIORITY);
		public static IncreasePlaceUnrest increasePlaceUnrest = new IncreasePlaceUnrest(GameNodeEvents.IncreasePlaceUnrest, GENERAL_PRIORITY);
		public static IncreaseDissWanted increaseDissWanted = new IncreaseDissWanted(DissEvents.IncreaseWanted, GENERAL_PRIORITY);
		public static ObatinCard obatinCard = new ObatinCard(PlayerEvents.PlayerObtainCard, GENERAL_PRIORITY);
	}
	public class DissEffect
	{
		static int GENERAL_PRIORITY = 1000;
		public static DissSpreadUnrest dissSpreadUnrest = new DissSpreadUnrest(DissEvents.SpreadUnrest, GENERAL_PRIORITY);
	}

	public class GovEffect
	{
		static int GENERAL_PRIORITY = 1000;
		public static Arrest arrest = new Arrest(GovEvents.Arrest, GENERAL_PRIORITY);
		public static PlaceRest placeRest = new PlaceRest(GovEvents.PlaceRest, GENERAL_PRIORITY);
		public static PickupBody pickupBody = new PickupBody(GovEvents.GovPickupBody, GENERAL_PRIORITY);
		public static ObtainBodyPlaceCard obtainBodyPlaceCard = new ObtainBodyPlaceCard(GovEvents.ObtainBodyPlaceCard, GENERAL_PRIORITY);
		public static Kidnap kidnap = new Kidnap(GovEvents.Kidnap, GENERAL_PRIORITY);
	}

	public class UnitBeingTargeted : EventEffectOn<InfoObjectEvent<Unit>>
	{
		public UnitBeingTargeted(GenericEvent<InfoObjectEvent<Unit>> Event, int priority) : base(Event, null, priority)
		{
		}
	}

	public class Move : EventEffectOn<InfoUnitMove>
	{
		protected static void effect(InfoUnitMove info)
		{
			info.Obj.loc = info.loc;
		}
		public Move(int priority) : base(UnitEvents.UnitMove, effect, priority) { }

	}

	public class CardPlayOn<T> : EventEffectOn<InfoCardPlayOnTarget<T>>
	{
		protected static void effect(InfoCardPlayOnTarget<T> info)
		{
			var Data = info;
			var card = Data.card;
			var player = Data.player;
			var cardEffect = Data.cardEffect;
			var target = Data.target;
			cardEffect.use(target);
		}
		public CardPlayOn(int priority,GenericEvent<InfoCardPlayOnTarget<T>> Event) : base(Event, effect, priority)
		{
		}
	}
	public abstract class ObjectOnEffect<T,U> : EventEffectOn<InfoObjectOn<T,U>>
	{
		protected ObjectOnEffect(GenericEvent<InfoObjectOn<T, U>> Event, Action<InfoObjectOn<T, U>> effect, int priority) : base(Event, effect, priority)
		{
		}
	}
	public abstract class  ObjectEventEffect<T> : EventEffectOn<InfoObjectEvent<T>>
	{
		protected ObjectEventEffect(GenericEvent<InfoObjectEvent<T>> Event, Action<InfoObjectEvent<T>> effect, int priority) : base(Event, effect, priority)
		{
		}
	}
	public class AddMoney : ObjectOnEffect<Unit, int>
	{
		protected static void effect(InfoObjectOn<Unit, int> info)
		{
			var Data = info;
			Data.Obj.Owner.money += Data.onObject;
		}
		public AddMoney(GenericEvent<InfoObjectOn<Unit, int>> Event, int priority) : base(Event, effect, priority)
		{

		}
	}
	public class IncreasePlaceUnrest : EventEffectOn<InfoUnrestIncrease>
	{
		protected static void effect(InfoUnrestIncrease info)
		{
			var place = info.onObject;
			var unit = info.Obj;
			place.unrest += info.unrestIncrease;

		}
		public IncreasePlaceUnrest(GenericEvent<InfoUnrestIncrease> Event, int priority) : base(Event, effect, priority)
		{
		}
	}
	public class IncreaseDissWanted : ObjectOnEffect<Dissident,int>
	{
		public IncreaseDissWanted(GenericEvent<InfoObjectOn<Dissident, int>> Event, int priority) : base(Event, effect, priority)
		{
		}

		protected static void effect(InfoObjectOn<Dissident,int> info)
		{
			var value = info.onObject;
			var unit = info.Obj;
			unit.wanted += value;
		}
	}
	public class DissSpreadUnrest : ObjectOnEffect<Dissident, Place>
	{
		public DissSpreadUnrest(GenericEvent<InfoObjectOn<Dissident, Place>> Event, int priority) : base(Event, effect, priority)
		{
		}

		protected static void effect(InfoObjectOn<Dissident,Place> info)
		{
			var unit = info.Obj;
			var place = info.onObject;
			var increaseValue = unit.unrestSpreadSkill;
			GameNodeEvents.IncreasePlaceUnrest.call(new InfoUnrestIncrease(unit, place, increaseValue));
		}
	}
	public class Arrest : EventEffectOn<InfoArrest>
	{
		public Arrest(GenericEvent<InfoArrest> Event, int priority) : base(Event, effect, priority)
		{
		}

		protected static void effect(InfoArrest info)
		{
			var cop = info.Obj;
			var diss = info.onObject;
			var place = info.prison;
			var increaseValue = -2;
			DissEvents.IncreaseWanted.call(new InfoObjectOn<Dissident, int>(diss, increaseValue));
			UnitEvents.UnitMove.call(new InfoUnitMove(diss, place));
		}
	}
	public class PlaceRest : ObjectOnEffect<Police, Place>
	{
		public PlaceRest(GenericEvent<InfoObjectOn<Police, Place>> Event, int priority) : base(Event, effect, priority)
		{
		}
		protected static void effect(InfoObjectOn<Police,Place> info)
		{
			var cop = info.Obj;
			var place = info.onObject;
			var unrestLevel = place.unrest;
			var newLevel = unrestLevel - 1;

			UnitEvents.UnitGetMoney.call(new InfoObjectOn<Unit, int>(cop, unrestLevel));
			GameNodeEvents.IncreasePlaceUnrest.call(new InfoUnrestIncrease(cop, place, newLevel));
		}
	}
	public class PickupBody : ObjectOnEffect<Gov, Body>
	{
		protected static void effect(InfoObjectOn<Gov,Body> info)
		{
			var unit = info.Obj;
			var body = info.onObject;
			unit.body = body;
			body.Owner = unit.Owner;
		}
		public PickupBody(GenericEvent<InfoObjectOn<Gov, Body>> Event, int priority) : base(Event, effect, priority)
		{

		}
	}
	public class ObatinCard : ObjectOnEffect<Player, Card>
	{
		public ObatinCard(GenericEvent<InfoObjectOn<Player, Card>> Event, int priority) : base(Event, effect, priority)
		{
		}
		protected static void effect(InfoObjectOn<Player,Card> info)
		{
			var Data = info;
			var player = Data.Obj;
			var card = Data.onObject;
			var cardSlot = player.getCardSlot();
			cardSlot.addCard(card);
		}
	}
	public class ObtainCardFromDeck<T> where T : Card
	{
		public static void blueprintEffect(InfoObjectOn<Player, Deck<T>> info)
		{
			var player = info.Obj;
			var deck = info.onObject;
			var cardSlot = player.getCardSlot();
			var card = deck.pass(true);
			PlayerEvents.PlayerObtainCard.call(new InfoObjectOn<Player, Card>(player, card));
		}
	}
	public class BuyCard : EventEffectOn<InfoPlayerBuy>
	{
		protected static void effect(InfoPlayerBuy info)
		{
			var player = info.player;
			var card = info.card;
			player.money -= card.cost;
			PlayerEvents.PlayerObtainCard.call(new InfoObjectOn<Player, Card>(player, card));
		}
		public BuyCard(int priority) : base(PlayerEvents.BuyCard, effect, priority) { }
	}
	public class ObtainBodyPlaceCard : ObjectEventEffect<Scientist>
	{
		public ObtainBodyPlaceCard(GenericEvent<InfoObjectEvent<Scientist>> Event, int priority) : base(Event, effect, priority)
		{
		}

		protected static void effect(InfoObjectEvent<Scientist> info)
		{
			var unit = info.Obj;
			var player = unit.Owner;
			ObtainCardFromDeck<BodyPlaceCard>.blueprintEffect(new InfoObjectOn<Player, Deck<BodyPlaceCard>>(player, Decks.bodyPlaceCardDeck.deck));
		}
	}
	public class Kidnap : EventEffectOn<InfoKidnap>
	{
		public Kidnap(GenericEvent<InfoKidnap> Event, int priority) : base(Event, effect, priority)
		{
		}
		protected static void effect(InfoKidnap info)
		{
			var unit = info.Obj;
			var loc = info.onObject;
			var card = info.bodycard;
			loc.addToken(new Body(loc));
			var bodyplace = card.place;
			if (bodyplace.tensionLevel < 3)
			{
				GameNodeEvents.IncreasePlaceUnrest.call(new InfoUnrestIncrease(unit,bodyplace,1));
			}
			else
			{
				foreach(Place neighbor in bodyplace.Neighbors)
				{
					GameNodeEvents.IncreasePlaceUnrest.call(new InfoUnrestIncrease(unit, neighbor, 1));
				}
			}
		}
	}
	




}
*/
