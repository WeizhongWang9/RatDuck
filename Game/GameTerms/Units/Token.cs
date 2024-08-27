using CardGame.Game.GameEvents;
using CardGame.Game.GameTerms.Abilities;
using CardGame.Game.GameTerms.PlayerUtility;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardGame.Game.GameTerms.Units
{
	public class CONST_TOKEN
	{
		public const int CREATE_WEIGHT = 1;

		public class AbilityToken
		{
			AbilityLoc abilityLoc;
			AbilityGameNode abilityGameNode;
			GameTrigger<TokenCreate> tokenCreated;
			public AbilityToken(AbilityLoc abilityLoc, AbilityGameNode abilityGameNode)
			{
				tokenCreated = new GameTrigger<TokenCreate>(onTokenCreated, CREATE_WEIGHT);
				this.abilityLoc = abilityLoc;
				this.abilityGameNode = abilityGameNode;
			}
			void onTokenCreated(TokenCreate tokenCreate)
			{
				var unit = tokenCreate.unit as Token;
				var loc = tokenCreate.loc;
				abilityLoc[unit] = loc;
				abilityGameNode.unitEnterByCreating(unit, loc);
			}

		}
	}
	/// <summary>
	/// Every object that can be attached on a game node is a token.
	/// For example, characters and game tokens.
	/// A card is not a token as it attachs to a player.
	/// </summary>
	public class Token : Unit
	{
		public Token(Game game):base(game)
		{
			var handle = game.abilityHandle;
			handle.abilityLoc.initUnit(this);
			
		}
		
	}
	public class Body : Token
	{
		public string name;
		public string description;
		public Body(Game game) : base(game)
		{
			var handle = game.abilityHandle;
			handle.abilityCarry.init(this);
		}
	}
	public class BodyFactory
	{
		public string name;
		public string description;
		Game game;
		public BodyFactory(string name, string description, Game game)
		{
			this.name = name;
			this.description = description;
			this.game = game;
		}
		public Body newBody()
		{
			var body = new Body(game);
			body.name = name;
			body.description = description;
			return body;
		}
	}

	public class TokenCreate : UnitCreated
	{
		public GameNode loc;
		public TokenCreate(Token token, GameNode loc) : base(token)
		{
			this.loc = loc;
			Global.getEventManager().emitEvent(this);
		}
	}



}
