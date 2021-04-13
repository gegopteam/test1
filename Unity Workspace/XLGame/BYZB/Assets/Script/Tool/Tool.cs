/* author:KinSen
 * Date:2017.07.04
 */

//#define UNITY_REMOVE_LOG //注：要发布应用时打开该宏进行编译

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using UnityEngine;
using System;


#if UNITY_EDITOR
using UnityEditor;
#endif

/*用法：
 * 如果要在UI上现实日志信息则：
 * 在MonoBehaviour继承的类中添加代码：
 * 
void OnGUI()
{
	Tool.ShowInGUI ();
}
 * 
 */

public class TypeLog
{
	public const int Normal = 1;
	public const int Warning = 2;
	public const int Error = 3;
}

//负责统一管理Log日志，打开关闭日志消息
public class Tool
{
	public static object changeMsg = new object ();
	public static object logFileIO = new object ();
	public static string msg = "";
	//信息内容
	public static bool showInGUI = false;
	//是否现实在UI上
	public static bool logAll = true;
	//所有日志的开关
	public static bool logNormal = true;
	//普通日志的开关
	public static bool logWarning = false;
	//警告日志的开关
	public static bool logError = true;
	//错误日志的开关

	public static string outLogPathFile = "";

	// #if UNITY_EDITOR
	// public static string outLogPathFile = Application.dataPath + "/OutLog.txt";
	// #else
	// public static string outLogPathFile = Application.persistentDataPath + "/OutLog.txt";
	// #endif

	public static void Init ()
	{//需要主动初始化
		#if UNITY_EDITOR
		outLogPathFile = Application.dataPath + "/OutLog.txt";
		#else
		outLogPathFile = Application.persistentDataPath + "/OutLog.txt";
		#endif
	}

	//----------------------------------------LOG-----------------------------------------//

	public static void Log (object message, bool debugLog = true)
	{
		#if UNITY_REMOVE_LOG
		return;
		#endif

		if (!logAll)
			return;
		if (!logNormal)
			return;
		
		message += "\n";
		if (debugLog)
			Debug.Log (message);
		AddLogMsg (message);
	}

	public static void LogWarning (object message, bool debugLog = true)
	{
		#if UNITY_REMOVE_LOG
		return;
		#endif

		if (!logAll)
			return;
		if (!logWarning)
			return;
		
		message += "\n";
		if (debugLog)
			Debug.LogWarning (message);
		AddLogMsg (message);
	}

	public static void LogError (object message, bool debugLog = true)
	{
		#if UNITY_REMOVE_LOG
		return;
		#endif

		if (!logAll)
			return;
		if (!logError)
			return;
		
		message += "\n";
		if (debugLog)
			Debug.LogError (message);
		AddLogMsg (message);
	}

	public static void AddLogMsg (object msg)
	{
		#if UNITY_REMOVE_LOG
		return;
		#endif

		msg += "\n";
		lock (changeMsg) {
			Tool.msg += msg;
		}
	}

	public static void ClearMsg ()
	{
		#if UNITY_REMOVE_LOG
		return;
		#endif

		lock (changeMsg) {
			Tool.msg = "";
		}
	}

	//----------------------------------------LOG-TO-UI-----------------------------------------//

	private static Vector2 ptBtn = new Vector2 (10, 10);
	private static Vector2 sizeBtn = new Vector2 (80, 40);
	private static Vector2 ptArea = new Vector2 (ptBtn.x, ptBtn.y + sizeBtn.y + 1);
	private static Vector2 sizeArea = new Vector2 (1000, 700);

	static Vector2 m_ScorllView;

