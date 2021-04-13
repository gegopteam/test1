using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using AssemblyCSharp;

public class ScreenManager : MonoBehaviour
{

	public static  float leftBorder;
	public static  float rightBorder;
	public static  float topBorder;
	public static  float downBorder;
	//	private float width;
	//	private float height;

	public static CanvasScaler uiScaler;
	public static  Camera uiCamera;

	static  DOTweenAnimation shakeAnim;

	void Awake ()
	{
		SetBasicValues ();
		uiScaler = GameObject.FindWithTag (TagManager.uiCamera).transform.parent.GetComponent<CanvasScaler> ();
		uiCamera = GameObject.FindWithTag (TagManager.uiCamera).GetComponent<Camera> ();
//        if (Facade.GetFacade().config.isIphoneX())
//        {
//            uiScaler.matchWidthOrHeight = 0.3f;
//        }
		shakeAnim = uiCamera.GetComponent<DOTweenAnimation> ();
		//Invoke ("DebugShow", 2f);
		Invoke ("DisableMainCamera", 2f);
	}

	void SetBasicValues ()
	{
		Vector3 cornerPos = Camera.main.ViewportToWorldPoint (new Vector3 (1f, 1f,
			                   Mathf.Abs (-Camera.main.transform.position.z)));
		leftBorder = Camera.main.transform.position.x - (cornerPos.x - Camera.main.transform.position.x);
		rightBorder = cornerPos.x;
		topBorder = cornerPos.y;
		downBorder = Camera.main.transform.position.y - (cornerPos.y - Camera.main.transform.position.y);
		//Invoke ("DisableMainCamera",5f);
	}

	void DisableMainCamera ()
	{
		Destroy (Camera.main.gameObject);
	}

	void DebugShow ()
	{
		HintText._instance.ShowHint ("上下左右边界值:" + topBorder.ToString ("f2") + "," + downBorder.ToString ("f2") +
		"," + leftBorder.ToString ("f2") + "," + rightBorder.ToString ("f2"));
	}

	public static bool IsInScreen (Vector3 pos)
	{
		if (pos.x > rightBorder || pos.x < leftBorder
		    || pos.y < downBorder || pos.y > topBorder) {
			return false;
		}
		return true;

	}

	public static bool IsAwayFromScreen (Vector3 pos, float awayDistance = 2)
	{
		if (pos.x > rightBorder) {
			return (pos.x - rightBorder) > awayDistance ? true : false;
		} else if (pos.x < leftBorder) {
			return(leftBorder - pos.x) > awayDistance ? true : false;
		} else if (pos.y > topBorder) {
			return(pos.y - topBorder) > awayDistance ? true : false;
		} else if (pos.y < downBorder) {
			return (downBorder - pos.y) > awayDistance ? true : false;
		}
		return false;

	}

	public static void ShakeCamera ()
	{
		shakeAnim.DORestart ();
		shakeAnim.DOPlay ();
	}

	public static Vector3 WorldToUIPos (Vector3 pos)
	{
  

		float resolutionX = uiScaler.referenceResolution.x;  
		float resolutionY = uiScaler.referenceResolution.y;  

		Vector3 viewportPos = GameObject.FindWithTag (TagManager.uiCamera).GetComponent<Camera> ().WorldToViewportPoint (pos);  

		Vector3 uiPos = new Vector3 (viewportPos.x * resolutionX - resolutionX * 0.5f,  
			                viewportPos.y * resolutionY - resolutionY * 0.5f, 0);  

		return uiPos;  
	}

	public static Vector3 UIToWorldPos (Vector3 uiPos)
	{
		uiPos = uiCamera.WorldToScreenPoint (uiPos);
		uiPos.z = 0f;
		uiPos = Camera.main.ScreenToWorldPoint (uiPos);
		return uiPos;
	}


	public static  bool IsPointOverUI (int fingerID)
	{
		return EventSystem.current.IsPointerOverGameObject (fingerID);
	}
}
