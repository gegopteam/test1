/* author:KinSen
 * Date:2017.05.23
 */

using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public enum DispatchType
{
	CLOSE,
	//关闭
	PAUSE,
	//暂停
	InUpdate,
	//在Update函数中分发消息
	InFixedUpdate,
	//在FixedUpdate函数中分发消息
	InCoroutine,
	//在协程函数中分发消息
}

/* do:简化该类
 * 测试协程函数，没有while的情况，会重复调用吗？
 * 协程是如何关闭while的
 */

//负责：把Socket中的信息接收到UI层，功能单一，执行效率
public class SocketToUI : MonoBehaviour
{

	public delegate void OnHttpCompelete (int nResult, Texture2D nTexture);

	private OnHttpCompelete mHttpCallback;

	private bool isLoadingTaskTurnOn = false;
	private string mAvatarUrl = null;

	private static SocketToUI instance = null;

	public static SocketToUI GetInstance ()
	{
		if (null == instance) {
			GameObject prefab = (GameObject)Resources.Load ("Prefabs/SocketToUI");
			//GameObject prefab = ResControl.GetInstance ().prefabInfo.GetPrefab (TypePrefab.SocketToUI);
			if (null == prefab) {
				Tool.LogError ("SocketToUI GetPrefab fail");
				return null;
			}
			GameObject obj = Instantiate (prefab);
			if (null == obj) {
				Tool.LogError ("SocketToUI Instantiate fail");
				return null;
			}
			Tool.Log ("SocketToUI success");
			instance = obj.GetComponent<SocketToUI> ();

            //如果timecount脚本丢失,重新自动挂载
            if(obj.GetComponent<TimeCount>() == null)
            {
                obj.AddComponent<TimeCount>();
            }
		}
		return instance;
	}

	public static void DestroyInstance ()
	{
		if (null != instance) {
			instance = null;
		}
	}

	private DispatchControl dispatchControl = null;
	private DispatchType dispatchType = DispatchType.CLOSE;
	private DispatchType dispatchTypeLast = DispatchType.CLOSE;

	void Awake ()
	{
		dispatchControl = DispatchControl.GetInstance ();
		InfoMsg.GetInstance ();
	}

	//IEnumerator nStateEnum = null;

	//bool bNeedNetWorkErrorTip = false;

	// Use this for initialization
	void Start ()
	{
		DontDestroyOnLoad (this.gameObject); //场景切换时不销毁
		RegeditNotification ();
		CleanNotification ();
		StartCoroutine (OnNetWorkStateCheck ());
	}

	IEnumerator OnNetWorkStateCheck ()
	{
		while (true) {
			yield return new WaitForSeconds (2);
			if (Application.internetReachability == NetworkReachability.NotReachable) {   
				Debug.LogError ("net work error");
				/*GameObject Window1 = UnityEngine.Resources.Load ("Window/WindowTipsThree")as UnityEngine.GameObject;
				GameObject WindowClone1 = UnityEngine.GameObject.Instantiate ( Window1 );
				UITipAutoNoMask ClickTips1 = WindowClone1.GetComponent<UITipAutoNoMask> ();
				ClickTips1.tipText.text = "网络连接异常!";*/
				//if ( bNeedNetWorkErrorTip == false ) {
				//Invoke ( "OnShutDownHandler" , 1.0f );
				//}
				OnShutDownHandler ();
			} else {
				//bNeedNetWorkErrorTip = false;
				//Debug.LogError ("net work success");
			}
		}
	}

	void OnShutDownHandler ()
	{
		Debug.LogError (" ---------- network error------------");
		//如果不在登陆场景，并且网络异常的情况下，发送close消息
		if (AppControl.GetInstance ().getActiveView () != AppView.LOGIN) {
			DispatchData nData = new DispatchData ();
			nData.type = FiEventType.CONNECTIONT_CLOSED;
			EventControl.instance ().dispatch (nData);
			//bNeedNetWorkErrorTip = true;
		}
	}

	// Update is called once per frame
	void Update ()
	{
		OnDispatchInUpdate ();
		if (isLoadingTaskTurnOn) {
			//Debug.LogError ( "-------LoadingImage---------------" + mAvatarUrl );
			StartCoroutine (LoadingImage (mAvatarUrl)); 
			isLoadingTaskTurnOn = false;
		}

	}

	void FixedUpdate ()
	{
		OnDispatchInFixedUpdate ();
	}

	void OnDispatchInUpdate ()
	{
		if (DispatchType.PAUSE == dispatchType)
			return;
		if (DispatchType.InUpdate == dispatchType) {
			dispatchControl.Dispatch ();
		}
	}

	void OnDispatchInFixedUpdate ()
	{
		if (DispatchType.PAUSE == dispatchType)
			return;
		if (DispatchType.InFixedUpdate == dispatchType) {
			dispatchControl.Dispatch ();
		}
	}

	void OnDispatchInCoroutine ()
	{
		if (DispatchType.PAUSE == dispatchType)
			return;
		if (DispatchType.InCoroutine == dispatchType) {
			dispatchControl.Dispatch ();
		}
	}

	public DispatchType GetDispatchType ()
	{
		return dispatchType;
	}

