using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 可拖拽区域控制器
/// </summary>
public class ScrollableAreaController : MonoBehaviour
{
	//排列类型 默认为Default
	public enum AdapationType
	{
		Default,
		ModifyColumns,
		Scale
	}

	[SerializeField]
	private ScrollableCell cellPrefab;
	//cell预制体
	[SerializeField]
	private int NUMBER_OF_COLUMS;
	//排数
	[SerializeField]
	private RectTransform canvasRec;
	//幕布Rect

	public float cellWidth = 30.0f;
	//cell宽
	public float cellHeight = 25.0f;
	//cell高
	public Vector2 cellOffset;
	//首个Cell偏移量
	public AdapationType adapationType;
	//排列类型

	private RectTransform content;
	private int visibleCellsTotalCount = 0;
	//可见Cell总数
	private int visibleCellsRowCount = 0;
	//屏幕中可见Cell行数
	private LinkedList<GameObject> localCellsPool = new LinkedList<GameObject> ();
	//cell池
	private LinkedList<GameObject> cellsInUse = new LinkedList<GameObject> ();
	//使用的Cell链表
	private ScrollRect rect;
	//显示cell的ScrollRect

	private IList allCellsData;
	//所有Cell的数据
	private int previousInitialIndex = 0;
	//之前的初始化索引
	private int initialIndex = 0;
	//初始化索引
	private float initposition = 0;
	//初始位置
	private float adjustSize;
	//调整尺寸

	private Vector3 contentPosition;
	//content坐标
	private CanvasScaler canvasScaler;
	//幕布上的Scaler组件

	private Vector3 firstCellPosition;
	//第一个cell的位置
	public Vector3 FirstCellPosition {//获取第一个cell的位置
		get
        { return new Vector3 (cellOffset.x, cellOffset.y, 0) * adaptationScale; }
	}

	private float adaptationScale = 1.0f;
	//缩放比率

	//是否可以水平滑动
	public bool horizontal {
		get { return rect.horizontal; }
	}

	void Awake ()
	{
		//初始化
		canvasScaler = canvasRec.gameObject.GetComponent<CanvasScaler> ();
		rect = this.GetComponent<ScrollRect> ();
		content = rect.content;
		if (horizontal) {//水平排列
			//求一行显示多少个 CeilToInt向上取整 
			visibleCellsRowCount = Mathf.CeilToInt (rect.viewport.GetComponent<RectTransform> ().rect.width / cellWidth);
			ChangeModel ();//修改模式
		} else {//竖直排列
			//求一列显示多少个
			visibleCellsRowCount = Mathf.CeilToInt (rect.viewport.GetComponent<RectTransform> ().rect.height / cellHeight);
			ChangeModel ();
		}
		//求生成cell总数 一排显示cell的数量为屏幕能够显示的数量+1，多的一个是用来实现无线滚动的
		visibleCellsTotalCount = visibleCellsRowCount + 1;
		//上面求的是一排的cell数，求总数还要乘以排数
		visibleCellsTotalCount *= NUMBER_OF_COLUMS;
		//记录content的localPosition
		contentPosition = content.localPosition;
		//生成Cell
		this.CreateCellPool ();
	}
	//更新
	void Update ()
	{
		if (allCellsData == null)
			return;
		previousInitialIndex = initialIndex;//记录之前的索引
		CalculateCurrentIndex ();//计算当前索引（当前索引代表在显示区域最上边的cell的索引） 通过initialIndex进行存储
		InternalCellsUpdate ();//单元格更新
	}
	//计算当前索引
	private void CalculateCurrentIndex ()
	{
		if (!horizontal) {
			//竖直排列 当前索引计算方式：竖直方向即为沿y轴方向滑动，initposition为content初始y
			//轴坐标，用当前content的y轴坐标减去初始y轴坐标为滑动的距离，再对cell高度求商，即为当前索引
			initialIndex = Mathf.FloorToInt ((content.localPosition.y - initposition) / cellHeight);
            
		} else {//水平排列  与竖直方向相同，只不过改成x轴
			initialIndex = (int)((content.localPosition.x - initposition) / cellWidth);
			initialIndex = Mathf.Abs (initialIndex);
		}
		//对索引做出限制：显示的cell数量对排数求商减一列最多显示数量
		//举例：当前16个cell，屏幕最多显示4个那么索引最多就是12，因为12索引以后的cell无法到达content的上方，会弹下去
		int limit = Mathf.CeilToInt ((float)allCellsData.Count / (float)NUMBER_OF_COLUMS) - visibleCellsRowCount;//这个是索引的最大值。
		if (initialIndex < 0)
			initialIndex = 0;
		if (initialIndex >= limit)
			initialIndex = limit - 1;
	}
	//单元格更新
	private void InternalCellsUpdate ()
	{
		//索引发生改变
		if (previousInitialIndex != initialIndex) {
			//该bool是用来表示索引变大还是变小，true为变大，false为变小 其实就是表示拖拽的方向 
			bool scrollingPositive = previousInitialIndex < initialIndex;
			//索引改变的值
			int indexDelta = Mathf.Abs (previousInitialIndex - initialIndex);
			//根据拖拽方向来确定符号
			int deltaSign = scrollingPositive ? +1 : -1;
			//根据索引的改变要对超出范围的cell做出更新处理
			for (int i = 1; i <= indexDelta; i++)
				this.UpdateContent (previousInitialIndex + i * deltaSign, scrollingPositive);
		}
	}