	public static void ShowInGUI ()
	{
//		Vector2 ptBtn = new Vector2 (10, 10);
//		Vector2 sizeBtn = new Vector2 (80, 40);
//		Vector2 ptArea = new Vector2(ptBtn.x, ptBtn.y+sizeBtn.y+1);
//		Vector2 sizeArea = new Vector2(1000, 700);
		#if UNITY_REMOVE_LOG
		return;
		#else
		#if UNITY_EDITOR
			
		#else
			
		#endif
		#endif

		GUI.skin.verticalScrollbar.fixedWidth = 100f;
		GUI.skin.verticalScrollbarThumb.fixedWidth = 100f;

		if (GUI.Button (new Rect (ptBtn.x, ptBtn.y, sizeBtn.x, sizeBtn.y), "Show/Hide")) {
			Tool.showInGUI = !Tool.showInGUI;
		}

		if (GUI.Button (new Rect (ptBtn.x + sizeBtn.x + 10, ptBtn.y, sizeBtn.x, sizeBtn.y), "ClearMsg")) {
			Tool.ClearMsg ();
		}
		if (Tool.showInGUI) {
			//GUI.TextArea (new Rect (ptArea.x, ptArea.y, sizeArea.x, sizeArea.y), Tool.msg);
			//GUI.TextField (new Rect (ptArea.x, ptArea.y, sizeArea.x, sizeArea.y), Tool.msg);
			//GUI.TextField (new Rect (10, 40, 1000, 700), Tool.msg);
		}
		m_ScorllView = GUILayout.BeginScrollView (m_ScorllView);
		if (Tool.showInGUI) {
			GUILayout.Label (Tool.msg, GUILayout.MinWidth (Screen.width));
			//GUILayout.Label (Tool.msg, GUI.TextArea (new Rect (ptArea.x, ptArea.y, sizeArea.x, sizeArea.y), Tool.msg), GUILayout.MinWidth (Screen.width));
		}
		GUILayout.EndScrollView ();
	}

	//----------------------------------------LOG_TO_FILE-----------------------------------------//

	public static bool OutLogFileIsExist ()
	{
		#if UNITY_REMOVE_LOG
		return false;
		#endif

		return FileIsExist (outLogPathFile);
	}

	public static void OutLogFileRemove ()
	{
		#if UNITY_REMOVE_LOG
		return;
		#endif

		FileRemove (outLogPathFile);
	}

	public static void OutLogToFile (object message)
	{
		return;
		#if UNITY_REMOVE_LOG
		return;
		#endif

		FileWrite (outLogPathFile, message.ToString ());
	}

	public static string OutLogFileRead ()
	{
		#if UNITY_REMOVE_LOG
		return "";
		#endif

		return FileRead (outLogPathFile);
	}

	public static void OutLogWithToFile (object message, int typeLog = TypeLog.Normal, bool thread = false)
	{//写日志加输出日志信息
		return;
		#if UNITY_REMOVE_LOG
		return;
		#endif

		OutLogToFile (message);

		if (thread) {
			AddLogMsg (message);
		} else {
			switch (typeLog) {
			case TypeLog.Normal:
				Log (message);
				break;
			case TypeLog.Warning:
				LogWarning (message);
				break;
			case TypeLog.Error:
				LogError (message);
				break;
			}

		}

		return;
	}

	//----------------------------------------FILE-----------------------------------------//

	public static bool FileIsExist (string pathFile)
	{//判断文件是否存在
		if ("" == pathFile)
			return false;

		return System.IO.File.Exists (pathFile);
	}

	public static void FileRemove (string pathFile)
	{//删除文件
		if ("" == pathFile)
			return;

		lock (logFileIO) {
			if (System.IO.File.Exists (outLogPathFile)) {
				File.Delete (outLogPathFile);
			}
		}
	}

	public static void FileWrite (string path, string file, string content, bool removeBefore = false)
	{
		string pathFile = path + file;
		if ("" == pathFile)
			return;
		FileWrite (pathFile, content, removeBefore);
	}

	public static void FileWrite (string pathFile, string content, bool removeBefore)
	{//removeBefore:如果文件存在，写内容前是否删除之前的内容，默认是追加内容
		if ("" == pathFile)
			return;

		if (removeBefore) {
			FileRemove (pathFile);
		}
		FileWrite (pathFile, content);
	}