	public void OpenDispatch (DispatchType type = DispatchType.InFixedUpdate)
	{//UI接收Socket消息(即socket线程转UI线程)
		if (dispatchType == type) //如果设置的类型是当前类型就不进行设置
			return;

		if (DispatchType.PAUSE != dispatchType) {//如果状态是暂停，则进行赋值
			dispatchTypeLast = dispatchType;
		}

		dispatchType = type;
		if (DispatchType.InCoroutine == dispatchTypeLast) {
			Tool.Log ("CloseDispatch DispatchType.InCoroutine");
			StopCoroutine ("ToDispatch");
			return;
		}

		if (DispatchType.InCoroutine == dispatchType) {
			Tool.Log ("OpenDispatch DispatchType.InCoroutine");
			StartCoroutine ("ToDispatch");
			return;
		}

	}

	public void LoadAvatar (string nAvatarUrl, OnHttpCompelete nCallback)
	{
		mHttpCallback = nCallback;
		mAvatarUrl = nAvatarUrl;
		isLoadingTaskTurnOn = true;
	}


	IEnumerator LoadingImage (string path)
	{
		WWW www = new WWW (path);
		yield return www;
		Texture2D image = www.texture;
		if (mHttpCallback != null) {
			//有错误信息
			if (!string.IsNullOrEmpty (www.error)) {
				Debug.LogError ("------------" + www.error + path);
				mHttpCallback (-1, null);
			} else {
				//Debug.LogError ( "-------success-----" + www.error );
				mHttpCallback (0, image);
			}
			//mHttpCallback = null;
		}
	}

	IEnumerator ToDispatch ()
	{//socket线程收集信息，UI线程提取信息
		yield return new WaitForSeconds (0.1f);
		while (true) {
			Debug.LogError ("ToDispatch");
			OnDispatchInCoroutine ();
			yield return new WaitForSeconds (0.1f);
		}
	}


	public static void RegeditNotification ()
	{
		#if UNITY_IPHONE
		UnityEngine.iOS.NotificationServices.RegisterForNotifications (UnityEngine.iOS.NotificationType.None | UnityEngine.iOS.NotificationType.Badge | UnityEngine.iOS.NotificationType.Alert | UnityEngine.iOS.NotificationType.Sound);
		#endif
	}

	//本地推送 你可以传入一个固定的推送时间
	public static void DoNotification (string message, System.DateTime nDateTime)
	{
		#if UNITY_IPHONE
		RegeditNotification ();
		if (nDateTime > System.DateTime.Now) {
			UnityEngine.iOS.LocalNotification localNotification = new UnityEngine.iOS.LocalNotification ();
			localNotification.fireDate = nDateTime;
			localNotification.alertBody = message;
			localNotification.applicationIconBadgeNumber = 0;
			localNotification.hasAction = true;
			/*if(isRepeatDay)
		{//是否每天定期循环
			localNotification.repeatCalendar = CalendarIdentifier.ChineseCalendar;
			localNotification.repeatInterval = CalendarUnit.Day;
		}*/
			localNotification.soundName = UnityEngine.iOS.LocalNotification.defaultSoundName;
			UnityEngine.iOS.NotificationServices.ScheduleLocalNotification (localNotification);
		}
		#endif
	}

	string GetNotificationContent (bool isMorning, int nDayIndex)
	{
		string nDayString = nDayIndex == 2 ? "二" : "三";
		if (isMorning) 
        {
			return "今日可領取起航禮包第" + nDayString + "彈，內含豐厚獎勵！";
		}
		return "起航禮包第" + nDayString + "彈獎勵仍未領取，錯過將無法領取！";
	}

	void OnApplicationPause (bool paused)
	{
		//程序进入后台时
		if (paused) 
        {
			//10秒后发送
			//Debug.LogError( "-------------OnApplicationPause----------------" );
			//DoNotification("-------------test for Start Gift Warning when back 10 seconds-------------" , System.DateTime.Now.AddSeconds( 10 ) );
			MyInfo nInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
			int nDayIndex = System.Math.Abs (nInfo.sailDay);
//			nDayIndex = 1;
			System.DateTime nCurrentDay = System.DateTime.Now;

			/*System.DateTime nTest =   nCurrentDay.AddSeconds(10);
			nTest = new System.DateTime( nTest.Year , nTest.Month , nTest.Day , nTest.Hour , nTest.Minute  ,nTest.Second );
			DoNotification( GetNotificationContent ( false , 2 ) , nTest );*/
            
			if (nDayIndex == 1 || nDayIndex == 2) {
				//明天的起航礼包早晨提醒
				System.DateTime nMorning = nCurrentDay.AddDays (1);
				nMorning = new System.DateTime (nMorning.Year, nMorning.Month, nMorning.Day, 8, 0, 0);
				DoNotification (GetNotificationContent (true, nDayIndex + 1), nMorning);

				//明天的起航礼包傍晚提醒
				System.DateTime nNight = nCurrentDay.AddDays (1);
				nNight = new System.DateTime (nNight.Year, nNight.Month, nNight.Day, 22, 0, 0);
				DoNotification (GetNotificationContent (false, nDayIndex + 1), nNight);
			}
		} else {
			//程序从后台进入前台时
			CleanNotification ();
		}
	}



	void OnDestroy ()
	{
		Debug.LogError ("-----------application OnDestroy-----------");
		DataControl.GetInstance ().ShutDown ();
	}


	void CleanNotification ()
	{
		#if UNITY_IPHONE
		UnityEngine.iOS.LocalNotification l = new UnityEngine.iOS.LocalNotification (); 
		l.applicationIconBadgeNumber = 0; 
		UnityEngine.iOS.NotificationServices.PresentLocalNotificationNow (l); 
		UnityEngine.iOS.NotificationServices.CancelAllLocalNotifications (); 
		UnityEngine.iOS.NotificationServices.ClearLocalNotifications (); 
		#endif
	}

}
