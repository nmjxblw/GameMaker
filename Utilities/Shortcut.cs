using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Text.RegularExpressions;
namespace GameMaker
{
	public static class Shortcut
	{
		/// <summary>
		/// 静态构造函数
		/// </summary>
		static Shortcut() { }
		/// <summary>
		/// 报错窗口快捷方法
		/// </summary>
		/// <param name="content"></param>
		public static void Error(params string[] content)
		{
			System.Windows.MessageBox.Show(string.Join("\n", content), "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
		}
		/// <summary>
		/// 使用窗口打开文件
		/// </summary>
		/// <param name="filter"></param>
		/// <returns>成功打开文件时会返回FileName，否则返回 default </returns>
		[STAThread]
		public static string OpenFile(string filter = default, bool multiselect = false)
		{
			try
			{
				System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog()
				{
					Filter = filter ?? "All Files(*.*) | *.*",
					Multiselect = multiselect,
				};
				if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					return openFileDialog.FileName;
				}
				else
				{
					return default;
				}
			}
			catch (Exception ex)
			{
				Recorder.RecordError(ex.Message);
				return default;
			}
		}
		/// <summary>
		/// 使用窗口打开文件夹
		/// </summary>
		/// <returns>文件夹的路径</returns>
		public static string OpenFolder()
		{
			try
			{
				System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
				if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					return folderBrowserDialog.SelectedPath;
				}
				else
				{
					return default;
				}
			}
			catch (Exception ex)
			{
				Recorder.RecordError(err_msg: ex.Message);
				return default;
			}
		}
		/// <summary>
		/// 正则表达式缓存
		/// </summary>
		public static Dictionary<string, Regex> Regexes { get; } = new Dictionary<string, Regex>{
			{   "FileExtension".ToLower(),new Regex(@"\.[^.]+$")},
			{   "Float".ToLower(),new Regex(@"[+-]?\d+\.?\d+?")},
			{   "Hex".ToLower(), new Regex(@"(0x)?([0-9a-f]+|[0-9A-F]+)")},
		};
	}
}
