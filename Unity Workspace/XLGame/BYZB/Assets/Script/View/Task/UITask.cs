using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using AssemblyCSharp;

public class UITask : MonoBehaviour,IUiMediator {

	public const int ACTIVITY = 1;
	public const int TASK = 0;
	public Text activeText;
	public delegate void SendRequest(int Id);
	public static event SendRequest SendRequestEvent;
	public static UITask instance;
	private AppControl appControl = null;
	private GameObject sliderGo;
	public BackageUI scroller;
	public Slider wholeSliders;
	public static int count = 1;//活跃度的值，为10的时候返回1
	public int wholeCount = 1;//返回当前slider的Index；
	public Transform effectLight;
	private readonly float MAX_ACTIVE=120;
	[SerializeField]
	private GameObject task;
	[SerializeField]
	private Camera taskCamera;
	[SerializeField]
	private Canvas mainCanvas;
	public Animator ani;
	private GameObject WindowClone;
	private FiEverydayActivityAwardResponse recActive;
	private UIReward reward;
	public static int Id;
	public Button reciveBtn;
	public SliderInfo CompleteTask;

	public static List< FiEverydayTaskDetial > recList;
	//其他的进度条都使用public拖拽赋值，再后来可以改变其值

	void Awake()
	{
		gameObject.GetComponent<Canvas>().worldCamera=GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera>();
		instance = this;
		if(wholeSliders==null)
			wholeSliders=this.transform.Find("Liveness").Find("Bg").GetComponent<Slider>();
		UIHall.ResetScrollEvent += ResetTaskScroll;
		appControl = AppControl.GetInstance ();

	}
	void ResetTaskScroll()
	{
		task = GameObject.FindGameObjectWithTag ("MainCamera");
		Debug.LogError (task.name);
		taskCamera = task.GetComponent<Camera> ();
		mainCanvas = transform.GetComponentInChildren<Canvas> ();
		mainCanvas.worldCamera = taskCamera;
	}

	void Start()
	{
		scroller=this.transform.GetComponentInChildren<BackageUI>();
		Facade.GetFacade().message.task.ChangeSliderValueEvent += ReceiveButton;
		Facade.GetFacade ().ui.Add ( FacadeConfig.TASK_MODULE_ID ,this );
		UIColseManage.instance.ShowUI (this.gameObject);
	}
	//玩家点击了领取按钮调用的函数，发送给服务器对应id的申请。
	public static void AlreadyFinsh(int id)
	{
		Debug.LogError ("我发消息了啊，我要领奖励"+id);
		if (SendRequestEvent != null) {
			SendRequestEvent (id);
		}
	}

	// Update is called once per frame
	void Update () {
		//wholeSliders.value += 0.001f;
	}

