﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using System.Text.Json.Serialization;
using System.Text.Encodings.Web;
using System.Text.Unicode;
namespace GameMaker
{
	/// <summary>
	/// Json帮助类
	/// 注意：仅允许序列化属性而不是字段
	/// </summary>
	/// <example>
	/// 这是一个示例类，展示如何使用 Json帮助类：
	/// <code>
	/// public class TestClass
	/// {
	///     public TestClass(string name = "Bob", int age = 15) 
	///     { 
	///         this.Name = name; 
	///         this.Age = age; 
	///     }
	///     
	///     public string Name { get; set; } = "Bob";
	///     public int Age { get; set; } = 15;
	/// }
	/// </code>
	/// </example>
	public static class JsonHelper
	{
		public readonly static string MainJsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "JsonFiles");
		private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions()
		{
			PropertyNameCaseInsensitive = true, // 属性名不区分大小写
			AllowTrailingCommas = true, // 允许尾随逗号
			ReadCommentHandling = JsonCommentHandling.Skip, // 跳过注释
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // 驼峰命名
			WriteIndented = true, // 增加阅读性
			DefaultIgnoreCondition = JsonIgnoreCondition.Never, // 确保不忽略任何字段
			Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase), }, // 将枚举型转化成字符串
			Encoder = JavaScriptEncoder.Create(UnicodeRanges.All), // 创建一个支持特定 Unicode 范围的编码器。
		};
		private readonly static object LockObj = new object();
		static JsonHelper()
		{
			if (!Directory.Exists(MainJsonFilePath))
			{
				Directory.CreateDirectory(MainJsonFilePath);
			}
		}
		/// <summary>
		/// 加载Json文件
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public static T Load<T>(string fileName)
		{
			if (!fileName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
			{
				fileName += ".json";
			}
			if (!File.Exists(Path.Combine(MainJsonFilePath, fileName)))
				return default;
			string jsonString = File.ReadAllText(Path.Combine(MainJsonFilePath, fileName));
			if (string.IsNullOrEmpty(jsonString))
				return default;
			return JsonSerializer.Deserialize<T>(jsonString, JsonOptions);
		}
		/// <summary>
		/// 存写Json文件
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="data"></param>
		/// <param name="fileName"></param>
		/// <param name="customPath"></param>
		/// <returns></returns>
		public static bool Save<T>(T data, string fileName = null, string customPath = null)
		{
			try
			{
				lock (LockObj) // 确保线程安全
				{
					if (string.IsNullOrEmpty(fileName))
					{
						fileName = data.ToString();
					}
					if (!fileName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
					{
						fileName += ".json";
					}
					// 确定保存路径
					string savePath = string.IsNullOrEmpty(customPath) ? MainJsonFilePath : customPath;

					// 如果自定义路径不存在，则创建它
					if (!Directory.Exists(savePath))
					{
						Directory.CreateDirectory(savePath);
					}

					string jsonString = JsonSerializer.Serialize(data, JsonOptions);
					string filePath = Path.Combine(savePath, fileName);

					// 使用 StreamWriter 写入文件
					using (StreamWriter writer = new StreamWriter(filePath, false))
					{
						writer.Write(jsonString);
					}
					return true;
				}
			}
			catch
			{
				return false;
			}
		}
	}
}