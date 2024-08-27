using CardGame.Game.GameTerms.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardGame.Game.GameTerms.Units
{
    public class Role : Token
    {
        public Role(Game game,Player player):base(game)
        {
            var handle = game.abilityHandle;
            handle.abilityMoney.init(this);
            handle.abilityMove.init(this, new Abilities.AbilityMove.Attribute(0,1));
            handle.abilityPlayer.init(this, player);
        }

    }

    public class Gov : Role
    {
        public Gov(Game game, Player player) : base(game,player)
        {
            var handle = game.abilityHandle;
            handle.abilityCarry.init(this);
           
           // handle.abilityCarry.setData(this);
        }
    }

    public class Police:Gov
    {
        public Police(Game game, Player player) : base(game, player)
        {
            var handle = game.abilityHandle;
            handle.abilityPolice.init(this);

        }
    }

    public class Diss : Role
    {
        public Diss(Game game, Player player) : base(game, player)
        {
            var handle = game.abilityHandle;
            handle.abilityDiss.init(this);

        }
    }

    public class Scientist:Role
    {
        public Scientist(Game game, Player player) : base(game, player)
        {
            var handle = Global.getAbilityHandle();


        }
    }

}