	public void OnButton()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.effect_closePanel);
		ani.SetBool ("isClose", true);
		transform.GetChild (0).GetChild(0).gameObject.SetActive (false);
		Invoke ("destroyGameObject", 0.3f);
	}
	void destroyGameObject(){
		UIColseManage.instance.CloseUI ();
	}
	//接到服务器的回调，参数是服务器发过来的活跃度增加量
	void ReceiveButton(int active)
	{
		Debug.LogError ("接到了服务器增加活跃度的回调，增加的活跃度为："+active);
		wholeSliders.value += active / MAX_ACTIVE;
	}
	int GetMaxValue(int taskid)
    {
        if (taskid < 9 && taskid > 0)
        {
            return TaskControl.getTaskInfo(taskid).maxValue;
        }
        else
        {
            return -1;
        }
	}
	//一件领取
	public void OnekeyToReceive()
	{
		AudioManager._instance.PlayEffectClip(AudioManager.ui_click);
		List<FiEverydayTaskDetial> temp = TaskMsgHandle.GetList;
		List<int> taskList = new List<int> ();
		foreach (var task in temp) {
			if (task.progress >= GetMaxValue (task.taskId)&&GetMaxValue (task.taskId)!=-1) {
				taskList.Add (task.taskId);
			}
		}
		Facade.GetFacade ().message.task.SendEveryDayTaskRequest ( taskList );
	}
		
	public void OnInit(){
		

	}
	public void OnRelease()
	{
		
	}
	void BtnIsActive(){
		int now;
		float max;
		for(int i=0;i<UITask.recList.Count;i++){
			FiEverydayTaskDetial temp = UITask.recList [i];
			if (temp.progress == -1)
				continue;
			now = temp.progress;
			max = TaskControl.getTaskInfo (temp.taskId).maxValue;
			if (now / max >= 1) {
				reciveBtn.GetComponent<Image> ().sprite = UIHallTexturers.instans.Task[4];
				reciveBtn.gameObject.GetComponent<Button> ().interactable = true;
				return ;
			}
		}
		reciveBtn.GetComponent<Image> ().sprite = UIHallTexturers.instans.Task[5];
		reciveBtn.gameObject.GetComponent<Button> ().interactable = false;
	}
	void CloseEffet(){
		CompleteTask.effect.SetActive (false);
		scroller.cellNumber = recList.Count;	
		scroller.Refresh ();
		CompleteTask = null;
	}
		
	//接到服务器推送来的消息
	public void OnRecvData( int nType , object nData )
	{
		//关于任务奖励的的消息推送
		if (nType == TASK) 
        {
			wholeSliders.value = DataControl.GetInstance ().getTaskInfo ().activity / MAX_ACTIVE;
			recList = (List<FiEverydayTaskDetial>)nData;
			if (CompleteTask != null) {
				CompleteTask.effect.SetActive (true);
				Invoke ("CloseEffet", 1);
			} else {
				scroller.cellNumber = recList.Count;	
				scroller.Refresh ();
			}

			activeText.text = DataControl.GetInstance ().getTaskInfo ().activity.ToString ();

			 BtnIsActive ();
		} else if (nType == ACTIVITY )  { //活跃度奖励的消息推送
			recActive=(FiEverydayActivityAwardResponse)nData;
			if (recActive.result == 0) {
				WindowClone = UnityEngine.GameObject.Instantiate (Resources.Load ("Window/RewardWindow")as GameObject);
				reward = WindowClone.GetComponent<UIReward> ();
				reward.SetRewardData ( recActive.property );
				CompleteActive (recActive.activity);
				reward.SetRewardData (recActive.property);
				Debug.LogError ("得到属性的列表长度为：" + recActive.property.Count);
				foreach (var temp in recActive.property) {
					Debug.LogError (temp.ToString ());
				}
			}

		}

	}
	//领取过活跃度宝箱时调用的函数
	public void CompleteActive(int active){
		GameObject tempGameobject;
		switch (active) {
		case 20:
			tempGameobject = WholeProgress.instance.twee.gameObject;
			tempGameobject.GetComponent<Image> ().sprite = UIHallTexturers.instans.Task [0];
			Destroy (WholeProgress.instance.twee);
			tempGameobject.transform.localScale = Vector3.one * 0.57f;
			tempGameobject.transform.localPosition = new Vector3 (11.7f, -43.7f, 0);
			tempGameobject.GetComponent<Button> ().interactable = false;
			break;
		case 50:
			tempGameobject = WholeProgress.instance.five.gameObject;
			tempGameobject.GetComponent<Image> ().sprite = UIHallTexturers.instans.Task[0];
			Destroy (WholeProgress.instance.five);
			tempGameobject.transform.localScale = Vector3.one * 0.63f;
			tempGameobject.transform.localPosition = new Vector3 (127f, -41.2f, 0);
			tempGameobject.GetComponent<Button> ().interactable = false;
			break;
		case 80:
			tempGameobject = WholeProgress.instance.eight.gameObject;
			tempGameobject.GetComponent<Image> ().sprite = UIHallTexturers.instans.Task[1];
			Destroy (WholeProgress.instance.eight);
			tempGameobject.transform.localScale = Vector3.one * 0.71f;
			tempGameobject.transform.localPosition = new Vector3 (250f, -38f, 0);
			tempGameobject.GetComponent<Button> ().interactable = false;
			break;
		case 120:
			tempGameobject = WholeProgress.instance.hund.gameObject;
			tempGameobject.GetComponent<Image> ().sprite = UIHallTexturers.instans.Task[1];
			Destroy (WholeProgress.instance.hund);
			tempGameobject.transform.localScale = Vector3.one * 0.8f;
			tempGameobject.transform.localPosition = new Vector3 (381f, -34.7f, 0);
			tempGameobject.GetComponent<Button> ().interactable = false;
			break;
		default:
			break;
		}
	
	}
	void OnDestroy(){
		UIHall.ResetScrollEvent -= ResetTaskScroll;
		Facade.GetFacade ().ui.Remove ( FacadeConfig.TASK_MODULE_ID );
	}
}