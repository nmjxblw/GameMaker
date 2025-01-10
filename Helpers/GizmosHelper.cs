using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace GameMaker.Helpers
{
	/// <summary>
	/// Gizmos帮助类
	/// </summary>
	public class GizmosHelper : MonoBehaviour
	{
		private static GizmosHelper _instance;
		/// <summary>
		/// Gizmos单例示例化引用
		/// </summary>
		public static GizmosHelper Instance
		{
			get
			{
				GizmosHelper temp = GameObject.FindFirstObjectByType<GizmosHelper>().GetComponent<GizmosHelper>();
				if (temp == null)
				{
					GameObject go = new GameObject("GizmosManager");
					temp = go.AddComponent<GizmosHelper>();
				}
				return _instance = temp;
			}
		}
		private void Awake()
		{
			if (_instance != null && _instance != this)
			{
				Destroy(gameObject);
				return;
			}
			_instance = this;
		}
		/// <summary>
		/// 配置类
		/// </summary>
		[Serializable]
		public class GizmosConfig
		{
			/// <summary>
			/// 枚举型
			/// </summary>
			public enum GizmosType
			{
				/// <summary>
				/// 球形
				/// </summary>
				Sphere,
				/// <summary>
				/// 方块
				/// </summary>
				Cube,
				/// <summary>
				/// 
				/// </summary>
				WireCube,
				/// <summary>
				/// 
				/// </summary>
				WireSphere,
				/// <summary>
				/// 
				/// </summary>
				Line,
				/// <summary>
				/// 
				/// </summary>
				Ray,
				/// <summary>
				/// 
				/// </summary>
				Icon,
				/// <summary>
				/// 
				/// </summary>
				Label,
				/// <summary>
				/// 
				/// </summary>
				Mesh,
				/// <summary>
				/// 
				/// </summary>
				Text
			}
			/// <summary>
			/// 
			/// </summary>
			public GizmosType type;
			/// <summary>
			/// 
			/// </summary>
			public Color color = Color.red;
			/// <summary>
			/// 
			/// </summary>
			public Vector3 size = Vector3.one;
			/// <summary>
			/// 
			/// </summary>
			public Vector3 position = Vector3.zero;
			/// <summary>
			/// 
			/// </summary>
			public Transform transform;
		}
		/// <summary>
		/// 
		/// </summary>
		public List<GizmosConfig> gizmosConfigs = new List<GizmosConfig>();
		/// <summary>
		/// 
		/// </summary>
		public void OnDrawGizmos()
		{
			for (int i = 0; i < gizmosConfigs.Count; i++)
			{
				GizmosConfig config = gizmosConfigs[i];
				Gizmos.color = config.color;
				switch (config.type)
				{
					case GizmosConfig.GizmosType.Sphere:
						Gizmos.DrawSphere(config.position, config.size.x);
						break;
					case GizmosConfig.GizmosType.Cube:
						Gizmos.DrawCube(config.position, config.size);
						break;
					case GizmosConfig.GizmosType.WireCube:
						Gizmos.DrawWireCube(config.position, config.size);
						break;
					case GizmosConfig.GizmosType.WireSphere:
						Gizmos.DrawWireSphere(config.position, config.size.x);
						break;
					case GizmosConfig.GizmosType.Line:
						Gizmos.DrawLine(config.position, config.position + config.size);
						break;
					case GizmosConfig.GizmosType.Ray:
						Gizmos.DrawRay(config.position, config.size);
						break;
					case GizmosConfig.GizmosType.Icon:
						Gizmos.DrawIcon(config.position, config.size.x.ToString());
						break;
					case GizmosConfig.GizmosType.Label:
						Gizmos.DrawIcon(config.position, config.size.x.ToString());
						break;
					case GizmosConfig.GizmosType.Mesh:
						Gizmos.DrawMesh(config.transform.GetComponent<MeshFilter>().sharedMesh, config.position, config.transform.rotation, config.size);
						break;
					case GizmosConfig.GizmosType.Text:
						Gizmos.DrawMesh(config.transform.GetComponent<MeshFilter>().sharedMesh, config.position, config.transform.rotation, config.size);
						break;
				}
			}
		}
	}
}
