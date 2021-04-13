/* author:KinSen
 * Date:2017.06.01
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class UIMsgBox : MonoBehaviour {

	public Text title;
	public Text content;
	public Button[] btnGroup = null;

	public System.Action msgBoxConfirm = null;
	public System.Action msgBoxCancel = null;

	public void OnConfirm()
	{
		if(null!=msgBoxConfirm)
		{
			msgBoxConfirm ();
		}
	}

	public void OnCancel()
	{
		if(null!=msgBoxCancel)
		{
			msgBoxCancel ();
		}
	}

}
