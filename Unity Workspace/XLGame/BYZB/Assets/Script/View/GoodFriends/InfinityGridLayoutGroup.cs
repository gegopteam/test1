using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(GridLayoutGroup))]
[RequireComponent(typeof(ContentSizeFitter))]
public class InfinityGridLayoutGroup : MonoBehaviour 
{
	private GameObject grid ;

    [SerializeField]
    int minAmount = 0;

    RectTransform rectTransform;

    GridLayoutGroup gridLayoutGroup;
    ContentSizeFitter contentSizeFitter;

    ScrollRect scrollRect;

    List<RectTransform> children=new List<RectTransform>();

	Vector2 startPosition;
    int amount = 0;


    public delegate void UpdateChildrenCallbackDelegate(int index, Transform trans);
    public UpdateChildrenCallbackDelegate updateChildrenCallback = null;

	int realIndex = -1;

    bool hasInit = false;
    Vector2 gridLayoutSize;
    Vector2 gridLayoutPos;
    Dictionary<Transform, Vector2> childsAnchoredPosition = new Dictionary<Transform, Vector2>();
    Dictionary<Transform, int> childsSiblingIndex = new Dictionary<Transform, int>();


	// Use this for initialization
	void Start ()
    {
       
	}

//	void OnGUI()
//	{
//		Tool.ShowInGUI ();
//	}

	IEnumerator InitChildren()
	{
		yield return 0;
		//获取当前GridList的高度
		rectTransform = GetComponent<RectTransform>();

		gridLayoutGroup = GetComponent<GridLayoutGroup> ();
		gridLayoutGroup.enabled = false;
		contentSizeFitter = GetComponent<ContentSizeFitter> ();
		contentSizeFitter.enabled = false;

		gridLayoutPos = rectTransform.anchoredPosition;
		gridLayoutSize = rectTransform.sizeDelta;

		//注册ScrollRect滚动回调;
		scrollRect = transform.parent.GetComponent<ScrollRect>();
		scrollRect.onValueChanged.AddListener((data) => { ScrollCallback(data); });


	}

	void ScrollCallback(Vector2 data)
	{
		UpdateChildern ();
	}

	void UpdateChildern()
	{
		 //amount有多少个将rectTransform扩大多少倍并实例化多少个Grid
	     for (int i = transform.childCount; i < amount; i++) {
			rectTransform.sizeDelta += new Vector2 (0, gridLayoutGroup.cellSize.y + gridLayoutGroup.spacing.y);
			switch (UIGoodFriends.currentButtonName) {
			    case "游戏好友":
				    grid = Resources.Load ("GameFriendGrid/Grid")as GameObject;
				    break;
			case "申请好友":
				    grid = Resources.Load ("ApplyFriendGrid/Grid")as GameObject;
				    break;
				}
			GameObject gridClone = Instantiate (grid);
			gridClone.transform.SetParent (this.transform);
			//显示其中的用户信息
			//判断一下获取哪个列表，好友列表在线的在前，离线的在后
			GameFriendInfo info = gridClone.GetComponent<GameFriendInfo>();
			}

	}
		
    public void SetAmount(int count)
    {
        amount = count;
        StartCoroutine(InitChildren());
    }
}
