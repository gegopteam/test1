using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using UnityEngine.UI;

public class ChatPanelManager : MonoBehaviour {
	public BackageUI scroll;
	public GameObject spriteGraphic;
	public static ChatPanelManager instans;
	public InputField input;
	public GameObject quickChatPanel;
	public RectTransform content;
	ChatDataInfo nInfo;
	private Transform graphicParent;
	MyInfo myInfo;
	// Use this for initialization
	void Awake(){
		instans = this;
		nInfo = (ChatDataInfo)Facade.GetFacade ().data.Get ( FacadeConfig.CHAT_MODULE_ID );

		myInfo =(MyInfo) Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
		graphicParent = spriteGraphic.transform.parent;
	}
	void Start(){
		scroll.cellNumber = nInfo.getChatList ().Count;
		//scroll.cellNumber=0;
		scroll.Refresh ();
		spriteGraphic.transform.SetSiblingIndex (-1);
		if(scroll.cellNumber>5)
			content.localPosition = Vector3.up *125+Vector3.up * (scroll.cellNumber - 3) * 67.5f+Vector3.right*-335.65f;
	}
	public void SendBtnClick(){
		if (!string.IsNullOrEmpty(input.text)) {
			Debug.LogError (input.text);
			Facade.GetFacade ().message.friend.SendChatMessage (input.text);
			FiChatMessage temp = new FiChatMessage ();
			temp.userId = myInfo.userID;
			temp.message = input.text;
			input.text = "";
			nInfo.addChatMsg (temp);

            if (PrefabManager._instance != null)
            {
                //Debug.LogError("ShowChat:"+temp.message);
                PrefabManager._instance.GetLocalGun().gunUI.ShowChatBubbleBox(temp.message, 3f);
            }

			RefreshList ();
		}
	}
	public void ArrowClick(int num){
		if (content.sizeDelta.y > 202.5f) {
			content.localPosition = Vector3.up*(content.localPosition.y + 67.5f*num)+Vector3.right*-241.9f;
		}
	}
 	public void RefreshList(){
		spriteGraphic.transform.SetParent (transform);
		scroll.cellNumber = nInfo.getChatList ().Count;
		scroll.Refresh ();
		spriteGraphic.transform.SetParent (graphicParent);
		if (content.sizeDelta.y > 350f) {
			content.localPosition = Vector3.up *125+Vector3.up * (scroll.cellNumber - 3) * 67.5f+Vector3.right*-335.65f;
		}
	}
	public void QuickChat(int num){
		FiChatMessage temp = new FiChatMessage ();
		temp.userId = myInfo.userID;
		switch (num) {
		case 1:
			temp.message = "大家好，很高兴见到各位！";
			break;
		case 2:
			temp.message = "抱歉！";
			break;
		case 3:
			temp.message = "打打打，看你能得意多久。";
			break;
		case 4:
			temp.message = "技不如人，甘拜下风！";
			break;
		case 5:
			temp.message = "不好意思，又赢了！";
			break;
		}
		if (temp.message != null) {
            
            if (PrefabManager._instance != null) {
                //Debug.LogError("ShowChat:"+temp.message);
                PrefabManager._instance.GetLocalGun().gunUI.ShowChatBubbleBox(temp.message, 3f,num);
            }

			quickChatPanel.SetActive (false);
			nInfo.addChatMsg (temp);
			Facade.GetFacade ().message.friend.SendChatMessage (temp.message);
			RefreshList ();
		}
	}
	public void OnButton()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.effect_closePanel);
		Destroy(this.gameObject);
	}
}
