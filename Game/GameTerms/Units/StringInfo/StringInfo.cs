using CardGame.Game.GameTerms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatDuck.Script.Game.GameTerms.Units.StringInfo
{
    public class StringInfos
    {

        static string[] roleString = init();
        static string[] init()
        {
            var strings = new string[3];
            roleString[(int)Roles.Police] = "Police";
            roleString[(int)Roles.Dissent] = "Dissent";
            roleString[(int)Roles.Scientist] = "Scientist";
            return strings;
        }
        public static string getRoleString(Roles role)
        {
            return roleString[(int)role];
        }


    }
}
