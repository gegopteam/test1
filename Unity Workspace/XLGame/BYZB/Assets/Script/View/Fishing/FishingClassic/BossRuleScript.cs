/***
 *
 *   Title: Boss规则匹配规则
 *
 *   Description: 挂载在 BossRuleCanvas 上
 *
 *   Author: bw
 *
 *   Date: 2019.2.13
 *
 *   Modify: 只是用来关闭界面
 * 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossRuleScript : MonoBehaviour
{

	Button knowBtn;

	void Start ()
	{
		knowBtn = transform.Find ("ButtonControl/OrangeButton").GetComponent <Button> ();
		knowBtn.onClick.RemoveAllListeners ();
		knowBtn.onClick.AddListener (delegate {
			Destroy (gameObject);
		});
	}
}