	public static void FileWrite (string pathFile, string content)
	{
		if ("" == pathFile)
			return;

		lock (logFileIO) {
			//创建文件
			//FileStream fileStream = File.Create (pathFile);
			//FileInfo fileInfo = new FileInfo (pathFile);
			//fileInfo.Create ();
			StreamWriter writer = new StreamWriter (pathFile, true, Encoding.UTF8);
			writer.WriteLine (content);
			writer.Close ();
			writer.Dispose ();
		}
	}

	public static string FileRead (string pathFile)
	{//读取文件内容
		if ("" == pathFile)
			return "";

		string content = "";
		lock (logFileIO) {
			if (System.IO.File.Exists (pathFile)) {
				StreamReader reader = new StreamReader (pathFile);
				//Txt = reader.ReadLine ();
				content = reader.ReadToEnd ();
			} else {
				Tool.Log (pathFile + "不存在");
			}
		}
		return content;
	}

	//----------------------------------------UnityEditor-----------------------------------------//
	//---------------------注：只能在UnityEditor环境下使用，不能打包进App程序---------------------------//

	public static void PauseEditor (bool pause = true)
	{//暂停/恢复UnityEditor
		#if UNITY_EDITOR
		EditorApplication.isPaused = pause;
		#endif
	
	}

	public static void PlayEditor (bool play = false)
	{//停止/启动UnityEditor
		#if UNITY_EDITOR
		EditorApplication.isPlaying = play;
		#endif
	}
	//--------------------------------限制名字----------------------------------
	public static string GetName (string nicName, int length)
	{
		char[] arr = nicName.ToCharArray ();
		int count = 0;
		string temp = "";
		StringBuilder outPut = new StringBuilder ();
		for (int i = 0; i < arr.Length; i++) {
			if (count <= 2 * (length - 1)) {
				if (!ischinese ((int)arr [i])) {
					count = count + 1;
					outPut.Append (arr [i]);

				} else {
					count = count + 2;
					outPut.Append (arr [i]);
				}
			} else {
				return temp + "...";
			}

			if (count == 2 * length || count == 2 * length - 1) {
				temp = outPut.ToString ();
			}
		}
		return outPut.ToString ();
	}

	public static bool ischinese (int c)
	{
		return c > 127;
	}




	public static int GetLength (string text)
	{
		char[] arr = text.ToCharArray ();
		int count = 0;
		for (int i = 0; i < arr.Length; i++) {
			if (ischinese ((int)arr [i]))
				count += 2;
			else
				count += 1;
		}
		return count;
	}
	//换行函数，length--一行能放下多少个字
	public static string GetContent (string nicName, string content, out string text, bool hasBroadCast = false, int type = 0, int length = 34)
	{
		char[] arr = content.ToCharArray ();
		int count = 0;
		string temp = "";
		text = null;
		StringBuilder outPut = new StringBuilder ();
		if (hasBroadCast) {
			if (type == 0) {
				outPut.Append (0);
			} else {
				outPut.Append (1);
			}
			outPut.Append ("                ");
			count += 10;
		}
		if (nicName != null) {
			count += GetLength (nicName);
			count += 2;
			outPut.Append (nicName + "：");
		}
		for (int i = 0; i < arr.Length; i++) {
			if (count <= 2 * (length - 1)) {
				if (!ischinese ((int)arr [i])) {
					count = count + 1;
					outPut.Append (arr [i]);
				} else {
					count = count + 2;
					outPut.Append (arr [i]);
				}
			} else {
				text = content.Substring (i);
				return temp;
			}
			if (count == 2 * length || count == 2 * length - 1) {
				temp = outPut.ToString ();
			}
		}
		return outPut.ToString ();
	}

	public static string GetMD532 (string content)
	{
		string cl = content;
		StringBuilder pwd = new StringBuilder ();
		MD5 md5 = MD5.Create ();//实例化一个md5对像
		// 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
		byte[] s = md5.ComputeHash (Encoding.UTF8.GetBytes (cl));
		// 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
		for (int i = 0; i < s.Length; i++) {
			// 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符
			pwd.Append (s [i].ToString ("X2"));
			//pwd = pwd + s[i].ToString("X");

		}
		return pwd.ToString ();
	}

