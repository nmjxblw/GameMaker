using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	/// <summary>
	/// 记录器
	/// </summary>
	public static class Recorder
	{
		/// <summary>
		/// 功能启用标识符
		/// </summary>
		public static bool Enable = false;
		private static readonly object LockObj = new object();
		private static readonly string MainRecordsPath = Directory.GetCurrentDirectory() + $"\\Recorder\\MainRecords";
		private static readonly string ErrorRecordsPath = Directory.GetCurrentDirectory() + $"\\Recorder\\ErrorRecords";
		private static readonly List<string> RecordPaths = new List<string>() { MainRecordsPath, ErrorRecordsPath };
		/// <summary>
		/// 私有构造，在构造时自动注册记录报错
		/// </summary>
		static Recorder()
		{
			Enable = true;
			foreach (string filePath in RecordPaths)
			{
				string path = PathHelper.GetRuntimeDirectory(filePath);
				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}
			}
			AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
			{
				Exception ex = (Exception)e.ExceptionObject;
				RecordError(ex.Message);
				RecordError(ex.StackTrace);
				Trace.WriteLine(ex.StackTrace);
			};
		}
		/// <summary>
		/// 写入
		/// </summary>
		/// <param name="enable">是否启用</param>
		/// <param name="filePath">存写文件位置</param>
		/// <param name="msg">存写内容</param>
		public static void Write(string filePath, string msg, bool enable = true)
		{

			if (!enable) return;

			string path = PathHelper.GetRuntimeDirectory(filePath);

			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			lock (LockObj)
			{
				string file = $"{path}\\{DateTime.Now:yyyy-MM-dd}.txt";
				string text = $"[{DateTime.Now:HH:mm:ss}] {msg}\n";

				file = PathHelper.GetRuntimeDirectory(file);
				File.AppendAllText(file, text);
			}
		}
		/// <summary>
		/// 标准记录
		/// </summary>
		/// <param name="msg"></param>
		public static void Record(string msg)
		{
			StackTrace stackTrace = new StackTrace();
			StackFrame stackFrame = stackTrace.GetFrame(1);
			string callingName = stackFrame.GetMethod().DeclaringType.FullName;
			Write(RecordPaths[0], string.Format("[{0:50}]\t{1}", callingName, msg), Enable);
		}
		/// <summary>
		/// 报错记录
		/// </summary>
		/// <param name="err_msg"></param>
		public static void RecordError(string err_msg) => Write(RecordPaths[1], "\n" + err_msg, Enable);
	}
	/// <summary>
	/// 路径辅助类
	/// </summary>
	public class PathHelper
	{
		/// <summary>
		/// 将路径转换成OS可识别的路径
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static string GetRuntimeDirectory(string path)
		{
			//ForLinux
			if (IsLinuxRunTime())
				return GetLinuxDirectory(path);
			//ForWindows
			if (IsWindowRunTime())
				return GetWindowDirectory(path);
			return path;
		}

		//OSPlatform.Windows监测运行环境
		private static bool IsWindowRunTime()
		{
			return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
		}

		//OSPlatform.Linux运行环境
		private static bool IsLinuxRunTime()
		{
			return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
		}

		private static string GetLinuxDirectory(string path)
		{
			string pathTemp = Path.Combine(path);
			return pathTemp.Replace("\\", "/");
		}
		private static string GetWindowDirectory(string path)
		{
			string pathTemp = Path.Combine(path);
			return pathTemp.Replace("/", "\\");
		}
	}
}
