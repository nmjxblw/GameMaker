using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
#if true
namespace GameMaker.Utilities
{
	[Serializable]
	public class QuickDictionary<TKey, TValue>
	{
		/// <summary>
		/// 私有构造
		/// </summary>
		QuickDictionary() { }
		public object this[object key]
		{
			get
			{
				return new object();
			}
			set
			{
				return;
			}
		}
	}
}
#endif