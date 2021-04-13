using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;

public class ChatItem : ScrollableCell {
	public Text talk;
	public Image head;
	public Text nametxt;
	ChatDataInfo nInfo;
	FiChatMessage data;
	MyInfo myInfo;
	Sprite headSp;
	void Awake(){
		headSp = head.sprite;
		nInfo = (ChatDataInfo)Facade.GetFacade ().data.Get ( FacadeConfig.CHAT_MODULE_ID );
		myInfo =(MyInfo) Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
	}
	public override void ConfigureCellData ()
	{
		base.ConfigureCellData ();
		if (dataObject != null) 
			data = nInfo.getChatList () [(int)dataObject];
		if(data!=null&& this.gameObject.activeInHierarchy){
			//if ((int)dataObject % 2 == 0) {
				//this.GetComponent<Image> ().enabled = false;
				//text.text = "<color=#7e3217ff>" + "这里是别人的名字：" + "</color>" + "<color=#b04d29ff>" + "这里是别人要说的内容[#0" + (int)dataObject % 10 + "]" + "</color>";
			//} else {
				//this.GetComponent<Image> ().enabled = true;
				//text.text = "<color=#ffffffff>" + "这里是我的名字：" + "</color>" + "<color=#ffea00ff>" + "这里是我要说的内容[#0" + (int)dataObject % 10 + "]" + "</color>";
			//}
			if (PrefabManager._instance.GetGunByUserID (data.userId) == null)
				head.sprite = null;
			else
			head.sprite= PrefabManager._instance.GetGunByUserID(data.userId).GetAvatorSprite();
			if (head.sprite == null)
				head.sprite = headSp;
			if (data.userId == myInfo.userID) {
				nametxt.text = "<color=#00ffe1ff>" + PrefabManager._instance.GetGunByUserID (data.userId).gunUI.GetNickName () + "</color>";
				talk.text =  "<color=#ffcb7dff>" + data.message + "</color>";
			}
			else{
				if (PrefabManager._instance.GetGunByUserID (data.userId) != null) {
					nametxt.text = "<color=#ffffffff>" + PrefabManager._instance.GetGunByUserID (data.userId).gunUI.GetNickName () + "</color>";
					talk.text = "<color=#ffffffff>" + data.message + "</color>";
				} else {
					nametxt.text = "<color=#ffffffff>" + "玩家" + data.userId + "</color>";
					talk.text =  "<color=#ffffffff>" + data.message + "</color>";
				}
			}
			talk.transform.parent.GetComponent<RectTransform> ().sizeDelta = new Vector2 (talk.preferredWidth + 40, 40);

		}
	}
}