	/// <summary>
	/// 更新content
	/// </summary>
	/// <param name="cellIndex">cell的索引</param>
	/// <param name="scrollingPositive">拖拽方向</param>
	private void UpdateContent (int cellIndex, bool scrollingPositive)
	{
		//计算需要处理的cell索引：scrollingPositive为true则为向上滑动  
		int index = scrollingPositive ? ((cellIndex - 1) * NUMBER_OF_COLUMS) + (visibleCellsTotalCount) : (cellIndex * NUMBER_OF_COLUMS);
		LinkedListNode<GameObject> tempCell = null;

		int currentDataIndex = 0;
		//遍历对每排进行处理
		for (int i = 0; i < NUMBER_OF_COLUMS; i++) {
			this.FreeCell (scrollingPositive);//重置cell改变链表顺序
			tempCell = GetCellFromPool (scrollingPositive);//从池中取出一个cell
			currentDataIndex = index + i;//当前需要赋值的索引

			PositionCell (tempCell.Value, index + i);//重置position，排列格子
			ScrollableCell scrollableCell = tempCell.Value.GetComponent<ScrollableCell> ();
			//更新数据
			if (currentDataIndex >= 0 && currentDataIndex < allCellsData.Count) {
				scrollableCell.Init (this, allCellsData [currentDataIndex], currentDataIndex);
			} else
				scrollableCell.Init (this, null, currentDataIndex);
			scrollableCell.ConfigureCell ();
		}
	}
	//重置cell
	private void FreeCell (bool scrollingPositive)
	{
		LinkedListNode<GameObject> cell = null;
		// 根据滑动方向把不可见的cell放置到末尾
		// 若向上滑动则将当前index前面的放在末尾
		// 若向下滑动则将当前index后面的放在前面
		if (scrollingPositive) {
			//向上滑动，将第一个放在末尾
			cell = cellsInUse.First;
			cellsInUse.RemoveFirst ();
			localCellsPool.AddLast (cell);
		} else {
			//向下滑动，将最后一个放在头前
			cell = cellsInUse.Last;
			cellsInUse.RemoveLast ();
			localCellsPool.AddFirst (cell);
		}
	}

	//修改模式
	//修改模式这里三个模式目前没有任何分别，可以进行拓展
	void ChangeModel ()
	{
		switch (adapationType) {
		case AdapationType.ModifyColumns:
			break;
		case AdapationType.Scale:
			break;
		default:
			break;
		}
	}
	//生成Cell池
	private void CreateCellPool ()
	{
		GameObject tmpCell = null;
		//根据总数生成cell
		for (int i = 0; i < visibleCellsTotalCount; i++) {
			tmpCell = InstantiateCell ();//生成
			localCellsPool.AddLast (tmpCell);//存储在链表中
		}
		content.gameObject.SetActive (false);//禁用掉content
	}
	//生成cell 并初始化
	private GameObject InstantiateCell ()
	{
		GameObject cellTempObject = Instantiate (cellPrefab.gameObject) as GameObject;
		cellTempObject.layer = this.gameObject.layer;//设置层为UI层
		cellTempObject.transform.SetParent (content.transform, false);//设置父物体
		cellTempObject.transform.localScale = cellPrefab.transform.localScale * adaptationScale;//设置缩放
		cellTempObject.transform.localPosition = cellPrefab.transform.localPosition;//设置localPosition
		cellTempObject.transform.localRotation = cellPrefab.transform.localRotation;//设置localRotation
		cellTempObject.SetActive (false);//默认不显示
		return cellTempObject;
	}

