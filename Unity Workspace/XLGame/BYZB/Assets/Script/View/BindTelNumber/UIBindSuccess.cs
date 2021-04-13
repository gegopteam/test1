using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 游客绑定成功之后操作
/// </summary>
public class UIBindSuccess : MonoBehaviour
{
	//int32 propID = 2; 物品Id为1000代表金币，1001代表钻石，0代表没有物品。
	//int32 propCount = 3; 增加或者减少奖励，数量为propCount

	public Text text;

	public Button BtnSure;
	public Button BtnEsc;
	public static int propID;
	public static int propCount;
	public static string TelNum;
	public static string PasW;
	public static FiSystemReward nResponse = new FiSystemReward ();

	//获取特惠礼包奖励物品
	public static List<FiProperty> GetPrefrenceGift = new List<FiProperty> ();

	void Start ()
	{
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		Debug.Log ("显示动画0？");

		text.text = "账号：" + TelNum + ", 密码：" + PasW;



		MyInfo myInfo = DataControl.GetInstance ().GetMyInfo ();

		FiProperty gift = new FiProperty ();
		gift.type = nResponse.propID;
		gift.value = nResponse.propCount;

		GetPrefrenceGift.Add (gift);




		List<FiProperty> mArray = new List<FiProperty> ();
		FiProperty nArray = new FiProperty ();

		BtnSure.onClick.AddListener (() => {
            
			AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
			//领取奖励,关闭弹窗

			RewardMsgHandle msgHandle = new RewardMsgHandle ();
			List<FiRewardStructure> rewardinfo = Facade.GetFacade ().message.reward.GetRewardInfo (RewardMsgHandle.YK_CONVERT_FORMAL_REWARD_TYPE);

			Debug.Log ("显示动画" + rewardinfo.Count);
			for (int i = 0; i < rewardinfo.Count; i++) {
				for (int j = 0; j < rewardinfo [i].rewardPro.Count; j++) {
					nArray.type = rewardinfo [i].rewardPro [j].type;
					nArray.value = rewardinfo [i].rewardPro [j].value;
					mArray.Add (nArray);
				}
			}
			if (UIPhoneBind.Instance != null)
				UIPhoneBind.Instance.OnClose ();
			if (nResponse.resultCode == 0) {
				//MyInfo nUserInfo = (MyInfo)Facade.GetFacade().data.Get(FacadeConfig.USERINFO_MODULE_ID);
				//nUserInfo.loginInfo.monthlyPackGot = true;
				UnityEngine.GameObject Window = Resources.Load ("Window/RewardWindow") as UnityEngine.GameObject;
				GameObject rewardInstance = Instantiate (Window);
				UIReward reward = rewardInstance.GetComponent<UIReward> ();
				//reward.SetRewardData(GetPrefrenceGift);
				reward.SetRewardData (mArray);//更换领取奖励方式



			}

			Destroy (gameObject);

			//switch (propID)
			//{
			//case 1000:
			//    myInfo.gold += propCount;
			//    if (Facade.GetFacade().ui.Get(FacadeConfig.UI_STORE_MODULE_ID) != null)
			//    {
			//        Facade.GetFacade().ui.Get(FacadeConfig.UI_STORE_MODULE_ID).OnRecvData(FiPropertyType.GOLD, myInfo.gold);
			//        Debug.Log("获得" + propCount + "金币");
			//    }
			//    Debug.Log("显示动画2？");
			//    ShowRewardUnits();
			//    Debug.Log("显示动画3？");
			//    propID = 0;
			//    propCount = 0;
			//    break;
			//case 1001:
			//    myInfo._diamond += propCount;
			//    if (Facade.GetFacade().ui.Get(FacadeConfig.UI_STORE_MODULE_ID) != null)
			//    {
			//        Facade.GetFacade().ui.Get(FacadeConfig.UI_STORE_MODULE_ID).OnRecvData(FiPropertyType.DIAMOND, myInfo._diamond);
			//        Debug.Log("获得" + propCount + "钻石");
			//    }
			//    ShowRewardUnits();
			//    propID = 0;
			//    propCount = 0;
			//    break;
			//default :
			////ShowRewardUnits();
			//Debug.Log("无奖励！");
			//break;
			//}



		});

		BtnEsc.onClick.AddListener (() => {
			AudioManager._instance.PlayEffectClip (AudioManager.effect_closePanel);
			Destroy (gameObject);
		});
	}



	/// <summary>
	/// 显示奖励
	/// </summary>
	//    void ShowRewardUnits()
	//    {
	//        Debug.LogError ( "display rewrd units!!!" );
	//        GameObject nWindow = Resources.Load("Image/tools/RewardUnit") as GameObject;    //获取到奖励预设体
	//        GameObject Window = Instantiate(Resources.Load("Window/RewardWindow") as GameObject);//获得显示的外框
	//        GameObject nUnit = Instantiate(nWindow, Window.transform) as GameObject;    //生成的奖励
	//        nUnit.GetComponent<RectTransform>().localPosition = new Vector3(14, 28, 0);
	//        //nUnit.GetComponent<RectTransform>().position = Vector3.zero;
	//        //nUnit.transform.SetParent(Window.transform,true);//----------父对象
	//        //nUnit.transform.localScale = new Vector3(1.35f, 1.35f, 1.35f);
	//        //nUnit.transform.position = new Vector3(825, 555, 0);
	//        Transform icon = nUnit.transform.Find("Icon");
	//        GameObject frame = nUnit.transform.Find("frame").gameObject;
	//        icon.gameObject.GetComponent<Image>().sprite = FiPropertyType.GetSprite(propID);
	//        if (propID == 1000)
	//        {
	//            icon.GetComponent<Image>().SetNativeSize();
	//            icon.GetComponent<RectTransform>().anchoredPosition += Vector2.up * 2;
	//            icon.GetComponent<RectTransform>().localScale = Vector3.one * 0.35f;
	//        }
	//        nUnit.GetComponent<RectTransform>().localPosition -= new Vector3(0, 0, nUnit.GetComponent<RectTransform>().localPosition.z);
	//        int nCountValue = propCount;
	//        string nResult = nCountValue.ToString();
	//        if (nCountValue > 10000)
	//        {
	//            nResult = nCountValue / 10000 + "万";
	//        }
	//        nUnit.transform.Find("NumText").gameObject.GetComponent<Text>().text = "×" + nResult;
	////        Debug.LogError ( nUnit.transform.localScale.x + " / " + nUnit.transform.localScale.y  );
	//}

}
