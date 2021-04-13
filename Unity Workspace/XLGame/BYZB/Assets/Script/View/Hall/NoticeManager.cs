using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using UnityEngine.UI;

public class NoticeManager : MonoBehaviour {
	public BackageUI scroll;
	public static NoticeManager instans;
	public InputField inputField;
	public RectTransform content;
	BroadCastInfo nDataInfo;
	MyInfo myInfo;

    public Text text;

	void Start(){
		myInfo=(MyInfo)Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
		nDataInfo =(BroadCastInfo) Facade.GetFacade ().data.Get ( FacadeConfig.BROADCAST_MODULE_ID );
		instans = this;
		RefreshNotice ();

        //text.text = myInfo.Consume.ToString();
	}
	public void CloseButtonClick(){
		AudioManager._instance.PlayEffectClip (AudioManager.effect_closePanel);
		if(BroadCastManager.instans!=null)
			BroadCastManager.instans.broadCastObj.GetComponent<Button> ().interactable = true;
		Destroy (this.gameObject);
	}
	public void RefreshNotice(){
		scroll.cellNumber = nDataInfo.GetNoticeList ().Count;
		scroll.Refresh ();
		if(scroll.cellNumber>7)
			content.localPosition = Vector3.up *185+Vector3.up * (scroll.cellNumber - 7) *65f+Vector3.left*444.5f;
	}
    /// <summary>
    /// 发送按钮的发送方法,出现确定弹窗
    /// </summary>
	public void SendButtonClick(){
		BroadCastManager.instans.broadCastObj.GetComponent<Button> ().interactable = true;
        if (TimeCount.Instanse.EndTime())
        {
            string path = "Window/WindowTipsThree";
            GameObject WindowClone = AppControl.OpenWindow(path);
            WindowClone.SetActive(true);
            UITipAutoNoMask ClickTips1 = WindowClone.GetComponent<UITipAutoNoMask>();
            ClickTips1.tipText.text = "請在" + myInfo.Timer/60 + "分鐘後再次發送";

            return;
        }
        else
        {
            string path = "Window/WindowTipsThree";
            GameObject WindowClone = AppControl.OpenWindow(path);
            WindowClone.SetActive(true);
            UITipAutoNoMask ClickTips1 = WindowClone.GetComponent<UITipAutoNoMask>();
            ClickTips1.tipText.text = "暫未開放";

            return;
        }
    
    


		if (string.IsNullOrEmpty (inputField.text)) {
			string path = "Window/WindowTipsThree";
			GameObject WindowClone = AppControl.OpenWindow (path);
			WindowClone.SetActive (true);
			UITipAutoNoMask ClickTips1 = WindowClone.GetComponent<UITipAutoNoMask> ();
			ClickTips1.tipText.text = "請輸入喊話內容";
			return;
		}
        if (myInfo.diamond < myInfo.Consume) {
			string path = "MainHall/Common/NoticeTipsOne";
			GameObject WindowClone = AppControl.OpenWindow (path);
			WindowClone.SetActive (true);
		} else {
			string path = "MainHall/Common/NoticeTipsTwo";
			GameObject WindowClone = AppControl.OpenWindow (path);
			WindowClone.SetActive (true);
		}

	}
}
