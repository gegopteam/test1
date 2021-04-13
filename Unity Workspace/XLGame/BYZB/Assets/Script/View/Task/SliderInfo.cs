using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;

public class SliderInfo : ScrollableCell
{

	public Text activeValue;
	public Text taskName;
	public Slider currentSlider;
	public Text maxValue;
	public Text nowValue;
	public int taskId;
	public Button Done;
	public Image buttonImage;
	private bool isChange = true;
	public bool isTrue = true;
	public GameObject effect;
	private FiEverydayTaskDetial data;

	// Use this for initialization
	void Awake ()
	{
		activeValue = transform.Find ("active").Find ("activeValue_Text").GetComponent<Text> ();
		taskName = transform.Find ("taskName_Text").GetComponent<Text> ();
		currentSlider = transform.GetComponentInChildren<Slider> ();
		maxValue = currentSlider.transform.Find ("maxValue_Text").GetComponent<Text> ();
		nowValue = currentSlider.transform.Find ("nowValue_Text").GetComponent<Text> ();
		Done = transform.Find ("DoButton").GetComponent<Button> ();
		buttonImage = Done.gameObject.GetComponent<Image> ();
		UITask.SendRequestEvent += Send;
	}

	void Send (int Id)
	{
		if (Id == taskId) {
			Facade.GetFacade ().message.task.SendEveryDayTaskRequest (taskId);
		}
	}

	public void SetChange (bool bChange)
	{
		isChange = bChange;
	}

	// Update is called once per frame
	void Update ()
	{
		//if (currentSlider.value == 1 && isChange) {
		//	buttonImage.sprite = Resources.Load ("Ranking/领取", typeof(Sprite))as Sprite;
		//	isChange = false;
		//}
	}

	void OnDestroy ()
	{
		UITask.SendRequestEvent -= Send;
	}

	void Start ()
	{
		Done.onClick.AddListener (ClickCallBack);
		
	}

	public override void ConfigureCellData ()
	{
		base.ConfigureCellData ();
		if (dataObject != null)
			data = UITask.recList [(int)dataObject];
		taskId = data.taskId;
		var taskInfo = TaskControl.getTaskInfo (taskId);
		taskName.text = taskInfo.taskName;
		activeValue.text = taskInfo.activeValue.ToString ();
		maxValue.text = taskInfo.maxValue.ToString ();
		nowValue.text = data.progress.ToString ();
		currentSlider.value = data.progress / float.Parse (maxValue.text);
		buttonImage.sprite = UIHallTexturers.instans.Ranking [1];
		buttonImage.SetNativeSize ();
		Done.interactable = true;
		if (currentSlider.value == 1) {
			buttonImage.sprite = UIHallTexturers.instans.Ranking [9];
			Done.interactable = true;
		}
		if (int.Parse (nowValue.text) == -1) {
			nowValue.text = maxValue.text;
			currentSlider.value = 1;
			buttonImage.sprite = UIHallTexturers.instans.Ranking [0];
			buttonImage.SetNativeSize ();
			//buttonImage.transform.localPosition =new Vector3(420,0.8f);
			buttonImage.GetComponent <RectTransform> ().localPosition = new Vector3 (420, 0.8f, 0);
			buttonImage.GetComponent <RectTransform> ().sizeDelta = new Vector2 (57.4f, 55.4f);
			Done.interactable = false;
		}
	}

	void ClickCallBack ()
	{
		//获取当前的活跃度，改变count
		//获取当前的ID，发送事件
		//Debug.Log (go.GetComponent<Image> ().sprite.ToString());
		AudioManager._instance.PlayEffectClip (AudioManager.ui_click);
		string currentImageName = Done.gameObject.GetComponent<Image> ().sprite.name;
		switch (currentImageName) {
		case "领取":
			UITask.instance.CompleteTask = this;
			UITask.AlreadyFinsh (taskId);
			break;
		case "去完成":
			string name = Done.gameObject.transform.parent.name;
			Debug.Log ("name" + name);
			if (taskId == 7) {
				GoMouthWindow ();
			} else {
				GoFinishButton ();
			}//后期加参数str判断进入哪个场次
			break;
		}
	}
	/*public void getButton(){
		effect.SetActive (false);
		taskId = Done.gameObject.transform.parent.GetComponent<SliderInfo> ().taskId;
		Done.gameObject.transform.parent.GetComponent<SliderInfo> ().isTrue = false;
		UITask.count = int.Parse (Done.gameObject.transform.parent.GetComponent<SliderInfo> ().activeValue.text) / 10;

	}*/


	void GoMouthWindow ()
	{
		UITask.instance.gameObject.transform.gameObject.SetActive (false);
		string path = "Window/MouthCardWindow";
		GameObject WindowClone = AppControl.OpenWindow (path);
		WindowClone.SetActive (true);

	}

	void GoFinishButton ()
	{
		UITask.instance.gameObject.transform.gameObject.SetActive (false);
		UIHallObjects.GetInstance ().StartGame ();
		//点击去完成则跳入当前任务的场景,通过传递的str来判定进入那个场景
	}
		
}
