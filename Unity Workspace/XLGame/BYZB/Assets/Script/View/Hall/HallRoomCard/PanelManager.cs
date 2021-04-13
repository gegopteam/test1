using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System .Collections.Generic;
using AssemblyCSharp;
/// <summary>
/// Panel manager.创建房间时，Toggle的选项，并使用infoList保存，创建不同房间的结构体，传给服务器不同的值
/// </summary>

public class PanelManager : MonoBehaviour
{
	private string coinMuch = "1萬";
	private string time;
	private string round;
	public static int modelIndex;
	private  string text;

	public Sprite[] IconCost;

	private ToggleGroup[] toggleGroup;

	public  Image roomPay = null;

	public static ArrayList infoList = new ArrayList ();

	// Use this for initialization
	void Start () {
		//uiHallMsg = UIHallMsg.GetInstance ();
		toggleGroup = GetComponentsInChildren<ToggleGroup> ();
	}

//	void OnGUI()
//	{
//		Tool.ShowInGUI ();
//	}


	// Update is called once per frame
	void Update()
	{
		
	}

	void ChangeRoomPay(IEnumerable<Toggle>  toggle)
	{
		foreach(Toggle  t in toggle)
		{
			if (t.isOn) {

				text = t.gameObject.GetComponentInChildren<Image>().transform.Find("Image").GetComponent<Image>().sprite.name;
			} 
		}
		switch (text) {
		case "1萬":
			roomPay.sprite = IconCost[ 0 ];// Resources.Load ("Room/1万1", typeof(Sprite))as Sprite;
			roomPay.rectTransform.sizeDelta = new Vector2 ( 74, 50 );
			break;
		case "10萬":
			roomPay.sprite = IconCost [1];//Resources.Load ("Room/5万",typeof(Sprite))as Sprite;
			roomPay.rectTransform.sizeDelta = new Vector2 ( 82, 50 );
			break;
		case "100萬":
			roomPay.sprite= IconCost [2];//Resources.Load ("Room/10万1",typeof(Sprite))as Sprite;
			roomPay.rectTransform.sizeDelta = new Vector2 ( 102, 50 );
			break;
		}
		//Debug.LogError ( "---------------------" + roomPay.rectTransform.localScale.x );
	}

	string  Check(IEnumerable<Toggle>  toggle)
	{
		foreach(Toggle  t in toggle)
		{
			if (t.isOn) {
				text = t.gameObject.GetComponentInChildren<Image>().transform.Find("Image").GetComponent<Image>().sprite.name;
				Debug.Log (text);
			}
		}
		return text;
	}

	public void OnCostChange( bool nValue )
	{
		for (int i = 0; i < toggleGroup.Length; i++) 
		{
			IEnumerable<Toggle> toggele;
			switch(toggleGroup [i].name)
			{
			case "SignUpChoose":
				toggele = toggleGroup [i].ActiveToggles ();
				ChangeRoomPay (toggele);
				break;
			}
		}
	}

	public void SetInfoList(int model)
	{
		int coin = 0;
		int person =0;
		for (int i = 0; i < toggleGroup.Length; i++) {
			IEnumerable<Toggle> toggele = toggleGroup [i].ActiveToggles ();
			switch(toggleGroup [i].name)
			{
			case "RoundChoose":
				round = Check (toggele);
				break;
			case "TimeChoose":
				time = Check (toggele);
				break;
			case "SignUpChoose":
				coinMuch = Check (toggele);
				break;
			}
		}
		modelIndex = model;
		Debug.Log ("ModelIndex"+modelIndex);
		infoList.Clear ();
		infoList.Add (coinMuch);
		infoList.Add (time);
		infoList.Add (round);
		Debug.Log (infoList[0].ToString()+infoList[1].ToString()+infoList[2].ToString());
		//服务器所需的数据
		SetServer ();

	}
		
	void SetServer()
	{
		int Time = 0;
		int Coin = 0;
		int Round = 0;

		switch(time)
		{
		case "3分鐘":
			Time = 0;
			break;
		case "5分鐘":
			Time = 1;
			break;
		}
		switch (coinMuch) {
		case "1萬":
			Coin = 1;
			break;
		case "10萬":
			Coin = 2;
			break;
		case "100萬":
			Coin = 3;
			break;
		}
		switch(round)
		{
		case "1局":
			Round = 0;
			break;
		case "3局":
			Round = 1;
			break;
		case "5局":
			Round = 2;
			break;
		}
		Tool.Log ("創建房間，向服務器發送數據");
		Debug.Log ("ModelIndex"+modelIndex);

		//Debug.LogError (  modelIndex + " / " + Coin + " / " + Time+ " / " + Round );
		Facade.GetFacade ().message.fishFriend.SendPKCreateFriendRoomRequest ( modelIndex, Coin , Time, Round );
	}

	void OnDestroy()
	{
		
	}
}
