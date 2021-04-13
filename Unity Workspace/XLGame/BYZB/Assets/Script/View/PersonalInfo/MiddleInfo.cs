using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;

public class MiddleInfo : MonoBehaviour
{

	public Image head;
	public Text userID;
	public Text level;
	public GameObject Vip;
	public Image VipLevel;
	public Image sex;
	public Text name;
	private Object[] num;
	public static MiddleInfo instance;

	void Awake ()
	{
		instance = this;
	}

	// Use this for initialization
	void Start ()
	{
		
		MyInfo nInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
		userID.text = nInfo.loginInfo.gameId.ToString ();

		name.text = Tool.GetName (nInfo.nickname, 6);// "你好呀";
		level.text = nInfo.level + "";// "100";
		if (nInfo.sex == 1) {
			sex.sprite = UIHallTexturers.instans.Ranking [7];
		} else {
			sex.sprite = UIHallTexturers.instans.Ranking [2];
		} 
		//添加头像
		if (!string.IsNullOrEmpty (nInfo.avatar)) {
			AvatarInfo nAvaInfo = (AvatarInfo)Facade.GetFacade ().data.Get (FacadeConfig.AVARTAR_MODULE_ID);
			nAvaInfo.Load (nInfo.userID, nInfo.avatar, (int nResult, Texture2D nTexture) => {
				if (nResult == 0 && head != null) {
					if (nTexture.width == 8 && nTexture.height == 8) {
						return;
					}
					nTexture.filterMode = FilterMode.Bilinear;
					nTexture.Compress (true);
					head.sprite = Sprite.Create (nTexture, new Rect (0, 0, nTexture.width, nTexture.height), new Vector2 (0, 0));
				}
			}
			);
		}
		if (nInfo.levelVip == 0) {
			//Vip.SetActive (false);
			Vip.GetComponent<Image> ().sprite = UIHallTexturers.instans.VipFrame [0];
		} else {
			//Vip.SetActive (true);
			Vip.GetComponent<Image> ().sprite = UIHallTexturers.instans.VipFrame [nInfo.levelVip];
			//VipLevel.sprite = UIHallTexturers.instans.RankNum [nInfo.levelVip];
		}


	}

	public void ShowInputName ()
	{
		string paths = "MainHall/Common/NoticeInputName";
		GameObject WindowClone = AppControl.OpenWindow (paths);
		WindowClone.SetActive (true);
	}

	public void OnChangeName (string newname)
	{
		name.text = Tool.GetName (newname, 8);
	}

}