using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BossDeadHint : MonoBehaviour {

    public GameObject[] bossGroup;
    public Sprite[] killBossNameSpriteGroup;

    private void Start()
    {
       // SetBossIconShow(20,"ssssa",1244); 
    }

    public void SetBossIconShow(int bossTypeId,string userName,int killGold){

        for (int i = 0; i < bossGroup.Length;i++){
            bossGroup[i].SetActive(false);
        }
        int index = bossTypeId - 19; //章鱼=19，也是最小的boss

        bossGroup[index].SetActive(true);
        transform.Find("BossDeadHint/KillBossName").GetComponent<Image>().sprite = killBossNameSpriteGroup[index];
        transform.Find("BossDeadHint/NameBar/UserName").GetComponent<Text>().text = userName;
        transform.Find("BossDeadHint/GoldNumBar/Text").GetComponent<Text>().text = killGold.ToString();
        Destroy(this.gameObject, 4f); 
    }


}
