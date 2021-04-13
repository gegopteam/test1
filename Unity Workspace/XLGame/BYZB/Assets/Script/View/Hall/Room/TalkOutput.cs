using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TalkOutput : MonoBehaviour {
	public GameObject[] talkText;
	private static int count;
	public Text [] nickname;
	public Text [] talks;
	public InfinityGridLayoutGroupRoom GridGroup;

	private MyInfo myInfo;

	int amount = 5;
//	infinityGridLayoutGroup.SetAmount(amount);
//	infinityGridLayoutGroup.updateChildrenCallback = UpdateChildrenCallback;
	// Use this for initialization
	void Start () {
		count = 0;
		TalkInput.InputTextEvent += ShowTalk;
		nickname = new Text[talkText.Length];
		talks = new Text[talkText.Length];
		myInfo = DataControl.GetInstance ().GetMyInfo ();
		GridGroup.SetAmount (amount);
	}

	void ShowTalk(string talk)
	{   
		//让聊天框返回原来位置，以前的消息都消除
		if (count == talkText.Length) {
			GridGroup.SetAmount (amount);
			for (int j = 0; j < talkText.Length; j++) {
				nickname[j] = talkText [j].transform.Find ("NickName").GetComponent<Text> ();
				talks[j] = talkText [j].transform.Find ("Text").GetComponent<Text> ();
				nickname [j].text = "";
				talks [j].text = "";
			}
			count = 0;
		
		}
		Debug.Log (count);
		nickname[count] = talkText [count].transform.Find ("NickName").GetComponent<Text> ();
		talks[count] = talkText [count].transform.Find ("Text").GetComponent<Text> ();
		//获取昵称
		nickname [count].text = myInfo.nickname;
		talks [count].text = talk;
		count++;


	}

	// Update is called once per frame
	void Update () {
	
	}
		
}
