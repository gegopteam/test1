using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class OtherUserInfoBox : MonoBehaviour
{

	bool isShow = false;
	GunControl thisGun = null;

	public Image boxImage;
	public Image avatarImage;

	public Text nickNameText;
	public Text levelText;
	public Text gunLevelText;

	public static OtherUserInfoBox _instance;

	void Awake()
	{
		if (null == _instance)
			_instance = this;
		//Hide ();
	}

	private void Start()
	{
		Hide();
	}

	void Update()
	{
		if (isShow)
		{
			if (Input.GetMouseButtonDown(0))
			{
				if (EventSystem.current.IsPointerOverGameObject())
				{ //点击到UI层时直接return，视为无效输入
					return;
				}
#if !UNITY_EDITOR //后期可能要对IOS和安卓平台单独检测
				if (ScreenManager.IsPointOverUI (Input.GetTouch (0).fingerId)) {
					return;
				}
#endif

				Hide();
			}
		}
	}

	public void Show(GunControl gun)
	{
		//		Debug.LogError ("ShowOtherUserInfo");
		if (isShow)
		{
			Hide();
		}

		isShow = true;
		thisGun = gun;
		nickNameText.text = gun.gunUI.GetNickName();
		levelText.text = "等級:" + gun.level.ToString();
		gunLevelText.text = "最大炮倍數:" + gun.maxCannonMultiple;
		avatarImage.sprite = gun.GetAvatorSprite();

		transform.position = gun.gunUI.bonusEffectPos.position;
		if (gun.thisSeat == GunSeat.LB || gun.thisSeat == GunSeat.RB)
		{ //在下方
			boxImage.rectTransform.localScale = new Vector3(1, -1, 1) * 1.846313f;
		}
		else
		{ //在上方
			boxImage.rectTransform.localScale = new Vector3(1, 1, 1) * 1.846313f;
		}
	}

	public void Hide()
	{
		isShow = false;
		thisGun = null;
		this.transform.position = Vector3.up * 1000f;

	}

	public void AddFriend()
	{
		AudioManager._instance.PlayEffectClip(AudioManager.ui_click);

		if (thisGun.isRobot || thisGun.gameID == 0)
		{ //如果是机器人，直接显示UI，不发送消息
			HintTextPanel._instance.SetTextShow("好友請求已發送!", 2.5f);
			return;
		}

		//否则判断是否是好友，对应显示两种UI
		FriendInfo nInfo = (FriendInfo)Facade.GetFacade().data.Get(FacadeConfig.FRIEND_MODULE_ID);
		List<FiFriendInfo> nInfoList = nInfo.GetFriendList();

		for (int i = 0; i < nInfoList.Count; i++)
		{
			if (thisGun.gameID == nInfoList[i].gameId)
			{
				HintTextPanel._instance.SetTextShow("你們已經是好友了", 2.5f);
				return;
			}
		}

		FriendMsgHandle friendMsgHanle = new FriendMsgHandle();
		friendMsgHanle.SendAddFriend(thisGun.gameID);
		HintTextPanel._instance.SetTextShow("好友請求已發送!", 2.5f);

	}

	public GunSeat GetSeat()
	{
		if (thisGun == null)
		{
			return GunSeat.Error;
		}
		else
		{
			return thisGun.thisSeat;
		}
	}


}
