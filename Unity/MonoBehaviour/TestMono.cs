using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameMaker
{
	public class TestMono: MonoBehaviour
	{
		void Awake() {
			Recorder.Record("测试成功");
		}
	}
}
