using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using AssemblyCSharp;

public class BackpackInfo : IDataProxy
{
	private List<FiBackpackProperty> mBackpackList = null;

	private IData rcvData = null;

	private bool updated = false;
	public BackpackInfo ()
	{
		Init ();
	}

	public List<FiBackpackProperty> getInfoArray ()
	{
		if (mBackpackList == null)
			return null;
		List< FiBackpackProperty > nResultInfo = new List<FiBackpackProperty> ();
		foreach (FiBackpackProperty nSingle in mBackpackList) {
			//Debug.LogError ( "-------FiBackpackProperty---------" + nSingle.name + " / " + nSingle.count + " / " + nSingle.description );
			if (nSingle.id != 0 && nSingle.type != 0 && nSingle.count > 0 && (nSingle.id<6000 || nSingle.id>7000) ) {
				nResultInfo.Add (nSingle);
			}
		}
		return nResultInfo;
	}

	void VerifyExsitPackage ()
	{
		if (mBackpackList != null) {
			for (int i = 0; i < mBackpackList.Count; i++) {
				FiBackpackProperty nEntity = mBackpackList[i];

				Debug.Log("  整理背包物品  VerifyExsitPackage  " + nEntity.id);
				//如果包含没有打开的鱼雷礼包，那么显示红角标
				if (nEntity.id == FiPropertyType.GIFT_TORPEDO && nEntity.count > 0)
				{
					SetUpdate(true);
					return;
				}
				//包含新手礼包
				if (nEntity.id >= FiPropertyType.GIFT_VIP1 && nEntity.id <= FiPropertyType.GIFT_VIP9 && nEntity.count > 0)
				{
					SetUpdate(true);
					return;
				}
			}
		}
		SetUpdate (false);
	}

	public void SetArrayData (List<FiBackpackProperty> nDataIn)
	{
		mBackpackList = nDataIn;
		VerifyExsitPackage ();
	}

	public void OnAddData (int nType, object nData)
	{
		
	}

	public void OnInit ()
	{
		
	}

	public void OnDestroy ()
	{
		Clear ();
	}

	~BackpackInfo ()
	{
		UnInit ();
	}

	private void Init ()
	{
		mBackpackList = new List<FiBackpackProperty> ();
	}

	private void UnInit ()
	{
		Clear ();
		mBackpackList = null;
	}

	public bool isUpdated ()
	{
		return updated;
	}


	public void SetRcv (object obj)
	{
		rcvData = (IData)obj;
	}

	public void OpenRcvInfo ()
	{
		if (null == rcvData)
			return;
		foreach (FiBackpackProperty info in mBackpackList) {
			rcvData.RcvInfo (info);
		}

		rcvData.RcvInfo (null);
		return;
	}

	public void SetUpdate (bool nUpdated)
	{
		updated = nUpdated;
	}

	public void Add (int id, int value)
	{
		FiBackpackProperty infoGet = Get (id);
		//SetUpdate ( true );
		if (null == infoGet) {
			Tool.Log ("11添加道具 id=" + id);
			FiBackpackProperty info = FiUtil.CreateBagTool (id, value);
			if (info != null)
				mBackpackList.Add (info);
		} else {
			Tool.Log ("22添加道具 id=" + id);
			infoGet.count += value;
		}
		VerifyExsitPackage ();
		//Debug.LogError ("-----------------------===>" + infoGet.count );
	}


	public void SetProperty (int nPropertyType, int propType, int nCount)
	{
		FiBackpackProperty infoGet = Get (nPropertyType);
		SetUpdate (true);
		if (null == infoGet) {
			Tool.Log ("11添加道具 id:");
			FiBackpackProperty nBackInfo = new FiBackpackProperty ();
			nBackInfo.id = nPropertyType;
			nBackInfo.count = nCount;
			mBackpackList.Add (nBackInfo);
		} else {
			Tool.Log ("22添加道具 id:" + nPropertyType);
			Debug.LogError ("SetProperty protype = " + propType);

			infoGet.type = propType;
			infoGet.count = nCount;
		}
	}

	public void Add (FiBackpackProperty info)
	{
		if (null == info)
			return;
		SetUpdate (true);
		Tool.Log ("添加道具");

		FiBackpackProperty infoGet = Get (info.id);
		if (null == infoGet) {
			Tool.Log ("11添加道具 id~" + info.id);
			mBackpackList.Add (new FiBackpackProperty (info));
		} else {
			Tool.Log ("22添加道具 id~" + info.id);
			if (info.id >= FiPropertyType.CANNON_VIP0 && info.id <= FiPropertyType.CANNON_VIP9) {
				//是炮台,不作任何操作
			} else {
				infoGet.count += info.count;
			}
		}
		VerifyExsitPackage ();

	}

	public void Delete (int id, int count)
	{
		foreach (FiBackpackProperty info in mBackpackList) {
			if (id == info.id) {
				info.count -= count;
				if (info.count < 0) {
					info.count = 0;
					Debug.LogError ("-----------FiBackpackProperty erro----------info.count == " + info.count);
				}
				break;
			}
		}
		VerifyExsitPackage ();
	}

	public void Replace (int nUnitId, long nCount)
	{
		FiBackpackProperty infoGet = Get (nUnitId);
		if (null == infoGet) {
			infoGet.count = (int)nCount;
			mBackpackList.Add (new FiBackpackProperty (infoGet));
		} else {
			infoGet.count = (int)nCount;
		}
		VerifyExsitPackage ();
	}

	void Remove (int id)
	{
		for (int i = mBackpackList.Count - 1; i >= 0; i--) {
			if (id == mBackpackList [i].id) {
				mBackpackList [i].count = 0;
				//mBackpackList.Remove (mBackpackList [i]);
			}
		}
	}

	public FiBackpackProperty Get (int id)
	{
		FiBackpackProperty infoGet = null;
		foreach (FiBackpackProperty info in mBackpackList) {
			if (id == info.id) {
				infoGet = info;
				break;
			}
		}

		return infoGet;
	}
    public int GetTorperdoCountNum()
    {
         int torpedoAllCount = 0;

        foreach (FiBackpackProperty info in mBackpackList)
        {
            if (info.id >= FiPropertyType.TORPEDO_MINI && info.id <= FiPropertyType.TORPEDO_PK)
            {
                torpedoAllCount += info.count;
//                Debug.LogError("GetTorperdoCountNum torpedoAllCount = " + torpedoAllCount);
            }
        }
        return torpedoAllCount;
    }
	public void Clear ()
	{
		SetUpdate (false);
		mBackpackList.Clear ();
	}

}
