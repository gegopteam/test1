using System;
using UnityEngine;
using UnityEngine.UI;

namespace AssemblyCSharp
{
	public class UiCountDownTip : MonoBehaviour
	{
		public Text txtCountDown;

		float nDeltaTime = 0;

		public UiCountDownTip ()
		{
		}

		public void OnButtonClick()
		{
			MyInfo nInfo =(MyInfo) Facade.GetFacade ().data.Get ( FacadeConfig.USERINFO_MODULE_ID );
			int nDayIndex = Math.Abs ( nInfo.sailDay );
			if (nDayIndex == 1 || nDayIndex == 2 ) {
				StartGiftManager.OpenCountDownWindow ( nDayIndex + 1 );
			}
		}

		void Update () {

			nDeltaTime += Time.deltaTime;

			if (nDeltaTime < 0.5) {
				return;
			}
			//	Debug.LogError ( nDeltaTime + "/" +Time.deltaTime);
			nDeltaTime = 0;

			System.DateTime nNow = System.DateTime.Now;

			int nRemianSecond = 86400 -  (nNow.Hour * 3600 + nNow.Minute * 60 + nNow.Second);

			int nHours = nRemianSecond/3600;
			int nMinutes =(nRemianSecond - nHours * 3600 )/60;
			int nSecond = nRemianSecond - nHours * 3600 - nMinutes * 60;

			string nStrHour = (nHours < 10) ? "0" + nHours : nHours.ToString();
			string nStrMinute = (nMinutes < 10) ? "0" + nMinutes : nMinutes.ToString();
			string nStrSecond =(nSecond < 10) ? "0" + nSecond : nSecond.ToString();
			txtCountDown.text = nStrHour + ":" + nStrMinute + ":" + nStrSecond;
		}

	}
}

