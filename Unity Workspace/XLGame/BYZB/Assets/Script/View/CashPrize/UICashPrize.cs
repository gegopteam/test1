using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UICashPrize : MonoBehaviour {

	private AppControl appControl = null;

	public Text sumRedPacketText;

	void Awake()
	{
		appControl = AppControl.GetInstance ();
	}

	void Start()
	{
		//DontDestroyOnLoad (this.gameObject);

	}

	void OnEnable(){
		double num = DataControl.GetInstance ().GetMyInfo ().redPacketTicket;
		
        sumRedPacketText.text = (num*0.01) .ToString ();
	}


	public void SetSumRedPacket(float num){
		sumRedPacketText.text = num.ToString ();
	}

	public void Btn_Change2(){
	
	}
	public void Btn_Change5(){

	}
	public void Btn_Change10(){

	}
	public void Btn_Change20(){

	}

	public void Btn_Back()
	{
		//Tweener tweener = ugui.DOScale (new Vector3 (1.2f, 1.2f, 1.2f), 0.5f);
		//Invoke ("Hide", 0.6f);
		//transform.gameObject.SetActive (false);
		Destroy(this.gameObject);
	}
}
