using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
/// <summary>
/// Infinity grid layout group room.每次都将scrollBar初始到原先的位置
/// </summary>

[RequireComponent(typeof(GridLayoutGroup))]
[RequireComponent(typeof(ContentSizeFitter))]
public class InfinityGridLayoutGroupRoom : MonoBehaviour 
{

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

        if (!hasInit)
        {
            //获取Grid的宽度;
            rectTransform = GetComponent<RectTransform>();

            gridLayoutGroup = GetComponent<GridLayoutGroup>();
            gridLayoutGroup.enabled = false;
            contentSizeFitter = GetComponent<ContentSizeFitter>();
            contentSizeFitter.enabled = false;

            gridLayoutPos = rectTransform.anchoredPosition;
            gridLayoutSize = rectTransform.sizeDelta;

			//注册ScrollRect滚动回调;
			scrollRect = transform.parent.GetComponent<ScrollRect>();
			scrollRect.onValueChanged.AddListener((data) => { ScrollCallback(data); });

            //获取所有child anchoredPosition 以及 SiblingIndex;
            for (int index = 0; index < transform.childCount; index++)
            {
                Transform child=transform.GetChild(index);
                RectTransform childRectTrans= child.GetComponent<RectTransform>();
                childsAnchoredPosition.Add(child, childRectTrans.anchoredPosition);

                childsSiblingIndex.Add(child, child.GetSiblingIndex());
            }
        }
        else
        {
			Tool.Log ("InitScrollRect刷新房间的信息", true);

            rectTransform.anchoredPosition = gridLayoutPos;
            rectTransform.sizeDelta = gridLayoutSize;

            children.Clear();

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
                else
                {
                    Debug.LogError("childsAnchoredPosition no contain "+child.name);
                }
            }
        }
        hasInit = true;
		//如果需要显示的个数小于设定的个数;
		for (int index = 0; index < minAmount; index++)
		{
			children[index].gameObject.SetActive(index < amount);
		}

		if (gridLayoutGroup.constraint == GridLayoutGroup.Constraint.FixedColumnCount)
		{
			//如果小了一行，则需要把GridLayout的高度减去一行的高度;
			int row = (minAmount - amount) / gridLayoutGroup.constraintCount;
			if (row > 0)
			{
				rectTransform.sizeDelta -= new Vector2(0, (gridLayoutGroup.cellSize.y + gridLayoutGroup.spacing.y) * row);
			}
		}
		else
		{
			//如果小了一列，则需要把GridLayout的宽度减去一列的宽度;
			int column = (minAmount - amount) / gridLayoutGroup.constraintCount;
			if (column > 0)
			{
				rectTransform.sizeDelta -= new Vector2((gridLayoutGroup.cellSize.x + gridLayoutGroup.spacing.x) * column, 0);
			}
		}
	}
		
	void Update () 
    {
	
	}

	void ScrollCallback(Vector2 data)
	{
		UpdateChildren();
	}

	void UpdateChildren()
	{
		if (transform.childCount < minAmount)
		{
			return;
		}

		Vector2 currentPos = rectTransform.anchoredPosition;

		if (gridLayoutGroup.constraint == GridLayoutGroup.Constraint.FixedColumnCount)
		{
			float offsetY = currentPos.y - startPosition.y;

			if (offsetY > 0)
			{
				//向上拉，向下扩展;
				{
					if (realIndex >= amount - 1)
					{
						startPosition = currentPos;
						return;
					}

					float scrollRectUp = scrollRect.transform.TransformPoint(Vector3.zero).y;

					Vector3 childBottomLeft = new Vector3(children[0].anchoredPosition.x, children[0].anchoredPosition.y - gridLayoutGroup.cellSize.y, 0f);
					float childBottom = transform.TransformPoint(childBottomLeft).y;

					if (childBottom >= scrollRectUp)
					{

						//移动到底部;
						for (int index = 0; index < gridLayoutGroup.constraintCount; index++)
						{
							children[index].SetAsLastSibling();

							children[index].anchoredPosition = new Vector2(children[index].anchoredPosition.x, children[children.Count - 1].anchoredPosition.y - gridLayoutGroup.cellSize.y - gridLayoutGroup.spacing.y);

							realIndex++;

							if (realIndex > amount - 1)
							{
								children[index].gameObject.SetActive(false);
							}
							else
							{
								UpdateChildrenCallback(realIndex, children[index]);
							}
						}

						//GridLayoutGroup 底部加长;
						rectTransform.sizeDelta += new Vector2(0, gridLayoutGroup.cellSize.y + gridLayoutGroup.spacing.y);

						//更新child;
						for (int index = 0; index < children.Count; index++)
						{
							children[index] = transform.GetChild(index).GetComponent<RectTransform>();
						}
					}
				}
			}
			else
			{
				//向下拉，下面收缩;
				if (realIndex + 1 <= children.Count)
				{
					startPosition = currentPos;
					return;
				}
				RectTransform scrollRectTransform = scrollRect.GetComponent<RectTransform>();
				Vector3 scrollRectAnchorBottom = new Vector3(0, -scrollRectTransform.rect.height - gridLayoutGroup.spacing.y, 0f);
				float scrollRectBottom = scrollRect.transform.TransformPoint(scrollRectAnchorBottom).y;

				Vector3 childUpLeft = new Vector3(children[children.Count - 1].anchoredPosition.x, children[children.Count - 1].anchoredPosition.y, 0f);

				float childUp = transform.TransformPoint(childUpLeft).y;

				if (childUp < scrollRectBottom)
				{
					//Debug.Log("childUp < scrollRectBottom");

					//把底部的一行 移动到顶部
					for (int index = 0; index < gridLayoutGroup.constraintCount; index++)
					{
						children[children.Count - 1 - index].SetAsFirstSibling();

						children[children.Count - 1 - index].anchoredPosition = new Vector2(children[children.Count - 1 - index].anchoredPosition.x, children[0].anchoredPosition.y + gridLayoutGroup.cellSize.y + gridLayoutGroup.spacing.y);

						children[children.Count - 1 - index].gameObject.SetActive(true);

						UpdateChildrenCallback(realIndex - children.Count - index, children[children.Count - 1 - index]);
					}

					realIndex -= gridLayoutGroup.constraintCount;

					//GridLayoutGroup 底部缩短;
					rectTransform.sizeDelta -= new Vector2(0, gridLayoutGroup.cellSize.y + gridLayoutGroup.spacing.y);

					//更新child;
					for (int index = 0; index < children.Count; index++)
					{
						children[index] = transform.GetChild(index).GetComponent<RectTransform>();
					}
				}
			}
		}

		startPosition = currentPos;
	}

    void UpdateChildrenCallback(int index,Transform trans)
    {
        if (updateChildrenCallback != null)
        {
            updateChildrenCallback(index, trans);
        }
    }
    public void SetAmount(int count)
    {
        amount = count;

        StartCoroutine(InitChildren());
    }
}
