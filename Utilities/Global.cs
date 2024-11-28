using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	/// <summary>
	/// 全局静态字典
	/// </summary>
	public static class Global
	{
		/// <summary>
		/// 私有构造函数
		/// </summary>
		static Global() { }
		/// <summary>
		/// 私有字典
		/// </summary>
		private static readonly Dictionary<object, dynamic> _storage = new Dictionary<object, dynamic>();
		/// <summary>
		/// 获取只读字典
		/// </summary>
		public static Dictionary<object, dynamic> Storage => _storage;
		/// <summary>
		/// 获取值
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static dynamic Get(object key)
		{
			_storage.TryGetValue(key, out var value);
			return value;
		}
		/// <summary>
		/// 设置键值对
		/// </summary>
		/// <param name="key">键</param>
		/// <param name="value">值</param>
		public static void Set(object key, object value) => _storage[key] = value;
		/// <summary>
		/// 查询
		/// </summary>
		/// <param name="key">键</param>
		/// <returns>是否存在</returns>
		public static bool ContainsKey(object key) => _storage.ContainsKey(key);
		/// <summary>
		/// 移除
		/// </summary>
		/// <param name="key">键</param>
		/// <returns>是否移除成功</returns>
		public static bool Remove(object key) => _storage.Remove(key);
		/// <summary>
		/// 移除所有
		/// </summary>
		public static void Clear() => _storage.Clear();
	}
}
