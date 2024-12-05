#if UNITY_EDITOR
#pragma warning disable CS0649
#endif
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using System.Dynamic;
namespace GameMaker
{
	[CreateAssetMenu(fileName = "TestSO", menuName = "GameMaker/TestSO")]
	public class TestSO : ScriptableObject, IComparable<TestSO>
	{
		[SerializeField]

		private TextAsset m_TextAsset;

		public int CompareTo(TestSO other)
		{
			return this.name.CompareTo(other.name);
		}

		void OnEnable()
		{
			string s = m_TextAsset?.text;
			Debug.Log($"s={s}");
		}
	}
}
namespace GameMaker.Unity.Test
{
	[Serializable]
	/// <summary>
	/// 测试用config
	/// </summary>
	public class Config
	{
		public string name;
		public dynamic data;
	}
}
