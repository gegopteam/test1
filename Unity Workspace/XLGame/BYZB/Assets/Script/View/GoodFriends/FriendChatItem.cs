using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;

public class FriendChatItem : ScrollableCell {
	public Text text;
	public Image pubble;
	public Image head;
	Quaternion myqua=new Quaternion(0,180,0,0);
	FriendChatInfo mChatInfo ;
	FiChatMessage dataInfo;
	MyInfo myInfo;
	void Awake(){
		myInfo =(MyInfo) Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
	}
	public override void ConfigureCellData ()
	{
		base.ConfigureCellData ();
		if (dataObject != null) {
			mChatInfo= (FriendChatInfo)Facade.GetFacade ().data.Get (FacadeConfig.FRIENDCHAT_MODULE_ID);
			dataInfo = mChatInfo.GetChatList (FriendChatManager.instance.chooseID) [(int)dataObject];
			text.text = dataInfo.message;

			if (text.preferredWidth > 339)
				pubble.GetComponent<RectTransform> ().sizeDelta = new Vector2 (359, text.preferredHeight + 20);
			else
				pubble.GetComponent<RectTransform> ().sizeDelta = new Vector2 (text.preferredWidth + 40, text.preferredHeight + 20);

			//如果是对方说话，调用这个函数。
			if (dataInfo.userId!=myInfo.userID) {
				print (dataObject);
				float temp = head.transform.localPosition.y;
				head.transform.localPosition = Vector3.left * 188 + Vector3.up * temp;
				pubble.transform.rotation = myqua;
				temp = pubble.transform.localPosition.y;
				pubble.transform.localPosition = Vector3.left * 150 + Vector3.up * temp;
				text.transform.localRotation = myqua;
			}
		}

	}

	public override void Init (ScrollableAreaController controller, object data, int index, float cellHeight = 0, float cellWidth = 0, ScrollableCell parentCell=null)
	{
		base.Init (controller, data, index, cellHeight, cellWidth, parentCell);
		//FriendChatInfo nInfo = (FriendChatInfo)Facade.GetFacade ().data.Get ( FacadeConfig.FRIENDCHAT_MODULE_ID );
	}
}
