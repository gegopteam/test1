using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class GoldCoinColumn : MonoBehaviour
{
    
	public Transform[] ColumnVector;
	Transform[] keepColumnVector;
	List<GameObject> ColumnObj;
	int count = 0;
	float second = 0f;
	public Sprite GoldCoinRedSprite;
	public Sprite GoldCoinGreenSprite;
	public GameObject GoldCoinColumnSilder;
    RoomInfo myRoomInfo = null;
	public enum GunGoldColumEnum
	{
		Left,
		Right
	}

	public GunGoldColumEnum GUnColumType;

	void Awake ()
	{
        myRoomInfo = DataControl.GetInstance().GetRoomInfo();
	}

	void Start ()
	{
		ColumnObj = new List<GameObject> ();
		keepColumnVector = ColumnVector;
		count = transform.childCount;
	}

	void Update ()
	{
		second += Time.deltaTime;
		if (second >= 2f) {
            
			for (int i = 0; i < transform.childCount; i++) {
				for (int j = 0; j < transform.GetChild (i).childCount; j++) {
					Destroy (transform.GetChild (i).GetChild (j).gameObject);
				}
			}
			second = 0f;
		}
	}

	GameObject goldCoinColumn;
	bool RedOrGreen = false;
	/// <summary>
    /// 生成金币柱
    /// </summary>
    /// <param name="cannonInfo">Cannon info.</param>
    /// <param name="GoldNum">金币数</param>
    /// <param name="enemyReturn">鱼倍数</param>
    /// <param name="gunSeat">炮位置</param>
	public void  BuildGoldCoinColumn (CannonInfo cannonInfo, int GoldNum, int enemyReturn, GunSeat gunSeat)
	{
		//if (gunSeat==GunSeat.LB)
		//{
		//    goldCoinColumn = GameObject.Instantiate(PrefabManager._instance.GoldCoinGridLeftObj, transform);
		//}
		//else if (gunSeat==GunSeat.RB)
		//{
		//    goldCoinColumn = GameObject.Instantiate(PrefabManager._instance.GoldCoinGridRightObj, transform);
		//}
        if (gunSeat == GunSeat.LB || gunSeat == GunSeat.RT) {
            
			goldCoinColumn = GameObject.Instantiate (GoldCoinColumnSilder, transform);
			goldCoinColumn.transform.Find ("ColumNumGreen/image/Text").gameObject.GetComponent<Text> ().text = GoldNum.ToString ();
			if (RedOrGreen) {
				goldCoinColumn.transform.Find ("ColumNumGreen/image").GetComponent<Image> ().sprite = GoldCoinGreenSprite;
			} else {
				goldCoinColumn.transform.Find ("ColumNumGreen/image").GetComponent<Image> ().sprite = GoldCoinRedSprite;
			}
			RedOrGreen = !RedOrGreen;
        } else if (gunSeat == GunSeat.RB || gunSeat == GunSeat.LT) {
			goldCoinColumn = GameObject.Instantiate (GoldCoinColumnSilder, transform);
			goldCoinColumn.transform.Find ("ColumNumGreen/image/Text").gameObject.GetComponent<Text> ().text = GoldNum.ToString ();
			if (RedOrGreen) {
				goldCoinColumn.transform.Find ("ColumNumGreen/image").GetComponent<Image> ().sprite = GoldCoinGreenSprite;
			} else {
				goldCoinColumn.transform.Find ("ColumNumGreen/image").GetComponent<Image> ().sprite = GoldCoinRedSprite;
			}
			RedOrGreen = !RedOrGreen;
		}
		goldCoinColumn.transform.localScale = new Vector3 (1.77f, 1.4f);
		
		ColumnObj.Add (goldCoinColumn);
        switch (myRoomInfo.roomMultiple)
        {
            //新手
            case 0:
                if (GoldNum  <= 2000)
                {
                    goldCoinColumn.GetComponent<Slider>().value = 0.04f;//0.01f代表一层金币
                }
                else if (GoldNum >= 25000)
                {
                    goldCoinColumn.GetComponent<Slider>().value = 0.5f;
                }
                else
                {
                    goldCoinColumn.GetComponent<Slider>().value = Mathf.Round(GoldNum / 500.0f) * 0.01f;
                }
                break;
            case 1:
                {
                    if (GoldNum <= 40000)
                    {
                        goldCoinColumn.GetComponent<Slider>().value = 0.04f;
                    }
                    else if (GoldNum >= 500000)
                    {
                        goldCoinColumn.GetComponent<Slider>().value = 0.5f;
                    }
                    else
                    {
                        goldCoinColumn.GetComponent<Slider>().value = Mathf.Round(GoldNum / 10000.0f) * 0.01f;
                    }
                    break;
                }
            //高级
            case 2:
            case 3:
            case 5:
                {
                    //由鱼倍数改为金币数判断
                    if (GoldNum <= 400000)
                    {
                        goldCoinColumn.GetComponent<Slider>().value = 0.04f;
                    }
                    else if (GoldNum >= 5000000)
                    {
                        goldCoinColumn.GetComponent<Slider>().value = 0.5f;
                    }
                    else
                    {
                        goldCoinColumn.GetComponent<Slider>().value = Mathf.Round(GoldNum / 100000.0f) * 0.01f;
                    }
                    break;
                }
            default:
                if (GoldNum <= 400000)
                {
                    goldCoinColumn.GetComponent<Slider>().value = 0.04f;
                }
                else if (GoldNum >= 5000000)
                {
                    goldCoinColumn.GetComponent<Slider>().value = 0.5f;
                }
                else
                {
                    goldCoinColumn.GetComponent<Slider>().value = Mathf.Round(GoldNum / 100000.0f) * 0.01f;
                }
                break;
        }
        //if (enemyReturn >= 50) {
		//	enemyReturn = 50;
		//} else {
		//	enemyReturn = enemyReturn + 5;
		//}
		//goldCoinColumn.GetComponent<Slider> ().value = (float)enemyReturn / 50f;
		//BuildGoldCoin(GoldNum,enemyReturn);
		MoveCoinColumn ();
		second = 0;
	}
	//生成金币柱中金币
	//void BuildGoldCoin(int goldnum,int enemyReturn)
	//{
	//    if (enemyReturn>=50)
	//    {
	//         enemyReturn = 50;
	//    }
	//    else
	//    {
	//        enemyReturn = enemyReturn+5;
	//    }
	//    for (int i = 0; i < enemyReturn; i++)
	//    {
	//        if (i==0)
	//        {
	//            if (a)
	//            {
	//                GameObject goldNum = GameObject.Instantiate(PrefabManager._instance.GoldCoinNumObjRed);
	//                goldNum.transform.Find("image/Text").gameObject.GetComponent<Text>().text = goldnum.ToString();
	//                RedOrGreen = !RedOrGreen;
	//                goldNum.transform.SetParent(goldCoinColumn.transform);
	//                goldNum.transform.localScale = new Vector3(0.9f, 0.73f, 1f);
	//            }
	//            else
	//            {
	//                GameObject goldNum = GameObject.Instantiate(PrefabManager._instance.GoldCoinNumObjGreen);
	//                goldNum.transform.Find("image/Text").gameObject.GetComponent<Text>().text = goldnum.ToString();
	//                RedOrGreen = !RedOrGreen;
	//                goldNum.transform.SetParent(goldCoinColumn.transform);
	//                goldNum.transform.localScale = new Vector3(0.9f, 0.73f, 1f);
	//            }
	//        }
	//        else
	//        {
	//            GameObject goldCoin = GameObject.Instantiate(PrefabManager._instance.GoldCoinObj);
	//            goldCoin.transform.SetParent(goldCoinColumn.transform);
	//            goldCoin.transform.localScale = new Vector3(1f, 1f, 1f);
	//        }
	//    }
	//    MoveCoinColumn();
	//}

    /// <summary>
    /// 移动金币柱
    /// </summary>
	void MoveCoinColumn ()
	{
		if (ColumnObj.Count > 4) {
			Destroy (ColumnObj [0]);
			ColumnObj.RemoveAt (0);

		}
		if (ColumnObj.Count == 1) {
			ColumnObj [0].transform.DOLocalMoveX (keepColumnVector [1].localPosition.x, 0.2f);
		} else if (ColumnObj.Count == 2) {
			ColumnObj [0].transform.DOLocalMoveX (keepColumnVector [2].localPosition.x, 0.2f);
			ColumnObj [1].transform.DOLocalMoveX (keepColumnVector [1].localPosition.x, 0.2f);

		} else if (ColumnObj.Count == 3) {
			ColumnObj [0].transform.DOLocalMoveX (keepColumnVector [3].localPosition.x, 0.2f);
			ColumnObj [1].transform.DOLocalMoveX (keepColumnVector [2].localPosition.x, 0.2f);
			ColumnObj [2].transform.DOLocalMoveX (keepColumnVector [1].localPosition.x, 0.2f);
		} else if (ColumnObj.Count == 4) {
			ColumnObj [0].transform.DOLocalMoveX (keepColumnVector [4].localPosition.x, 0.2f);
			ColumnObj [1].transform.DOLocalMoveX (keepColumnVector [3].localPosition.x, 0.2f);
			ColumnObj [2].transform.DOLocalMoveX (keepColumnVector [2].localPosition.x, 0.2f);
			ColumnObj [3].transform.DOLocalMoveX (keepColumnVector [1].localPosition.x, 0.2f);
		}
	}
}
