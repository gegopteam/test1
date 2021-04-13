using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/// <summary>
/// Master ready players.房主信息UI管理
/// </summary>

public class MasterReadyPlayers : MonoBehaviour {
	public Image headImage;
	public Text playerName;
	public Image cannonImage;
	public Image readyImage;

	public Transform waitImage;
	public Transform show;

	public int useId;

	public bool isActive = false;

	public bool isPrepared = false;

	// Use this for initialization
	void Awake () {
		Init ();
	}

	void Init ()
	{
		show = transform.Find ("Show");
		headImage = show.Find ("HeadImage").GetComponent<Image> ();
		playerName = show.Find ("PlayerName").GetComponent<Text> ();
		cannonImage = show.Find ("CannonImage").Find("Cannon").GetComponent<Image>();
		readyImage = show.Find("ReadyImage").GetComponent<Image>();
		waitImage = transform.Find ("NoShow");
	}

	// Update is called once per frame
	void Update () {
		
	}
}
