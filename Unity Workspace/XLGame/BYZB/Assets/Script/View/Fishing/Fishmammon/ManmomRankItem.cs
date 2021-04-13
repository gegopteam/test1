using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AssemblyCSharp;
using System.Text;
using System;

public class ManmomRankItem : ScrollableCell
{
	public Text ranknum;
	public Text usename;
	public Text victorytime;
	public Text rewardnum;

	public FiRankInfo data;

	//	public List<FiManmonRankInfo> TestArry = new List<FiManmonRankInfo> ();


	void Awake ()
	{

//		FiManmonRankInfo s = new FiManmonRankInfo ();
//		s.usename = "1";
//		s.wintime = 11;
//		s.rewardnum = 1111;
//		TestArry.Add (s);
//
//		FiManmonRankInfo s1 = new FiManmonRankInfo ();
//		s.usename = "2";
//		s.wintime = 22;
//		s.rewardnum = 2222;
//		TestArry.Add (s1);
//
//		FiManmonRankInfo s2 = new FiManmonRankInfo ();
//		s.usename = "3";
//		s.wintime = 33;
//		s.rewardnum = 3333;
//		TestArry.Add (s2);
//
//		FiManmonRankInfo s3 = new FiManmonRankInfo ();
//		s.usename = "4";
//		s.wintime = 44;
//		s.rewardnum = 4444;
//		TestArry.Add (s3);
//
//		FiManmonRankInfo s4 = new FiManmonRankInfo ();
//		s.usename = "5";
//		s.wintime = 55;
//		s.rewardnum = 5555;
//		TestArry.Add (s4);
//
//		FiManmonRankInfo s5 = new FiManmonRankInfo ();
//		s.usename = "6";
//		s.wintime = 66;
//		s.rewardnum = 6666;
//		TestArry.Add (s5);


	}
	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public override void ConfigureCellData ()
	{
		base.ConfigureCellData ();
		if (dataObject != null) {
			data = UIManmonGameShow.instance.TestArry [(int)dataObject];
		}
			
//		for (int i = 0; i < UIManmonGameShow.instance.TestArry.Count; i++) {
//			Debug.LogError ("ssssssssssssss" + UIManmonGameShow.instance.TestArry [i].nickname);
//		}
//
//		Debug.LogError ("111111111111111111111sss" + (int)dataObject + data.nickname + "jiangli" + data.gold);
		if (data != null) {
			usename.text = Tool.GetName (data.nickname, 5);

			victorytime.text = data.gold.ToString ();

			ranknum.text = ((int)dataObject + 1).ToString ();

			rewardnum.text = UIManmonGameShow.instance.rankcout [((int)(dataObject))].value.ToString ();
		}
	}
}
