using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using System.Linq.Expressions;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Concurrent;
using System.Reflection;
namespace GameMaker
{
	/// <summary>
	/// 使用ConcurrentDictionary实现的动态基类
	/// </summary>
	public class DynamicAsset : DynamicObject, IDisposable
	{
		public DynamicAsset(params object[] param)
		{
			Param = param;
		}
		public override string ToString()
		{
			return string.Join(", ", _values);
		}
		/// <summary>
		/// 特殊参数
		/// </summary>
		public object Param { get; set; }
		// 使用线程安全的 ConcurrentDictionary 存储键值对
		private readonly ConcurrentDictionary<string, object> _values = new ConcurrentDictionary<string, object>(StringComparer.OrdinalIgnoreCase);

		// 存储每个键对应的值类型
		private readonly ConcurrentDictionary<string, Type> _types = new ConcurrentDictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

		// 重写 TrySetIndex：实现索引器设置逻辑
		public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
		{
			if (value == null) return false;
			if (indexes.Length == 1 && indexes[0] is string key)
			{
				_values[key] = value;
				_types[key] = value.GetType(); // 记录值的类型（值为 null 时类型为 null）
				return true;
			}
			return false;
		}

		// 重写 TryGetIndex：实现索引器获取逻辑
		public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
		{
			if (indexes.Length == 1 && indexes[0] is string key)
			{
				return _values.TryGetValue(key, out result);
			}
			result = null;
			return false;
		}

		// 重写 TrySetMember：支持动态属性赋值（如 obj.Name = "Alice"）
		public override bool TrySetMember(SetMemberBinder binder, object value)
		{
			if (value == null) return false;
			string key = binder.Name;
			_values[key] = value;
			_types[key] = value.GetType();
			return true;
		}

		// 重写 TryGetMember：支持动态属性读取（如 obj.Name）
		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			return _values.TryGetValue(binder.Name, out result);
		}

		// 返回存储值的类型
		public virtual Type GetMemberType(string key)
		{
			return _types.TryGetValue(key, out Type type) ? type : null;
		}

		// 返回所有键名
		public override IEnumerable<string> GetDynamicMemberNames()
		{
			return _values.Keys;
		}
		// 重写 TryDeleteMember：删除单个成员（支持动态语法）
		public override bool TryDeleteMember(DeleteMemberBinder binder)
		{
			string key = binder.Name;
			return TryRemoveMemberInternal(key);
		}
		// 内部删除逻辑
		protected virtual bool TryRemoveMemberInternal(string key)
		{
			bool removedValue = _values.TryRemove(key, out _);
			bool removedType = _types.TryRemove(key, out _);
			return removedValue || removedType;
		}
		/// <summary>
		/// 清空
		/// </summary>
		public void Clear()
		{
			_values.Clear();
			_types.Clear();
		}
		/// <summary>
		/// 批量删除成员
		/// </summary>
		/// <param name="keys"></param>
		/// <returns></returns>
		public virtual int Remove(params string[] keys)
		{
			if (keys == default) return 0;
			int count = 0;
			foreach (string key in keys)
			{
				if (string.IsNullOrEmpty(key)) continue;
				if (TryRemoveMemberInternal(key)) count++;
			}
			return count;
		}

		public virtual void Dispose()
		{
			this.Clear();
			GC.SuppressFinalize(this);
		}
	}
}
