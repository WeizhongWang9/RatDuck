using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardGame.Game.GameTerms
{
	public class InGameObject : IHandle
	{
		public ulong handle = 0;

		public ulong getHandle()
		{
			return handle;
		}
	}

}
