using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ActivityDownLoad : MonoBehaviour
{
	Sprite placeholder;
	private static ActivityDownLoad _instance = null;

	public static ActivityDownLoad GetInstance ()
	{
		return Instance;
	}

	public static ActivityDownLoad Instance {
		get {
			if (_instance == null) {
				GameObject obj = new GameObject ("AsyncImageDownload");
				_instance = obj.AddComponent<ActivityDownLoad> ();
				DontDestroyOnLoad (obj);
				_instance.Init ();
			}
			return _instance;
		}
	}

	public bool Init ()
	{
		if (!Directory.Exists (Application.persistentDataPath + "/ImageCache/")) {
			//创建文件夹
			Directory.CreateDirectory (Application.persistentDataPath + "/ImageCache/");
		}
		if (placeholder == null) {
			placeholder = Resources.Load ("placeholder") as Sprite;
		}
		return true;

	}

	public void SetAsyncImage (string url)
	{
		//开始下载图片前，将UITexture的主图片设置为占位图
		//image.sprite = placeholder;
		//判断是否是第一次加载这张图片
		if (!File.Exists (path + url.GetHashCode ())) {
			//如果之前不存在缓存文件
			StartCoroutine (DownloadImage (url));
		} else {
			LoadLocalImage (url);
		}
	}

	public void SetAsyncImage (string url, Image image)
	{
		//开始下载图片前，将UITexture的主图片设置为占位图
		//image.sprite = placeholder;
		//判断是否是第一次加载这张图片
		if (!File.Exists (path + url.GetHashCode ())) {
			//如果之前不存在缓存文件
			StartCoroutine (DownloadImage (url, image));
		} else {
			LoadLocalImage (url, image);
		}
	}

	IEnumerator DownloadImage (string url, Image image)
	{
		if (!string.IsNullOrEmpty (url)) {
			Debug.Log ("downloading new image:" + path + url.GetHashCode ());//url转换HD5作为名字
			WWW www = new WWW (url);
			yield return www;

			Texture2D tex2d = www.texture;
			//将图片保存至缓存路径
			byte[] pngData = tex2d.EncodeToPNG ();
			File.WriteAllBytes (path + url.GetHashCode (), pngData);

			Sprite m_sprite = Sprite.Create (tex2d, new Rect (0, 0, tex2d.width, tex2d.height), new Vector2 (0, 0));
			if (image == null)
				yield return null;
			else
				image.sprite = m_sprite;
		}
	}

	IEnumerator DownloadImage (string url)
	{
		if (!string.IsNullOrEmpty (url)) {
			Debug.Log ("downloading new image:" + path + url.GetHashCode ());//url转换HD5作为名字
			WWW www = new WWW (url);
			Debug.LogError ("DownloadImage url = " + url);
			if (url == null) {
				yield return null;
			}
			yield return www;

			Texture2D tex2d = www.texture;
			//将图片保存至缓存路径
			byte[] pngData = tex2d.EncodeToPNG ();
			File.WriteAllBytes (path + url.GetHashCode (), pngData);	
		}
	}

	void  LoadLocalImage (string url, Image image)
	{
		if (!string.IsNullOrEmpty (url)) {
			//"file:///" +
			string filePath = path + url.GetHashCode ();
//		Debug.Log ("filePath = " + filePath);
			FileStream fileStream = new FileStream (filePath, FileMode.Open, FileAccess.Read);
			fileStream.Seek (0, SeekOrigin.Begin);
			byte[] byteTemp = new byte[fileStream.Length];
			fileStream.Read (byteTemp, 0, (int)fileStream.Length);
			fileStream.Close ();
			fileStream.Dispose ();
			fileStream = null;
			//Debug.Log ("getting local image:" + filePath);
//		WWW www = new WWW (filePath);
//		yield return www;
			Texture2D texture = new Texture2D (925, 576);
			texture.LoadImage (byteTemp);
			Sprite m_sprite = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2 (0, 0));
			if (image == null)
				return;
			else
				image.sprite = m_sprite;
		}
	}

	void  LoadLocalImage (string url)
	{
		if (!string.IsNullOrEmpty (url)) {
			//"file:///" +
			string filePath = path + url.GetHashCode ();
			//		Debug.Log ("filePath = " + filePath);
			FileStream fileStream = new FileStream (filePath, FileMode.Open, FileAccess.Read);
			fileStream.Seek (0, SeekOrigin.Begin);
			byte[] byteTemp = new byte[fileStream.Length];
			fileStream.Read (byteTemp, 0, (int)fileStream.Length);
			fileStream.Close ();
			fileStream.Dispose ();
			fileStream = null;
			//Debug.Log ("getting local image:" + filePath);
			//		WWW www = new WWW (filePath);
			//		yield return www;
			Texture2D texture = new Texture2D (925, 576);
			texture.LoadImage (byteTemp);
		}
	}

	public string path {
		get {
			//pc,ios //android :jar:file//
			return Application.persistentDataPath + "/ImageCache/";
		}
	}
}
