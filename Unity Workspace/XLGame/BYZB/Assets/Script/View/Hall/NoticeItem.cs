using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;

public class NoticeItem : ScrollableCell {
	public Text contents;
	public Image frame;
	BroadCastInfo nDataInfo;
	string data;
	public GameObject notice;
	int length;
	int type;
	void Awake(){
		nDataInfo =(BroadCastInfo) Facade.GetFacade ().data.Get ( FacadeConfig.BROADCAST_MODULE_ID );
	}


	public override void ConfigureCellData ()
	{
		base.ConfigureCellData ();
		notice.SetActive (true);
		if (dataObject != null) {
			data = nDataInfo.GetNoticeList () [(int)dataObject];

			length = data.Length;
			if (data.Length > 15) {//换行之后长度大于15，判断有没有占位符，长度小于15肯定是换行后，直接隐藏公告
				if (data.Substring (1, 15) != "               ") {
					notice.SetActive (false);
				//} else if (data.Substring (data.Length - 1) == "*") {
				//	contents.text = data.Substring (0, length - 1);
				//	
				} else {
					type =int.Parse( data.Substring (0, 1));
					data= data.Remove (0, 1);
					if (type == 0) {//公告
						notice.GetComponent<Image> ().sprite = UIHallTexturers.instans.Notice [0];
					} else {
						notice.GetComponent<Image> ().sprite = UIHallTexturers.instans.Notice [1];
					}

				}
			} else {
				notice.SetActive (false);
			}
			contents.text = data;
			//if ((int)dataObject % 2 == 0) 
			//	frame.gameObject.SetActive (false);
			// else
			//	frame.gameObject.SetActive (true);
			}
	}
}
