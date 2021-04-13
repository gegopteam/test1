using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;

public class VIPInfo : MonoBehaviour {
	Image[] VIPImage;
	Text[] vipInfo;
	public Image point;
	public Image gunImage;
	public Text gunName;
	public Text VIPLevel;
	public Text gunNames;
	[HideInInspector]
	public int Level;
	// Use this for initialization

	void Awake(){
	}
	void Start () {

	}
	public void InitalInfo (int level)
	{
		Level = level;
		VIPImage = new Image[5];
		vipInfo = new Text[5];
		for (int i = 0; i < vipInfo.Length; i++) {
			VIPImage [i] = VipSliderContrl.CloneGameObject (point.gameObject).GetComponent<Image> ();
			vipInfo [i] = VIPImage [i].GetComponentInChildren<Text> ();
			VIPImage [i].gameObject.SetActive (false);
		}
		//这个六级是写死的。后面接活性等级
		string[] str = StoreRectHelper.GetVipInfo (level);
		for (int i = 0; i < str.Length; i++) {
			vipInfo [i].text = str [i];
			VIPImage [i].gameObject.SetActive (true);
		}
		gunName.text= FiPropertyType.GetCannonName (3000+level);//获取名字的函数，从3000开始是0级，3009是VIP9
		gunNames.text=FiPropertyType.GetCannonName (3000+level);
		gunImage.sprite = UIHallTexturers.instans.Vip [9 + level];// Resources.Load<Sprite> ("Image/store/" + FiPropertyType.GetCannonName (3000+level));
		gunImage.SetNativeSize ();
		VIPLevel.text="VIP"+level;//以上这些6都是可变的等级，目前写死为6
	}
}

