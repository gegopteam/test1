using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ExpressionPanel : MonoBehaviour {

	public InputField input;
	// Use this for initialization
	void Awake(){
		
	}
	public void ExpressionClick(GameObject name){
		input.text =input.text+ name.name;
		gameObject.SetActive (false);
	}
}
