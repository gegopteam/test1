using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// User interface frist.首充特惠
/// </summary>

public class UiFrist : MonoBehaviour {
	[SerializeField]
	private GameObject frist;
	[SerializeField]
	private Camera fristCamera;
	[SerializeField]
	private Canvas mainCanvas;

	void Awake()
	{
        
		
        if(SceneManager.GetActiveScene().name=="Fishing"){
            frist = GameObject.FindGameObjectWithTag(TagManager.uiCamera);
            fristCamera = frist.GetComponent<Camera>();
        }else{
            frist = GameObject.FindGameObjectWithTag("MainCamera");
            fristCamera = frist.GetComponent<Camera>();
        }
		
		mainCanvas = transform.GetComponentInChildren<Canvas> ();
		mainCanvas.worldCamera = fristCamera;
		
	}

    public GameObject storePrefab;

	public void OnExit()
	{
		AudioManager._instance.PlayEffectClip(AudioManager.effect_closePanel);
        Destroy(this.gameObject);
        Debug.LogError("OnExit");
        if(GameController._instance!=null){
            GameObject temp = GameObject.Instantiate(storePrefab);
            temp.GetComponent<UIStore>().CoinButton();
        }
		
		//transform.gameObject.SetActive (false);
	}

	public void BuyButton()
	{
		//完成首充以后，大厅界面UI界面改变，将首充特惠去除
		//方案：隐藏当前，显示只有两个的
		Destroy (this.gameObject);
		// UIToPay.OpenThirdPartPay ( 6 );// OpenUIToPay(ProductID.Pack_Preference_CNY_6);
		// UIToPay.OpenApplePay ( ProductID.Pack_Preference_CNY_6 );

	}
}
