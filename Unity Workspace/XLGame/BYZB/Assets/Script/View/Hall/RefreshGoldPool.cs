using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using UnityEngine.UI;

public class RefreshGoldPool : MonoBehaviour,IUiMediator
{

	DragonCardInfo shenLongTableData;
	public GameObject GoldPool;
	public GameObject BossPool;
	float timeSpend = 0;

	void Awake ()
	{
		Facade.GetFacade ().ui.Add (FacadeConfig.UI_COINNUM, this);
	}

	void Start ()
	{
		Facade.GetFacade ().message.fishCommom.SendLongPoolRewardRequest (0);
		shenLongTableData = (DragonCardInfo)Facade.GetFacade ().data.Get (FacadeConfig.UI_DRAGONCARD);
		//Debug.Log("shenLongData.LongTimeStart "+shenLongTableData.LongTime);
		if (shenLongTableData.GoldPool.Count != 0) {
			transform.Find ("Text").GetComponent<Text> ().text = shenLongTableData.GoldPool [0].ToString ("N0");

			GoldPool.transform.Find ("Text").GetComponent<Text> ().text = shenLongTableData.GoldPool [1].ToString ("N0");

			BossPool.transform.Find ("Text").GetComponent<Text> ().text = shenLongTableData.GoldPool [2].ToString ("N0");
		}
		StartCoroutine (GetRefreshGoldPoolLocal ());
		StartCoroutine (GetRefershGoldPoolServer ());
	}
	
	// Update is called once per frame
	void Update ()
	{
		//进度倒计时
		timeSpend += Time.deltaTime;
		if (timeSpend >= 1) {
			shenLongTableData.LongTime = shenLongTableData.LongTime - 1;
			//Debug.Log(shenLongTableData.LongTime);
			timeSpend = 0;
		}
	}
    //long gold1 = 0;
    //long gold2 = 0;
    //long gold3 = 0;
    long gold4 = 0;
    long godl5 = 0;
    long gold6 = 0;
    bool numboll = true;
	//本地假数据刷新
	IEnumerator GetRefreshGoldPoolLocal ()
	{
		yield return new WaitForSeconds (0.5f);
		int rangNum1 = Random.Range (1000, 5000);
		int rangNum2 = Random.Range (1000, 5000);
		int rangNum3 = Random.Range (1000, 5000);
		int numj = Random.Range (1, 100);
		//if (numj < 3) {
		//	rangNum1 = -rangNum1;
		//	rangNum2 = -rangNum2;
		//	rangNum3 = -rangNum3;
		//}
       // Debug.Log(gold1+"gold1"+gold2+"gold2"+gold3+"gold3");

      
		if (shenLongTableData.GoldPool.Count != 0) {
            long gold1 = (long)shenLongTableData.GoldPool[0] + (long)rangNum1;
            long gold2 = (long)shenLongTableData.GoldPool[1] + (long)rangNum2;
            long gold3 = (long)shenLongTableData.GoldPool[2] + (long)rangNum3;
            if (numboll)
            {   gold4 = gold4 + gold1-(long)shenLongTableData.GoldPool[0];; //(long)rangNum1+gold1;
                godl5 = godl5 + gold2-(long)shenLongTableData.GoldPool[1];; //(long)rangNum2;
                gold6 = gold6 + gold3-(long)shenLongTableData.GoldPool[2];;//(long)rangNum3;
                numboll = false;
            }
            else
            {
                gold4 = gold4 +  (long)rangNum1-(long)shenLongTableData.GoldPool[0]; //(long)rangNum1+gold1;
                godl5 = godl5 +  (long)rangNum2-(long)shenLongTableData.GoldPool[1]; //(long)rangNum2;
                gold6 = gold6 +  (long)rangNum3-(long)shenLongTableData.GoldPool[2];//(long)rangNum3;
                numboll = false;
            }
            gold4 = gold4 + gold1; //(long)rangNum1+gold1;
            godl5 = godl5 + gold2; //(long)rangNum2;
            gold6 = gold6 + gold3;//(long)rangNum3;

            transform.Find ("Text").GetComponent<Text> ().text = (gold4).ToString ("N0");
            GoldPool.transform.Find ("Text").GetComponent<Text> ().text = (godl5).ToString ("N0");
            BossPool.transform.Find ("Text").GetComponent<Text> ().text = (gold6).ToString ("N0");
		}


        //gold4 = gold4 + (long)rangNum1;
        //godl5 = godl5 + (long)rangNum2;
        //gold6 = gold6 + (long)rangNum3;

		StartCoroutine (GetRefreshGoldPoolLocal ());
	}

	IEnumerator GetRefershGoldPoolServer ()
	{
		yield return new WaitForSeconds (60.3f);
		Facade.GetFacade ().message.fishCommom.SendLongPoolRewardRequest (0);
		StartCoroutine (GetRefershGoldPoolServer ());
	}

	public void OnRecvData (int nType, object nData)
	{
		if (UIHallCore.Instance == null) {
			return;
		}
		if (nType == FiEventType.RECV_XL_TURNTABLEGETPOOL_RESPONSE && shenLongTableData.GoldPool.Count != 0) {
    
			//Debug.Log ("奖池的长度" + shenLongTableData.GoldPool.Count);
            //Debug.Log("奖池的11111" + shenLongTableData.GoldPool[0]);
            //Debug.Log("奖池的22222" + shenLongTableData.GoldPool[1]);
            //Debug.Log("奖池的33333" + shenLongTableData.GoldPool[2]);

            gold4 = 0;
            godl5 = 0;
            gold6 = 0;
            numboll = true;
            transform.Find("Text").GetComponent<Text>().text = shenLongTableData.GoldPool[0].ToString("N0");

            GoldPool.transform.Find("Text").GetComponent<Text>().text =shenLongTableData.GoldPool[1].ToString("N0");

            BossPool.transform.Find("Text").GetComponent<Text>().text = shenLongTableData.GoldPool[2].ToString("N0");//shenLongTableData.GoldPool[2].ToString();
		}
	}

	public  void OnInit ()
	{

	}

	public void OnRelease ()
	{

	}
}
