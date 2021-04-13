using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;
using UnityEngine;


/*
 *  2020/02/29 Joey 判斷用戶間到第幾天
 */

public class DailySignSevenDay : MonoBehaviour {


	public int DayNumber;
	public GameObject sign;
	public GameObject unSign;



	// Use this for initialization
	IEnumerator Start () {
		yield return new WaitForSeconds(0.5f);
		NewSevenDayInfo nSevenDayinfo = (NewSevenDayInfo)Facade.GetFacade().data.Get(FacadeConfig.UI_SEVENPART);
		//Debug.LogError("今天遷到第幾天 = "+ nSevenDayinfo.nuserDay);
		//Debug.LogError("ncurDay = " + nSevenDayinfo.ncurDay);

		if (UISevenSign.have2minus)
		{
			if ( (nSevenDayinfo.nuserDay-1) >= DayNumber)
			{
				sign.SetActive(true);
				unSign.SetActive(false);
			}
			else
			{
				sign.SetActive(false);
				unSign.SetActive(true);
			}
		}
		else
        {
			if (nSevenDayinfo.nuserDay >= DayNumber)
			{
				sign.SetActive(true);
				unSign.SetActive(false);
			}
			else
			{
				sign.SetActive(false);
				unSign.SetActive(true);
			}
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
