using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace GameMaker.Utilities
{

	/// <summary>
	/// 可序列化字典
	/// </summary>
	/// <typeparam name="TKey"></typeparam>
	/// <typeparam name="TValue"></typeparam>
	[Serializable]
	public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
	{
		[SerializeField]
		private List<TKey> keys = new List<TKey>();

		[SerializeField]
		private List<TValue> values = new List<TValue>();
		/// <summary>
		/// 反序列化时调用
		/// </summary>
		/// <exception cref="Exception"></exception>
		public void OnAfterDeserialize()
		{
			this.Clear();

			if (keys.Count != values.Count)
				throw new Exception("there are " + keys.Count + " keys and " + values.Count + " values after deserializing, they don't match.");

			for (int i = 0; i < keys.Count; i++)
			{
				this[keys[i]] = values[i];
			}
		}

		/// <summary>
		/// 序列化时调用
		/// </summary>
		public void OnBeforeSerialize()
		{
			keys = new List<TKey>(this.Keys);
			values = new List<TValue>(this.Values);
		}
	}
}
