using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace CardGame.Game.GameTerms
{

	public interface IHandle
	{
		ulong getHandle();
	}
	public class HandleManager<T> where T: IHandle
	{
		ulong objectCount = 0;
		readonly Dictionary<ulong, T> keyValuePairs = new Dictionary<ulong, T>();
		/// <summary>
		/// Associate a handle on an object.
		/// </summary>
		/// <returns></returns>
		public ulong assignObjectID(T obj)
		{
			if (obj.getHandle() == 0)
			{
				objectCount++;
				var handle = objectCount;
				keyValuePairs.Add(handle, obj);
				return handle;
			}
			else
				throw (new Exception("Attempt to assign multiple handles on an object."));
		}
		public T getObject(ulong handle)
		{
			if (keyValuePairs.TryGetValue(handle, out var obj))
			{  return obj; }
			return default;
		}
		void removeObject(ulong handle)
		{
			keyValuePairs.Remove(handle);
		}
		/// <summary>
		/// erase handle.
		/// </summary>
		/// <param name="obj"></param>
		public void removeObject(T obj)
		{
			removeObject(obj.getHandle());
		}
	}
}
