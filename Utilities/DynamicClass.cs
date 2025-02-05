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
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections;
namespace GameMaker
{
	/// <summary>
	/// 能够序列化Json动态类
	/// </summary>
	[JsonConverter(typeof(DynamicClassConverter))]
	public class DynamicClass : DynamicObject
	{
		private readonly ConcurrentDictionary<string, object> _properties = new ConcurrentDictionary<string, object>();

		// 动态属性访问
		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			return _properties.TryGetValue(binder.Name, out result);
		}

		public override bool TrySetMember(SetMemberBinder binder, object value = default)
		{
			if (value == default) return false;
			_properties[binder.Name] = value;
			return true;
		}

		// 索引器访问
		public object this[string key]
		{
			get
			{
				if (string.IsNullOrEmpty(key)) return default;
				return _properties.TryGetValue(key, out var value) ? value : default;
			}
			set
			{
				if (!string.IsNullOrEmpty(key))
					_properties[key] = value ?? "null";
			}
		}

		// 获取所有属性
		public IEnumerable<KeyValuePair<string, object>> Properties => _properties;
		// 返回所有键名
		public override IEnumerable<string> GetDynamicMemberNames()
		{
			return _properties.Keys;
		}
		// 重写 TryDeleteMember：删除单个成员（支持动态语法）
		public override bool TryDeleteMember(DeleteMemberBinder binder)
		{
			string key = binder.Name;
			return TryRemoveMemberInternal(key);
		}
		// 内部删除逻辑
		private bool TryRemoveMemberInternal(string key)
		{
			return _properties.TryRemove(key, out _);
		}
		/// <summary>
		/// 清空
		/// </summary>
		public void Clear()
		{
			_properties.Clear();
		}
		/// <summary>
		/// 批量删除成员
		/// </summary>
		/// <param name="keys"></param>
		/// <returns></returns>
		public int Remove(params string[] keys)
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
	}

	public class DynamicClassConverter : JsonConverter<DynamicClass>
	{
		public override DynamicClass Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			var data = new DynamicClass();
			ReadObject(ref reader, data, options);
			return data;
		}

		private void ReadObject(ref Utf8JsonReader reader, DynamicClass data, JsonSerializerOptions options)
		{
			while (reader.Read())
			{
				if (reader.TokenType == JsonTokenType.EndObject)
				{
					return;
				}

				if (reader.TokenType == JsonTokenType.PropertyName)
				{
					var propertyName = reader.GetString();
					reader.Read();
					data[propertyName] = ReadValue(ref reader, options);
				}
			}
		}

		private object ReadValue(ref Utf8JsonReader reader, JsonSerializerOptions options)
		{
			switch (reader.TokenType)
			{
				case JsonTokenType.StartObject:
					var nestedData = new DynamicClass();
					ReadObject(ref reader, nestedData, options);
					return nestedData;

				case JsonTokenType.StartArray:
					var list = new List<object>();
					while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
					{
						list.Add(ReadValue(ref reader, options) ?? "null");
					}
					return list;

				case JsonTokenType.String:
					return reader.GetString();

				case JsonTokenType.Number:
					if (reader.TryGetInt32(out int intValue)) return intValue;
					if (reader.TryGetInt64(out long longValue)) return longValue;
					return reader.GetDecimal();

				case JsonTokenType.True:
					return true;

				case JsonTokenType.False:
					return false;

				case JsonTokenType.Null:
					return null;

				default:
					throw new JsonException($"Unsupported token type: {reader.TokenType}");
			}
		}

		public override void Write(Utf8JsonWriter writer, DynamicClass value, JsonSerializerOptions options)
		{
			writer.WriteStartObject();
			foreach (var kvp in value.Properties)
			{
				writer.WritePropertyName(kvp.Key);
				WriteValue(writer, kvp.Value, options);
			}
			writer.WriteEndObject();
		}

		private static void WriteValue(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
		{
			switch (value)
			{
				case null:
					writer.WriteNullValue();
					break;

				case DynamicClass data:
					JsonSerializer.Serialize(writer, data, options);
					break;

				case IList list:
					writer.WriteStartArray();
					foreach (var item in list)
					{
						WriteValue(writer, item, options);
					}
					writer.WriteEndArray();
					break;

				default:
					JsonSerializer.Serialize(writer, value, value.GetType(), options);
					break;
			}
		}

	}
}
