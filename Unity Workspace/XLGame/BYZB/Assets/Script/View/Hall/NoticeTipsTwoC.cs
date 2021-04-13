using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;
public class NoticeTipsTwoC : MonoBehaviour {
	public Button sure;//右边的确定按钮
	public Button refuse;//左边的取消按钮
	public Text content;
    MyInfo myInfo;
	// Use this for initialization
	void Start () {
        myInfo = (MyInfo)Facade.GetFacade().data.Get(FacadeConfig.USERINFO_MODULE_ID);

        content.text = "是否消耗"+myInfo.Consume+"钻石发布公告？";
		sure.onClick.AddListener (SureButtonClick);
		refuse.onClick.AddListener (RefuseClick);

		UIColseManage.instance.ShowUI (this.gameObject);
	}
	void RefuseClick(){
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		Destroy (this.gameObject);
		UIColseManage.instance.CloseUI();

	}

	void SureButtonClick(){

		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		Facade.GetFacade ().message.broadcast.SendBroadCastMessage ( NoticeManager.instans.inputField.text );
		NoticeManager.instans.inputField.text = null;
		Destroy (this.gameObject);
		Destroy (NoticeManager.instans.gameObject);
	}

}
