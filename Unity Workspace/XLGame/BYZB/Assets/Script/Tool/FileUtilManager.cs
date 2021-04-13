using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileUtilManager
{
	/// <summary>
	/// 创建一个文本文件
	/// </summary>
	/// <param name="path">文件路径</param>
	/// <param name="filename">文件名称</param>
	/// <param name="info">文件信息</param>
	public static void CreateFile (string path, string filename, string info)
	{
		StreamWriter steamWrite;
		FileInfo finfo = new FileInfo (path + "//" + filename);
		if (!Directory.Exists (path))
			Directory.CreateDirectory (path);
		if (finfo.Exists)
			finfo.Delete ();
		steamWrite = finfo.CreateText ();
		steamWrite.WriteLine (info);
		steamWrite.Close ();
		steamWrite.Dispose ();
	}

	/// <summary>
	/// 读取文件
	/// </summary>
	/// <param name="path"></param>
	/// <param name="filename"></param>
	/// <returns></returns>
	public static string LoadFile (string path, string filename)
	{
		if (!IsExistsFile (path, filename))
			return null;
		StreamReader sr = File.OpenText (path + "//" + filename);
		ArrayList arr = new ArrayList ();
		while (true) {
			string line = sr.ReadLine ();
			if (line == null)
				break;
			arr.Add (line);
		}
		string str = "";
		foreach (string i in arr)
			str += i;
		sr.Close ();
		sr.Dispose ();
		return str;
	}

	/// <summary>
	/// 文件是否存在
	/// </summary>
	/// <param name="path"></param>
	/// <param name="filename"></param>
	/// <returns></returns>
	public static bool IsExistsFile (string path, string filename)
	{
		if (!Directory.Exists (path))
			return false;
		if (!File.Exists (path + "//" + filename))
			return false;
		return true;
	}

	public static void DeleteFile (string path, string filename)
	{
		File.Delete (path + "//" + filename);
	}
}
