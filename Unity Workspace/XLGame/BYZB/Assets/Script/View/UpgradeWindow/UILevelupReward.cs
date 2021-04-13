using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using AssemblyCSharp;
using System.Collections;

public class UILevelupReward : MonoBehaviour {

	public Text level;
	public Text goldMount;

	public void SetRewardData(int taskLevel, long goldGet)
	{
		level.text = "領取" + taskLevel + "級金幣禮包";
		goldMount.text = "獲得" + goldGet + "金幣獎勵";
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnDestroy()
    {
		Debug.LogError(" -------- UILevelupReward -------- OnDestroy -------- ");
		UIHallCore.isNeedToUpdate = true;
		Debug.Log("UIHallCore.isNeedToUpdate = " + UIHallCore.isNeedToUpdate);
	}
}
