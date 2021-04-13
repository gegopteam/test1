/* author:KinSen
 * Date:2017.06.09
 */

using UnityEngine;
using System.Collections;
using DG.Tweening;
using AssemblyCSharp;

enum MoveSide
{
	Non,
	Left,
	Right,
	Up,
	Down,
}

/*
 *  2020/03/13 Joey 判斷用戶是否要跳轉場景
 *  2020/03/13 Joey 跳轉場景
 */

public class GradeField : MonoBehaviour
{
	//public RectTransform[] ptTransform = null; //被移动的对象
	public float scaleNormal = 1.0f;
	public float scaleMax = 1.5f;
	private Vector3[] mCordinate = new Vector3[ 3 ];
	private int[] siblingIndex = new int[3];
	//显示的层级
	private float changeTime = 0.5f;
	//滑动的时间

	Vector3 mMouseDown = new Vector3 ();

	//bool bMovingStart  = false;
	bool bMouseDown = false;

	GameObject mTipWindowClone;

    MyInfo nInfo;

	//
	public static bool unlockScene = false;
	public static int whichGame;
    void Start ()
	{
		//if(null!=ptTransform)
		//{
		//	if(3 == ptTransform.Length)
		//	{
		//              //mCordinate[0] = new Vector3(ptTransform[0].localPosition.x, ptTransform[0].localPosition.y, ptTransform[0].localPosition.z);
		//              //mCordinate[1] = new Vector3(ptTransform[1].localPosition.x, ptTransform[1].localPosition.y, ptTransform[1].localPosition.z);
		//              //mCordinate[2] = new Vector3(ptTransform[2].localPosition.x, ptTransform[2].localPosition.y, ptTransform[2].localPosition.z);
		//              //siblingIndex[0] = ptTransform[0].transform.GetSiblingIndex();
		//              //siblingIndex[1] = ptTransform[1].transform.GetSiblingIndex();
		//              //siblingIndex[2] = ptTransform[2].transform.GetSiblingIndex();
		//              //ptTransform[0].localScale = new Vector3(scaleNormal, scaleNormal, scaleNormal);
		//              //ptTransform[1].localScale = new Vector3(scaleMax, scaleMax, scaleNormal);
		//              //ptTransform[2].localScale = new Vector3(scaleNormal, scaleNormal, scaleNormal);
		ResetMask ();
        //  }
        //}

        nInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
		if (nInfo.cannonMultipleMax < 100) {
			MoveForward (MoveSide.Right, false);
		} else if (nInfo.cannonMultipleMax >= 300) {
			MoveForward (MoveSide.Left, false);
		}
		//AddClickHandle ( true );
	}

	void AddClickHandle (bool value)
	{
		//if (value) {
		//	EventTriggerListener.Get (ptTransform [0].gameObject).onClick = OnButtonClick;
		//	EventTriggerListener.Get (ptTransform [1].gameObject).onClick = OnButtonClick;
		//	EventTriggerListener.Get (ptTransform [2].gameObject).onClick = OnButtonClick;
		//} else {
		//	EventTriggerListener.Get (ptTransform [0].gameObject).onClick = null;
		//	EventTriggerListener.Get (ptTransform [1].gameObject).onClick = null;
		//	EventTriggerListener.Get (ptTransform [2].gameObject).onClick = null;
		//}
	}

	// Update is called once per frame
	void Update()
	{
        //2020/03/13 Joey 判斷用戶是否要跳轉場景
		if (unlockScene)
        {
			Debug.LogError("-------GradeField-------Update-------");
			ChangSence(whichGame);
			unlockScene = false;
		}
	}

	//2020/03/13 Joey 跳轉場景
	public void ChangSence(int s)
    {
		MyInfo nInfo = (MyInfo)Facade.GetFacade().data.Get(FacadeConfig.USERINFO_MODULE_ID);
		int nMaxMutiple = nInfo.cannonMultipleMax;

		switch (s)
        {
			case 1:
				Debug.LogError("-------ChangSence-------GradeField_1-------");
				UIHallObjects.GetInstance().PlayFieldGrade_1();
				nInfo.isShowNumner = 1;
				break;
			case 2:
				Debug.LogError("-------ChangSence-------GradeField_2-------");
				UIHallObjects.GetInstance().PlayFieldGrade_2();
				nInfo.isShowNumner = 2;
				break;
			case 3:
				Debug.LogError("-------ChangSence-------GradeField_3-------");
				UIHallObjects.GetInstance().PlayFieldGrade_3();
				nInfo.isShowNumner = 3;
				break;
			case 4:
				Debug.LogError("-------ChangSence-------GradeField_4-------");
				UIHallObjects.GetInstance().PlayFieldGrade_4();
				nInfo.isShowNumner = 4;
				break;
		}
	}

