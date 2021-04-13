using UnityEngine;
using System.Collections;

public class GiftBagCommand : MonoBehaviour
{
    public GameObject[] imageGameobject;
    private void Awake()
    {
        for (int i = 0; i < imageGameobject.Length; i++)
        {
            imageGameobject[i].gameObject.SetActive(false);
        }
    }

    public void OnDeleteClick() 
    {
        Destroy(this.gameObject);
    }
}
