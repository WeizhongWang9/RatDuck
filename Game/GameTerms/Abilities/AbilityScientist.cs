using CardGame.Game.GameTerms.Units;
using CardGame.GameEvents.UnitEvents;
using CardGame.Lib.Deck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace CardGame.Game.GameTerms.Abilities
{
	public class CONST_SCI
	{
		public const int RESEARCH_EARN = 10;
		public const int RESEARCH_BIO_ADD = 1;
	}

	public class ScientistResearch : UnitEvents
	{
		public Body body;
		public ScientistResearch(Unit unit, Body body) : base(unit) { 
			this.body = body;
			Global.getEventManager().emitEvent(this);
		}
	}
	public class AbilityScientist
	{
		Game game;
		AbilityMoney abilityMoney;
		AbilityLoc abilityLoc;
		eventDictionary<Data> data = new eventDictionary< Data>();
		DeckWithShowcase<BodyPlaceCard> _deck;
		public DeckWithShowcase<BodyPlaceCard> deck => _deck;
		int currentCardAmount;

		DelTrigger<ScientistResearch> _researchTrig;
		
		public AbilityScientist(Game game,AbilityHandle abilityHandle)
		{
			this.game = game;
			currentCardAmount = 0;
			abilityLoc = abilityHandle.abilityLoc;
			abilityMoney = abilityHandle.abilityMoney;
			_researchTrig = new DelTrigger<ScientistResearch>(onResearch, 0);
		}

		void onResearch(ScientistResearch research)
		{
			var unit = research.getTriggerUnit();
			var body = research.body;

			new UnitLeaveMap(body, abilityLoc[body]);
			abilityMoney.addValue(unit, CONST_SCI.RESEARCH_EARN);
			game.map.addBioProgess(CONST_SCI.RESEARCH_BIO_ADD);
		}

		public void init(Deck<BodyPlaceCard> deck, int maxCap)
		{
			var shuffle = new Shuffler();
			shuffle.Shuffle<BodyPlaceCard>(deck);
			this._deck = new(deck,maxCap);

		}

		public bool canGetNewPlaceCard()
		{
			return currentCardAmount < 2;
		}
		public bool tryGetNewPlaceCard()
		{
			if (canGetNewPlaceCard())
			{
				deck.drawToShowcase(true, true);
				return true;
			}
			return false;
		}
		public bool canCatch(Unit unit, BodyPlaceCard card)
		{
			if (abilityLoc.tryGetLoc(unit, out var loc))
				return loc == card.gameNode;
			return false;
		}
		public bool tryCatch(Unit unit, BodyPlaceCard card)
		{
			if(canCatch(unit, card))
			{
				new TokenCreate(new Token(game), card.gameNode);
				bodyCardDiscard(card);
				deck.drawToShowcase(true, true);
				return true;
			}
			return false;
		}
		public Deck<BodyPlaceCard> getShowcase() { return deck.showcase; }
		public bool tryResearch(Unit unit, Body body)
		{
			if (abilityLoc[body] == game.map.lab)
			{
				abilityMoney.addValue(unit, CONST_SCI.RESEARCH_EARN);
				game.map.addBioProgess(1);
				return true;
			}
			return false;
		}

		public void bodyCardDiscard(BodyPlaceCard card)
		{
			deck.discard(card);
		}

		protected class Data
		{

		}

		public bool hasAbility(Unit unit)
		{ return data.ContainsKey(unit); }

		

	}
}
