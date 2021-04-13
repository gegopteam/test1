using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using UnityEngine.UI;

public class UIBarbette : MonoBehaviour
{
	public  Sprite[] barbetteSprite;
	public  Sprite[] redSprite;
	public GameObject day;
	public GameObject function;
	Image barbetteImage;
	Text titleText;
	Text describeText;
	Image redImage;
	Button acceptBtn;
	bool isBattery = false;


	public static UIBarbette Instance;

	void Awake ()
	{
		Instance = this;
		Init ();
	}

	void Start ()
	{
		try
		{
			if (GetComponent<Canvas>().worldCamera == null)
			{
				GetComponent<Canvas>().worldCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
			}
		}
		catch {
			Debug.LogError(" MainCamera ");
        }
		

		StartCoroutine (ReceiveBarbette ());
	}

	void Init ()
	{
		barbetteImage = transform.Find ("gunplat/gunImg").GetComponent <Image> ();
		titleText = transform.Find ("gunplat/TitleImage/TitleText").GetComponent <Text> ();
		Debug.LogError ("titleText.name = " + titleText.name);
		describeText = transform.Find ("gunplat/DescribeImage/DescribeText").GetComponent <Text> ();
		redImage = transform.Find ("gunplat/red/image").GetComponent <Image> ();
		acceptBtn = transform.Find ("SureButton").GetComponent <Button> ();
		Debug.LogError ("acceptBtn.name = " + acceptBtn.name);

		acceptBtn.onClick.RemoveAllListeners ();
		acceptBtn.onClick.AddListener (ReceiveButton);
	}

	/// <summary>
	/// 设置炮台充值成功显示图片
	/// </summary>
	/// <param name="index">Index.</param>
	public void SetBarbetteImage (int index)
	{
		switch (index) {
		case 1:
			titleText.text = "青銅砲座";
			describeText.text = "炮彈秒速每秒6發";
			break;
		case 2:
			titleText.text = "白銀砲座";
			describeText.text = "炮彈秒速每秒7發";
			break;
		case 3:
			titleText.text = "黃金砲座";
			describeText.text = "炮彈秒速每秒8發";
			break;

		case 6001:
				titleText.text = "加農火炮";
				describeText.text = "";
				index = 4;
				break;
		case 6002:
				titleText.text = "冰凍新星";
				describeText.text = "";
				index = 5;
				break;
		case 6003:
				titleText.text = "綠野仙踪";
				describeText.text = "";
				index = 6;
				break;
		case 6004:
				titleText.text = "蒼鷹之眸";
				describeText.text = "";
				index = 7;
				break;
		case 6005:
				titleText.text = "鳳凰之影";
				describeText.text = "";
				index = 8;
				break;
		case 6006:
				titleText.text = "烈焰風暴";
				describeText.text = "";
				index = 9;
				break;
		case 6007:
				titleText.text = "雷霆之怒";
				describeText.text = "";
				index = 10;
				break;
		case 6008:
				titleText.text = "天使之翼";
				describeText.text = "";
				index = 11;
				break;
		case 6009:
				titleText.text = "神聖之光";
				describeText.text = "";
				index = 12;
				break;
			default:
			break;
		}
		barbetteImage.sprite = barbetteSprite [index - 1];
		if (index < 4) {
			redImage.sprite = redSprite[index - 1];
			isBattery = false;
		}
			
		else {
			day.SetActive(false);
			redImage.gameObject.SetActive(false);
			function.SetActive(false);
			isBattery = true;
			//UIBatteryBuy.instance.ClosePanel();
		}
			
	}

	IEnumerator ReceiveBarbette ()
	{
		yield return new WaitForSeconds (4f);
		ReceiveButton ();
	}

	/// <summary>
	/// 接受奖励按钮
	/// </summary>
	void ReceiveButton ()
	{
		Debug.Log("UIBarbette ReceiveButton");
        if(isBattery)
		    UIBatteryBuy._Instance.OnExit();
		Destroy (gameObject);
	}
		
}
