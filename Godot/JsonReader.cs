using Godot;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatDuck.Script.Godot
{
	public class JsonReader<T>
	{
		Random randGen = new(Guid.NewGuid().GetHashCode());
		List<T> _infos;
		public List<T> infos { get { return _infos; } }
		public JsonReader(Json json)
		{
			var stringCollection = Json.Stringify(json.Data);
			_infos = JsonConvert.DeserializeObject<List<T>>(stringCollection);
		}

		public T rollTokenLabel(bool rollOut = false)
		{
			var list = _infos;
			var randNumber = randGen.Next() % list.Count;
			var ele = list[randNumber];
			if (rollOut) { list.Remove(ele); }
			return ele;
		}
	}
}
