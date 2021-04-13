using System;
using UnityEngine;
using UnityEngine.UI;
using AssemblyCSharp;

public class UISellWindow:MonoBehaviour
{
	
	int mPropertyId = 0;

	public Image ToolIcon;

	public Text TxtInputCount;

	public Text TxtToolName;

	public Text TxtSellCost;

	int mSellCount = 1;

	int mUnitPrice = 0;

	public UISellWindow ()
	{
		
	}

	public void OnSelectMaxCount()
	{
		Debug.LogError ( "-----------OnSelectMaxCount------------" );
		BackpackInfo nInfo =  (BackpackInfo)Facade.GetFacade ().data.Get ( FacadeConfig.BACKPACK_MODULE_ID );
		FiBackpackProperty nProp = nInfo.Get ( mPropertyId );
		if (nProp != null) {
			mSellCount = nProp.count;
			TxtInputCount.text = mSellCount.ToString();
		}
		updateSellPrice ();
	}

	void updateSellPrice()
	{
		int nTotalPrice = mSellCount* mUnitPrice;
		if (nTotalPrice > 10000) {
			TxtSellCost.text = nTotalPrice / 10000 + "萬";
		}else{
			TxtSellCost.text = nTotalPrice.ToString();
		}
	}

	public void DoInit( int nPropertyId )
	{
		mPropertyId = nPropertyId;
		mUnitPrice = FiPropertyType.GetSellCost ( nPropertyId );
	}

	void Start()
	{
		if (mPropertyId != 0) {
			//string nPath = FiPropertyType.GetToolPath ( mPropertyId );
			TxtToolName.text = FiPropertyType.GetToolName ( mPropertyId );
		}
		TxtInputCount.text = mSellCount.ToString();
		updateSellPrice ();
		UIColseManage.instance.ShowUI (this.gameObject);
	}

	public void SetIcon( Sprite nTartgetImage )
	{
		ToolIcon.sprite = nTartgetImage;
	}

	public void OnAddCount()
	{
		//Debug.LogError ("-----OnAddCount------");
		mSellCount++;
		BackpackInfo  nBackInfo = (BackpackInfo) Facade.GetFacade().data.Get( FacadeConfig.BACKPACK_MODULE_ID );
		FiBackpackProperty nProp = nBackInfo.Get ( mPropertyId );
		if (mSellCount > nProp.count) {
			mSellCount = nProp.count;
			return;
		}
		TxtInputCount.text = mSellCount.ToString();
		updateSellPrice ();
	}


	public void OnReduceCount()
	{
		//Debug.LogError ("-----OnReduceCount------");
		mSellCount--;
		if (mSellCount <= 0) {
			mSellCount = 1;
			return;
		}
		TxtInputCount.text = mSellCount.ToString();
		updateSellPrice ();
	}

	public void OnSell()
	{
//		Debug.LogError ("-----OnSell------");
		FiProperty nSellData = new FiProperty ();
		nSellData.value = mSellCount;
		nSellData.type = mPropertyId;
		Facade.GetFacade ().message.backpack.SendSellRequest ( nSellData );
		OnClose ();
	}

	public void OnClose()
	{
//		Debug.LogError ("-----OnClose------");

		UIColseManage.instance.CloseUI ();
	}

}

