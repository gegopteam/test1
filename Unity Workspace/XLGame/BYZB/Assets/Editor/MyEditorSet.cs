using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;

public class MyEditorSet : Editor {

	[MenuItem("GameObject/UI/Image")]
	static void CreatImage()
	{
		if(Selection.activeTransform)
		{
			if(Selection.activeTransform.GetComponentInParent<Canvas>())
			{
				GameObject go = new GameObject("image",typeof(Image));
				go.GetComponent<Image>().raycastTarget = false;
				go.transform.SetParent(Selection.activeTransform);
				go.GetComponent<RectTransform> ().localScale = Vector3.one;
				Selection.activeGameObject=go;
			}
		}
	}
		
	[MenuItem("GameObject/UI/Text")]
	static void CreatText()
	{
		if(Selection.activeTransform)
		{
			if(Selection.activeTransform.GetComponentInParent<Canvas>())
			{
				GameObject go = new GameObject("Text",typeof(Text));
				go.GetComponent<Text>().raycastTarget = false;
				go.transform.SetParent(Selection.activeTransform);
				go.GetComponent<RectTransform> ().localScale = Vector3.one;
				Selection.activeGameObject=go;
			}
		}
	}
}
