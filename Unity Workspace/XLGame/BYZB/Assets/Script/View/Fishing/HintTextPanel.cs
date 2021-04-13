using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class HintTextPanel : MonoBehaviour {

	public static HintTextPanel _instance=null;

	public  Image bgImage;
	public Text childText;
	public float lerpDuration = 0.3f;

	bool isShow=false;

	void Awake(){
		if (null == _instance)
			_instance = this;
	}

	// Use this for initialization
	void Start () {
		SetShow (false);
	}
	
	public void SetTextShow(string str,float hideTime=3f,bool lerpFlag=true){
		if (isShow) {
			CancelInvoke ("Hide");//如果a信息显示到中途，b信息也要通过刚方法显示，则替换text内容，重置延长隐藏时间
		} else {
		
		}
		SetShow (true,lerpFlag);
		childText.text = str;
		if (hideTime > 0) {  //如果HideTime<=0，则设置不自动隐藏
			Invoke ("Hide", hideTime);
		}

	}

	public void SetShow(bool toShow,bool useLerp=true){
		isShow = toShow;
		if (toShow) {
			bgImage.enabled = true;
			childText.gameObject.SetActive (true);
			if (useLerp) {
				bgImage.color = new Color (0, 0, 0, 0);
				bgImage.DOColor (Color.white, lerpDuration);
				childText.color = new Color (0, 0, 0, 0);
				childText.DOColor (Color.white, lerpDuration);
			} else {
				bgImage.color = Color.white;
				childText.color = Color.white;
			}
		} else {
			if (useLerp) {
				bgImage.DOFade (0, lerpDuration);
				childText.DOFade (0, lerpDuration);
				Invoke ("TrueHide", lerpDuration + 0.1f);
			} else {
				bgImage.color = new Color (0, 0, 0, 0);
				childText.color = new Color (0, 0, 0, 0);
			}
		}
	}
	void TrueHide(){
		bgImage.enabled = false;
		childText.gameObject.SetActive (false);
	}


	void Hide(){
		SetShow (false);
	}
}
