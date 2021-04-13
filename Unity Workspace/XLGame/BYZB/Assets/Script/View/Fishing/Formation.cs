using UnityEngine;
using System.Collections;

public class Formation : MonoBehaviour {

	public int maxCount=5;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public Vector3 GetOffset(int index) //根据鱼的id编号，获取和id为0的鱼的位移补偿
	{
		Transform tempTrans = transform.Find (index.ToString ());
		if (tempTrans == null) {
			Debug.LogError ("Error:Can't find formation:" + index);
		}
		return tempTrans.localPosition ;
	}

    public float GetScale(int index){
        Transform tempTrans = transform.Find(index.ToString());
        if (tempTrans == null)
        {
            Debug.LogError("Error:Can't find formation:" + index);
        }
        return tempTrans.localScale.x;
    }
}
