using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using UnityEngine.UI;

public class UISetSailGift : MonoBehaviour , IUiMediator {
	//public delegate void SeeHandEffect();
	//public static event SeeHandEffect SeeHandEvent;

	public GameObject panel;
	public GameObject fristWindow;
	public GameObject secondWindow;
	public GameObject thridWindow;
	public GameObject fristButton;
	public GameObject effectBaoxiag;
	public GameObject effectQipao;
	public GameObject toolTween;
	public GameObject dayPanel;

	//[SerializeField]
	//private UIToolTween currentUI;
	//private bool isDes = false;
	[SerializeField]
	private Animator buttonAnimator;

	[SerializeField]
	private GameObject sailGift;
	[SerializeField]
	private Camera sailGiftCamera;
	[SerializeField]
	private Canvas mainCanvas;

	public Text txtGoldCount;

	void Awake()
	{
		//currentUI = toolTween.GetComponentInChildren<UIToolTween> ();
		toolTween.SetActive (false);
		sailGift = GameObject.FindGameObjectWithTag ("MainCamera");
		Debug.Log (sailGift.name);
		sailGiftCamera = sailGift.GetComponent<Camera> ();
		mainCanvas = transform.GetComponentInChildren<Canvas> ();
		mainCanvas.worldCamera = sailGiftCamera;
		Debug.Log (buttonAnimator.name);
		buttonAnimator.SetBool ("Open", false);
	}
	// Use this for initialization
	void Start () {
		//UIHallMsg.GetInstance().ToolEvent += Tool;
		fristButton.SetActive (true);
		panel.SetActive (false);
		effectQipao.SetActive (false);
		//Debug.LogError ("day"+UIHallMsg.GetInstance ().currentDay);
		MyInfo myInfo = (MyInfo) Facade.GetFacade().data.Get( FacadeConfig.USERINFO_MODULE_ID );
		if ( myInfo.sailDay  > 1) {
			Init ();
		}

		Facade.GetFacade ().ui.Add ( FacadeConfig.UI_START_GIFT_MODULE_ID , this );
	}

	public void OnRecvData( int nType , object nData )
	{
		
		dayPanel.SetActive(false);
		toolTween.SetActive(true);
		Invoke ("DoCloseWindow",1f);
	}

	public void OnInit()
	{
		
	}

	public void OnRelease()
	{
		
	}

	void Init()
	{
		fristButton.SetActive (false);
		effectBaoxiag.SetActive (false);
		panel.SetActive (true);
		//后面两天的礼包
		//Debug.Log("UIHallMsg.GetInstance ().currentDay"+UIHallMsg.GetInstance ().currentDay);
		MyInfo myInfo = (MyInfo) Facade.GetFacade().data.Get( FacadeConfig.USERINFO_MODULE_ID );
		switch ( myInfo.sailDay ) {
		case 2:
			fristWindow.SetActive (false);
			secondWindow.SetActive (true);
			thridWindow.SetActive (false);
			break;
		case 3:
			fristWindow.SetActive (false);
			secondWindow.SetActive (false);
			thridWindow.SetActive (true);
			break;
			//
			//Debug.Log ("UIToolTween"+currentUI.name);
		}
		Invoke ("DoBubble",3f);
	}

	void InitFristWindow()
	{
		fristWindow.SetActive (true);
		secondWindow.SetActive (false);
		thridWindow.SetActive (false);
	}

	public void ClickButtonGetGift()
	{
		buttonAnimator.SetBool ("Open", true);
		InitFristWindow();
		Invoke ("OpenGetWindow", 0.8f);
	}
		
	void OpenGetWindow()
	{
		fristButton.SetActive (false);
		effectBaoxiag.SetActive (false);
		panel.SetActive (true);
		//Debug.Log ("FristUIToolTween"+currentUI.name);
		Invoke ("DoBubble",0.35f);
	}

	void DoBubble()
	{
		effectQipao.SetActive (true);
	}

	public void ExitButton()
	{
		//发送领取礼包的消息
		//并且发送新手引导的图标事件
		//Debug.Log("发送领取礼包的消息");
		MyInfo myInfo = (MyInfo) Facade.GetFacade().data.Get( FacadeConfig.USERINFO_MODULE_ID );
		Facade.GetFacade ().message.gift.SendGetStartGiftRequest ( myInfo.sailDay );
	}

	void DoCloseWindow()
	{
		Destroy ( gameObject );

		//只有领取起航礼包第二天的，才显示起航礼包倒计时窗口
		MyInfo nInfo =(MyInfo) Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
		int nDayIndex = System.Math.Abs ( nInfo.sailDay );
		if ( nDayIndex == 2 ) {
			GameObject nWindow = StartGiftManager.OpenCountDownWindow ( nDayIndex + 1 );
			UITomorrow nScript = nWindow.GetComponentInChildren<UITomorrow> ();
			nScript.RegisterCloseCallBack ( ()=>{
				IUiMediator nMediatorHall= Facade.GetFacade ().ui.Get ( FacadeConfig.UI_HALL_MODULE_ID );
	        	if (nMediatorHall != null) 
				{
					((UIHallCore)nMediatorHall).ShowStartGiftBoxTip ();
	        	}
			} );
		}
	}

	void OnDestroy()
	{
		Facade.GetFacade ().ui.Remove ( FacadeConfig.UI_START_GIFT_MODULE_ID );
		//UIHallMsg.GetInstance().ToolEvent -= Tool;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
