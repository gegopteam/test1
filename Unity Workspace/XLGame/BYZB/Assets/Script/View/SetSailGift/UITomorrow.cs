using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;
using DG.Tweening;

public class UITomorrow : MonoBehaviour {

	public delegate void OnWindowClose();

	private OnWindowClose mCloseHandler;

	public Image dayImage;

	float nDeltaTime = 0;

	public Text  txtRemainTime;

	public Text  txtGiftGolds;

	public Sprite IconThirdDay;

	int mDayIndex = 2;

	// Use this for initialization
	void Start () {
		Init ();
	}

	public void RegisterCloseCallBack( OnWindowClose nCallback )
	{
		mCloseHandler = nCallback;
	}

	public void SetDayIndex( int nDayIndex )
	{
		mDayIndex = nDayIndex;
	}

	void Init()
	{
		//MyInfo myInfo = (MyInfo) Facade.GetFacade().data.Get( FacadeConfig.USERINFO_MODULE_ID );
		if (mDayIndex == 2) {
			//dayImage.sprite = Resources.Load ("Image/第2弹",typeof(Sprite))as Sprite;
		} else {
			dayImage.sprite = IconThirdDay;
			//dayImage.sprite = Resources.Load ("Image/第3弹",typeof(Sprite))as Sprite;
			txtGiftGolds.text = 30000 + "";
		}
	}
	
	// Update is called once per frame
	void Update () {

		nDeltaTime += Time.deltaTime;

		if (nDeltaTime < 0.5) {
			return;
		}
	//	Debug.LogError ( nDeltaTime + "/" +Time.deltaTime);
		nDeltaTime = 0;

		System.DateTime nNow = System.DateTime.Now;

		int nRemianSecond = 86400 -  (nNow.Hour * 3600 + nNow.Minute * 60 + nNow.Second);

		int nHours = nRemianSecond/3600;
		int nMinutes =(nRemianSecond - nHours * 3600 )/60;
		int nSecond = nRemianSecond - nHours * 3600 - nMinutes * 60;

		string nStrHour = (nHours < 10) ? "0" + nHours : nHours.ToString();
		string nStrMinute = (nMinutes < 10) ? "0" + nMinutes : nMinutes.ToString();
		string nStrSecond =(nSecond < 10) ? "0" + nSecond : nSecond.ToString();
		txtRemainTime.text = nStrHour + ":" + nStrMinute + ":" + nStrSecond;
	}

	public void ExitButton()
	{
		/*IUiMediator nMediatorHall= Facade.GetFacade ().ui.Get ( FacadeConfig.UI_HALL_MODULE_ID );
		if (nMediatorHall != null) {
			((UIHall)nMediatorHall).ShowStartGiftBoxTip ();
		}*/
		Debug.LogError ( "-------ExitButton------" );

		transform.Find ( "Panel" ).DOScale( new Vector3 (0.1f, 0.1f, 0.1f) , .3f );
		Invoke ("OnAnimateCompelte", 0.3f);
	}

	void OnAnimateCompelte()
	{
		if (mCloseHandler != null) {
			mCloseHandler ();
		}
		Destroy (this.gameObject);
	}

}
