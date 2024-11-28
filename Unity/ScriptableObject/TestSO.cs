using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace GameMaker
{
	[CreateAssetMenu(fileName = "TestSO", menuName = "GameMaker")]
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
			string s = m_TextAsset.text;
			Debug.Log($"s={s}");
		}
	}
}
namespace GameMaker.Unity.Test
{
	public class Config
	{

	}
}