	public void OnButtonClick (GameObject nObject)
	{
        Debug.LogError("-------OnButtonClick-------");
        //if (nObject.Equals (ptTransform [2].gameObject)) {
        //	MoveForward (MoveSide.Left);
        //} else if (nObject.Equals (ptTransform [0].gameObject)) {
        //	MoveForward (MoveSide.Right);
        //} else if (nObject.Equals (ptTransform [1].gameObject)) {
        MyInfo nInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
		int nMaxMutiple = nInfo.cannonMultipleMax;
		if (nObject.name.Equals("GradeField_1")) {//新手海湾
			Debug.LogError("-------OnButtonClick-------GradeField_1-------");
			UIHallObjects.GetInstance().PlayFieldGrade_1();
			/*if (nMaxMutiple > 0 && nMaxMutiple <= CannonMultiple.NEWBIE) {
					
				} else {
					GameObject Window = Resources.Load ("Window/WindowTips")as GameObject;
					mTipWindowClone = Instantiate (Window);

					UITipClickHideManager ClickTip = mTipWindowClone.GetComponent<UITipClickHideManager> ();
					ClickTip.SetClickClose ();
					ClickTip.text.text = "炮台等级高于本房间限制，试试其他房间吧!";
					ClickTip.time.text = "";
					ClickTip.tipString.text = "";
				}*/
			nInfo.isShowNumner = 1;
		} else if (nObject.name.Equals("GradeField_2")) {//深海遗址
			Debug.LogError("-------OnButtonClick-------GradeField_2-------");
			UIHallObjects.GetInstance().PlayFieldGrade_2();
			nInfo.isShowNumner = 2;
		} else if (nObject.name.Equals("GradeField_3")) {//海神寶藏
			Debug.LogError("-------OnButtonClick-------GradeField_3-------");
			UIHallObjects.GetInstance().PlayFieldGrade_3();
			nInfo.isShowNumner = 3;
		} else if (nObject.name.Equals("GradeField_4")) {//奪金島
			Debug.LogError("-------OnButtonClick-------GradeField_4-------");
			UIHallObjects.GetInstance().PlayFieldGrade_4();
			nInfo.isShowNumner = 4;
		} else if (nObject.name.Equals("GradeField_5")) {
			Debug.LogError("-------OnButtonClick-------GradeField_5-------");
			UIHallObjects.GetInstance().PlayFieldGrade_5();
			nInfo.isShowNumner = 5;
		}
		else if (nObject.name.Equals("Button")) {
			UIHallObjects.GetInstance().PlayExperience();
			nInfo.isShowNumner = 6;
		}
		//}
	}

	public void openUserMsg()
    {
		GameObject Window1 = UnityEngine.Resources.Load("Window/WindowTipsThree") as UnityEngine.GameObject;
		GameObject WindowClone1 = UnityEngine.GameObject.Instantiate(Window1);
		UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask>();
		ClickTips1.tipText.text = "result = " + UIHallCore.Instance.testresult + " ; status = "+ UIHallCore.Instance.teststatus;
	}

	void OnGUI ()
	{
		if (mTipWindowClone != null && mTipWindowClone.activeSelf)
			return;
		OnMoveStart ();
	}

	void OnAnimateFinish ()
	{
		AddClickHandle (true);
		//	Debug.LogError( "[ recv ] invoke animate finish!!!!!!!" );
	}

	void OnMoveStart ()
	{
		//如果缓冲动画没有完成，那么不执行操作
		if (Input.GetMouseButtonDown (0) && bMouseDown == false) {
			mMouseDown = Input.mousePosition;
			bMouseDown = true;
			return;
		}

		//如果有鼠标点击事件，并且没有位移动画，那么根据阀值判断是否有
		if (bMouseDown) {
			float deltaX = Input.mousePosition.x - mMouseDown.x;

			float throhold = 5;

			if (deltaX >= throhold) {
				bMouseDown = false;
				MoveForward (MoveSide.Right);
			}
			if (deltaX <= -throhold) {
				bMouseDown = false;
				MoveForward (MoveSide.Left);
			}
		}

		if (Input.GetMouseButtonUp (0)) {
			bMouseDown = false;
			return;
		}
	}

