using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;

public class BankGiveTips : MonoBehaviour {

	public Button sure;
	public Button refuse;
	public Text content;
	FiUserInfo userInfo;
	int giftType=0;
	public static BankGiveTips instanse;
	Bank_PresentManager presentManager;
	// Use this for initialization
	void Awake () {
		instanse = this;
		sure.onClick.AddListener (SureClick);
		refuse.onClick.AddListener (Refuse);
		presentManager = BankManager.instance.PresentGameObject.GetComponent<Bank_PresentManager> ();
	}
	void Start(){
        content.text = "贈送" + presentManager.GiveNumber+GetGiftName(presentManager.Cost)+ "給玩家\n“" + Tool.GetName( userInfo.nickName,4) +"”\n（ID:" + userInfo.gameId + "）";
		UIColseManage.instance.ShowUI (this.gameObject);
	}
	void SureClick(){
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		if (userInfo != null) {
			FiProperty sendPro = new FiProperty ();
			sendPro.type = giftType;
			sendPro.value = presentManager.GiveNumber;
			Facade.GetFacade ().message.bank.SendCLGiveRequest (2,Google.Protobuf.ByteString.CopyFromUtf8(Tool.GetMD532(presentManager.password.text)),userInfo.gameId,sendPro);//presentManager.GiveNumber,presentManager.Cost,Tool.GetMD532(presentManager.password.text),userInfo.userId);
		}
	}
	public void GiveSucess(){
		Destroy (this.gameObject);
	}
	void Refuse(){
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		DestroyImmediate (this.gameObject);
	}
	public void SetUser(FiUserInfo user){
		userInfo = user;
	}
	string GetGiftName(long giftCost){
		switch (giftCost) {
        case 10000:
            giftType = 1;
            return "個蛋糕";
            break;
		case 100000:
			giftType = 2;
			return "輛跑車";
			break;
		case 500000:
			giftType = 3;
			return "支車隊";
			break;
		case 1000000:
			giftType = 4;
			return "台飛機";
			break;
		default:
			return null;
			break;
		}
	}
}
