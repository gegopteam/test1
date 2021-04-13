using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AssemblyCSharp;
using System.Text;
using System;

public class BigwinRankItem : ScrollableCell
{

	public Text ranknum;
	public Text usename;
	public Text streamflow;


	public  Text levetext;
	public GameObject Vip;
	public Image numimage;
	public Image headimage;
	public Image RewardBox;
	public Image RankBg;
	public Image RankAllbg;


	public Sprite tempHead;
	AvatarInfo nAvaInfo;
	private FiRankInfo data;
	private NobelInfo nobelinfo;
	MyInfo myInfo;
	// Use this for initialization
	void Start ()
	{
		//tempHead = headimage;
	}

	IEnumerator ChangeImage (Image headImage, string path)
	{
		WWW www = new WWW (path);
		yield return www;

		if (www.error == null) {
			Texture2D image = www.texture;
			headImage.sprite = Sprite.Create (image, new Rect (0, 0, image.width, image.height), new Vector2 (0, 0));
		} 
//		else
//			headImage.sprite = tempHead;
	}

	public override void ConfigureCellData ()
	{
		base.ConfigureCellData ();
		if (dataObject != null) {
			data = UIBigWinner.nRankData [(int)dataObject];

		}


		if (data != null && this.gameObject.activeInHierarchy) {
			if ((int)dataObject < 3) {
				numimage.gameObject.SetActive (true);
				RewardBox.gameObject.SetActive (true);
				RankBg.gameObject.SetActive (false);	
				numimage.sprite = UIHallTexturers.instans.Ranking [(int)dataObject + 16];//Resources.Load<Sprite> ("Ranking/排名" + ((int)dataObject + 1));
				ranknum.text = "";
			} else {
				numimage.gameObject.SetActive (false);
				RewardBox.gameObject.SetActive (false);
				RankBg.gameObject.SetActive (true);
				ranknum.text = ((int)dataObject + 1).ToString ();
			}

			usename.text = Tool.GetName (data.nickname, 10);

			streamflow.text = data.gold.ToString ("N0");
			//usename.text = data.nickname;
			levetext.text = "Lv." + data.level.ToString ();

			nobelinfo = (NobelInfo)Facade.GetFacade ().data.Get (FacadeConfig.UI_NOBEL_ID);
			myInfo = (MyInfo)Facade.GetFacade ().data.Get (FacadeConfig.USERINFO_MODULE_ID);
			Debug.LogError ("myinfouserid: " + myInfo.userID + " ;data.userid :" + data.userId);
			if ((int)dataObject == nobelinfo.Myselfindex && myInfo.userID == data.userId) {
				
				RankAllbg.gameObject.SetActive (true);
			} else {
				RankAllbg.gameObject.SetActive (false);
			}

			RankPlayerHeadImg ();
			Debug.LogError ("关于头像" + data.nickname + "id:" + data.userId + "头像链接" + data.avatarUrl + "有东西吗");
			//nAvaInfo = (AvatarInfo)Facade.GetFacade ().data.Get (FacadeConfig.AVARTAR_MODULE_ID);
			//         nAvaInfo.Load ((int)data.userId, data.avatarUrl, (int nResult, Texture2D nTexture) => {
			//	if (headimage != null) {
			//		if (nResult == 0) {
			//			//Debug.LogError (UserAvatar +  "-----22-----" + currentUserId + "/"+nTexture + "/" + UserIcon  + "-----" + UserIcon.GetComponentInParent<Image>() );
			//			headimage.sprite = Sprite.Create (nTexture, new Rect (0, 0, nTexture.width, nTexture.height), new Vector2 (0, 0));
			//		} else
			//			headimage = tempHead;
			//	}
			//});


//			headimage.gameObject.GetComponent<Button> ().onClick.AddListener (() => HeadClick (data));
			if (data.vipLevel == 0) {
				//Vip.SetActive (true);
				Vip.GetComponent<Image> ().sprite = UIHallTexturers.instans.VipFrame [10];
				Vip.gameObject.transform.localScale = new Vector3 (0.43f, 0.43f, 0.43f);
//				effect.SetActive (false);
			} else {
				//Vip.SetActive (true);
				Vip.GetComponent<Image> ().sprite = UIHallTexturers.instans.VipFrame [data.vipLevel];
//				if (data.vipLevel == 9)
////					effect.SetActive (true);
//				else
//					effect.SetActive (false);
				//	VipLevel.sprite = UIHallTexturers.instans.RankNum [data.vipLevel];
			}
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	/// <summary>
	/// 头像的处理 ,yky加
	/// </summary>
	void  RankPlayerHeadImg ()
	{

		if (!string.IsNullOrEmpty (data.avatarUrl)) {
			nAvaInfo = (AvatarInfo)Facade.GetFacade ().data.Get (FacadeConfig.AVARTAR_MODULE_ID);

			nAvaInfo.Load ((int)data.userId, data.avatarUrl, (int nResult, Texture2D nTexture) => {
				if (headimage != null) {
					if (nResult == 0) {
						//Debug.LogError (UserAvatar +  "-----22-----" + currentUserId + "/"+nTexture + "/" + UserIcon  + "-----" + UserIcon.GetComponentInParent<Image>() );
						headimage.sprite = Sprite.Create (nTexture, new Rect (0, 0, nTexture.width, nTexture.height), new Vector2 (0, 0));
					} else
						headimage.sprite = tempHead;
				}
			});
		} else {
			Debug.LogError ("头像是空的");
			headimage.sprite = tempHead;
		}

	}
}