	//设置显示的层级
	void SetSiblingIndex (MoveSide side)
	{
		//if(MoveSide.Left == side)
		//{
		//	ptTransform [0].transform.SetSiblingIndex(siblingIndex [0]);
		//	ptTransform [1].transform.SetSiblingIndex(siblingIndex [2]);
		//	ptTransform [2].transform.SetSiblingIndex(siblingIndex [1]);
		//}
		//else if(MoveSide.Right == side)
		//{
		//	ptTransform [0].transform.SetSiblingIndex(siblingIndex [1]);
		//	ptTransform [1].transform.SetSiblingIndex(siblingIndex [2]);
		//	ptTransform [2].transform.SetSiblingIndex(siblingIndex [0]);
		//}
	}

	void ResetMask ()
	{
		//ptTransform [0].FindChild ( "Mask" ).gameObject.SetActive(true);
		//ptTransform [1].FindChild ( "Mask" ).gameObject.SetActive(false);
		//ptTransform [2].FindChild ( "Mask" ).gameObject.SetActive(true);
	}

	void MoveForward (MoveSide nSide, bool bAinimate = true)
	{
		AddClickHandle (false);//移除事件监听，防止效果混乱
		SetSiblingIndex (nSide);
		if (nSide == MoveSide.Left) {
			//设置缩放效果
			//设置位移效果
			//if (bAinimate) {
			//	//ptTransform [ 1 ].transform.DOScale ( new Vector3(scaleNormal, scaleNormal, scaleNormal) , changeTime );
			//	//ptTransform [ 2 ].transform.DOScale ( new Vector3(scaleMax, scaleMax, scaleNormal) , changeTime );
			//	//ptTransform [0].transform.DOLocalMove (mCordinate [2], changeTime);
			//	//ptTransform [1].transform.DOLocalMove (mCordinate [0], changeTime);
			//	//ptTransform [2].transform.DOLocalMove (mCordinate [1], changeTime);
			//} else {
			//	ptTransform [1].transform.localScale = new Vector3 (scaleNormal, scaleNormal, scaleNormal);
			//	ptTransform [2].transform.localScale = new Vector3 (scaleMax, scaleMax, scaleNormal);
			//	ptTransform [0].transform.localPosition = mCordinate [2];
			//	ptTransform [1].transform.localPosition = mCordinate [0];
			//	ptTransform [2].transform.localPosition = mCordinate [1];
			//}
			//更新组建索引
			//RectTransform nTempTrans = ptTransform [0];
			//ptTransform [0] = ptTransform[1];
			//ptTransform [1] = ptTransform[2];
			//ptTransform [2] = nTempTrans;
		} else if (nSide == MoveSide.Right) {
			//设置缩放效果
			//设置位移效果
			//if (bAinimate) {
			//	ptTransform [ 1 ].transform.DOScale ( new Vector3(scaleNormal, scaleNormal, scaleNormal) , changeTime );
			//	ptTransform [ 0 ].transform.DOScale ( new Vector3(scaleMax, scaleMax, scaleNormal) , changeTime );
			//	ptTransform [0].transform.DOLocalMove (mCordinate [1], changeTime);
			//	ptTransform [1].transform.DOLocalMove (mCordinate [2], changeTime);
			//	ptTransform [2].transform.DOLocalMove (mCordinate [0], changeTime);
			//} else {
			//	ptTransform [1].transform.localScale = new Vector3 (scaleNormal, scaleNormal, scaleNormal);
			//	ptTransform [0].transform.localScale = new Vector3 (scaleMax, scaleMax, scaleNormal);
			//	ptTransform [0].transform.localPosition = mCordinate [1];
			//	ptTransform [1].transform.localPosition = mCordinate [2]; 
			//	ptTransform [2].transform.localPosition = mCordinate [0];
			//}
			//更新组建索引
			//RectTransform nTempTrans = ptTransform [0];
			//ptTransform [0] = ptTransform[2];
			//ptTransform [2] = ptTransform[1];
			//ptTransform [1] = nTempTrans;
		}
		ResetMask ();

		CancelInvoke ();
		Invoke ("OnAnimateFinish", changeTime);
	}
		
}
