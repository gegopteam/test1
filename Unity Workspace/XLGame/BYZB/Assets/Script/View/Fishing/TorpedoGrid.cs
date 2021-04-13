using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TorpedoGrid : MonoBehaviour {

	public Text numText;
	int count=10;
	// Use this for initialization  
	void Start () {
        //InvokeRepeating ("CountDown", 1f, 1f);  //改成了不要等待直接发射
        Invoke("ForceFireTorpedo", 0.2f);
	}
	
	void CountDown(){
		count--;

		if (count < 0) {
			count = 0;
		} else if (count == 0) {
			PrefabManager._instance.GetLocalGun ().ForceFireTorpedo ();
		}
		numText.text = count.ToString ();
	}

    void ForceFireTorpedo(){
        PrefabManager._instance.GetLocalGun().ForceFireTorpedo();
    }
}
