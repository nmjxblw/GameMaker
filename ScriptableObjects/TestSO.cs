using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace GameMaker.ScriptableObjects
{
	/// <summary>
	/// 测试用ScriptableObject
	/// </summary>
	[Serializable]
	[CreateAssetMenu(fileName = "GameMaker/ScriptableObjects", menuName = "TestSO")]
	public class TestSO : ScriptableObject
	{
		/// <summary>
		/// 测试用字段
		/// </summary>
		public string s;
	}
}
