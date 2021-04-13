using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ResetStore: MonoBehaviour {
	public delegate void ScrollIndex (float index);
	public static event ScrollIndex ScrollIndexEvent;

	RectTransform rectTransform;

	GridLayoutGroup gridLayoutGroup;
	ContentSizeFitter contentSizeFitter;

	Vector2 gridLayoutSize;
	Vector2 gridLayoutPos;

	bool hasInit = false;

	Dictionary<Transform, Vector2> childsAnchoredPosition = new Dictionary<Transform, Vector2>();
	Dictionary<Transform, int> childsSiblingIndex = new Dictionary<Transform, int>();


	// Use this for initialization
	void Start () {

	}
		
	IEnumerator InitChildren()
	{
		yield return 0;
		if (ScrollIndexEvent != null) {
		
			ScrollIndexEvent (0);
		}
		if (!hasInit)
		{
			//获取Grid的宽度;
			rectTransform = GetComponent<RectTransform>();

			//			gridLayoutGroup = GetComponent<GridLayoutGroup>();
			//			gridLayoutGroup.enabled = false;
			//			contentSizeFitter = GetComponent<ContentSizeFitter>();
			//			contentSizeFitter.enabled = false;

			gridLayoutPos = rectTransform.anchoredPosition;
			gridLayoutSize = rectTransform.sizeDelta;

			//获取所有child anchoredPosition 以及 SiblingIndex;
			for (int index = 0; index < transform.childCount; index++)
			{
				Transform child=transform.GetChild(index);
				RectTransform childRectTrans= child.GetComponent<RectTransform>();
				childsAnchoredPosition.Add(child, childRectTrans.anchoredPosition);

				childsSiblingIndex.Add(child, child.GetSiblingIndex());
			}
		} else
		{
			rectTransform.anchoredPosition = gridLayoutPos;
			rectTransform.sizeDelta = gridLayoutSize;

			//children重新设置上下顺序;
			foreach (var info in childsSiblingIndex)
			{
				info.Key.SetSiblingIndex(info.Value);
			}

			//children重新设置anchoredPosition;
			for (int index = 0; index < transform.childCount; index++)
			{
				Transform child = transform.GetChild(index);

				RectTransform childRectTrans = child.GetComponent<RectTransform>();
				if (childsAnchoredPosition.ContainsKey(child))
				{
					childRectTrans.anchoredPosition = childsAnchoredPosition[child];
				}
			}
		}
		hasInit = true;
	}

	// Update is called once per frame
	void Update () {

	}

	public void SetAmount()
	{
		StartCoroutine(InitChildren());
	}
}