	private LinkedListNode<GameObject> GetCellFromPool (bool scrollingPositive)
	{
		//池子里没有返回null
		if (localCellsPool.Count == 0)
			return null;
		//从头结点取出一个元素
		LinkedListNode<GameObject> cell = localCellsPool.First;
		localCellsPool.RemoveFirst ();

		if (scrollingPositive)//放使用链表的末尾
            cellsInUse.AddLast (cell);
		else//放使用链表的前面
            cellsInUse.AddFirst (cell);
		return cell;
	}
	//设置content大小
	private void SetContentSize ()
	{
		//一行显示的cell数量
		int cellOneWayCount = Mathf.CeilToInt ((float)allCellsData.Count / NUMBER_OF_COLUMS);
		//初始化ContentSize
		if (horizontal)
			content.sizeDelta = new Vector2 (cellOneWayCount * cellWidth, content.sizeDelta.y);
		else
			content.sizeDelta = new Vector2 (content.sizeDelta.x, cellOneWayCount * cellHeight);
	}
	//对cell进行排列
	private void PositionCell (GameObject go, int index)
	{
		int rowMod = index % NUMBER_OF_COLUMS;
		if (horizontal)//水平排列   
            go.transform.localPosition = firstCellPosition + new Vector3 ((index / NUMBER_OF_COLUMS) * cellWidth, -cellHeight * (rowMod), 0);
		else//竖直排列 x根据排数以当前索引对排数求余即平移一个cellWidth 高度即为当前索引对排数求商乘cellHeight
            go.transform.localPosition = firstCellPosition + new Vector3 (cellWidth * (rowMod), -(index / NUMBER_OF_COLUMS) * cellHeight, 0);
	}
	//这里是将初始的位置进行定位
	public void SetCellposition (int index)
	{
		float y = cellHeight * index + initposition;
		content.localPosition = new Vector3 (content.localPosition.x, y, content.localPosition.z);
	}
	//初始化cell数据 并生成cell
	public void InitializeWithData (IList cellDataList, int index = 0)
	{
		if (cellsInUse.Count > 0) {//若有在使用的cell 释放掉
			foreach (GameObject cell in cellsInUse) {
				localCellsPool.AddLast (cell);
			}
			cellsInUse.Clear ();
		} else {//若没有则记录初始坐标
			if (horizontal)//水平排列  初始位置为content的x坐标  因为水平排列即x轴方向排列
                initposition = content.localPosition.x;
			else//竖直排列  沿y轴进行排列 初始位置为content的y坐标
                initposition = content.localPosition.y;
		}
		//初始化
		previousInitialIndex = 0;
		initialIndex = 0;
		content.gameObject.SetActive (true);
		LinkedListNode<GameObject> tempCell = null;
		allCellsData = cellDataList;

		SetContentSize ();//设置content大小
		firstCellPosition = FirstCellPosition;//初始化第一个Cell的位置

		int currentDataIndex = 0;//当前数据索引
		for (int i = 0; i < visibleCellsTotalCount; i++) {
			tempCell = GetCellFromPool (true);//获取一个cell
			if (tempCell == null || tempCell.Value == null)
				continue;
			//为当前索引赋值
			currentDataIndex = i + initialIndex * NUMBER_OF_COLUMS;
			//根据索引对当前cell进行排列
			PositionCell (tempCell.Value, currentDataIndex);
			//显示
			tempCell.Value.SetActive (true);
			//获取cell身上的ScrollableCell   这个是父类，子类是预制体身上挂的backitem  其实拿的也是backitem
			ScrollableCell scrollableCell = tempCell.Value.GetComponent<ScrollableCell> ();
			if (currentDataIndex < cellDataList.Count)//数据初始化   吧backageUI里面list存的东西放到scrollablecell里
                scrollableCell.Init (this, cellDataList [i], currentDataIndex);
			else//超出cellDataList说明没有数据
                scrollableCell.Init (this, null, currentDataIndex);
			//cell对UI控件进行数据初始化赋值
			scrollableCell.ConfigureCellData ();//backitem重写的方法，显示数据。
		}
		SetCellposition (index);

	}

}