	public static string GetRankCoinNum (long gold)
	{
		if (gold < 100000) {
			return gold.ToString ();
		} else if (gold < 100000000) {
			StringBuilder strBlider = new  StringBuilder ();
			strBlider.Append (gold / 10000 + ".");
			string temp = (gold % 10000).ToString ();
			switch (temp.Length) {
			case 3:
				strBlider.Append ("0" + (int.Parse (temp) / 100).ToString () + "w");
				break;
			case 4:
				strBlider.Append ((int.Parse (temp) / 100).ToString () + "w");
				break;
			default:
				strBlider.Append ("00w");
				break;
			}
			return strBlider.ToString ();
		} else {
			StringBuilder strBlider1 = new  StringBuilder ();
			strBlider1.Append (gold / 100000000 + ".");
			string temp1 = (gold % 100000000).ToString ();
			switch (temp1.Length) {
			case 7:
				strBlider1.Append ("0" + (int.Parse (temp1) / 1000000).ToString () + "y");
				break;
			case 8:
				strBlider1.Append ((int.Parse (temp1) / 1000000).ToString () + "y");
				break;
			default:
				strBlider1.Append ("00y");
				break;
			}
			return strBlider1.ToString ();
		}
	}


	/// <summary>  
	/// 字符串转为UniCode码字符串  
	/// </summary>  
	/// <param name="s"></param>  
	/// <returns></returns>  
	public static string StringToUnicode (string s)
	{
		char[] charbuffers = s.ToCharArray ();
		byte[] buffer;
		StringBuilder sb = new StringBuilder ();
		for (int i = 0; i < charbuffers.Length; i++) {
			buffer = System.Text.Encoding.Unicode.GetBytes (charbuffers [i].ToString ());
			sb.Append (string.Format ("\\u{0:X2}{1:X2}", buffer [1], buffer [0]));
		}
		return sb.ToString ();
	}

	/// <summary>  
	/// Unicode字符串转为正常字符串  
	/// </summary>  
	/// <param name="srcText"></param>  
	/// <returns></returns>  
	public static string UnicodeToString (string srcText)
	{
		string dst = "";
		string src = srcText;
		int len = srcText.Length / 6;
		for (int i = 0; i <= len - 1; i++) {
			string str = "";
			str = src.Substring (0, 6).Substring (2);
			src = src.Substring (6);
			byte[] bytes = new byte[2];
			bytes [1] = byte.Parse (int.Parse (str.Substring (0, 2), System.Globalization.NumberStyles.HexNumber).ToString ());
			bytes [0] = byte.Parse (int.Parse (str.Substring (2, 2), System.Globalization.NumberStyles.HexNumber).ToString ());
			dst += Encoding.Unicode.GetString (bytes);
		}
		return dst;
	}

	public static string GB2312ToUTF8 (string srcText)
	{
		Encoding utf8 = Encoding.GetEncoding ("UTF-8");
		Encoding gb2312 = Encoding.GetEncoding ("GB2312");
		byte[] gb = gb2312.GetBytes (srcText);
		gb = Encoding.Convert (gb2312, utf8, gb);
		return utf8.GetString (gb);
	}

	public static string UTF8ToGB2312 (string srcText)
	{
		byte[] bs = Encoding.GetEncoding ("UTF-8").GetBytes (srcText);
		bs = Encoding.Convert (Encoding.GetEncoding ("UTF-8"), Encoding.GetEncoding ("GB2312"), bs);
		return Encoding.GetEncoding ("GB2312").GetString (bs);
	}
	//生成unix格式时间
	public static long GetUnixTimestamp ()
	{
		try {
			TimeSpan timespan = DateTime.UtcNow - new DateTime (1970, 1, 1);
			return (long)timespan.TotalSeconds;//获取10位
		} catch (Exception) {
			return -1;
		}
	}
}


