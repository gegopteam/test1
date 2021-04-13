using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class QuickChat : MonoBehaviour
{
    //public GameObject panel;
    public GameObject quickChat;
    public GameObject expression;

    // Use this for initialization
    void Start()
    {
        //GetComponent<Button>().onClick.AddListener(quickChatClick);
    }
    //void quickChatClick()
    //{
    //    if (panel.activeInHierarchy)
    //        panel.SetActive(false);
    //    else
    //    {
    //        panel.SetActive(true);

    //        panel.transform.SetSiblingIndex(-1);
    //    }
    //}
    //快速聊天
    public void OnShowQuickChatPanelClick()
    {
        quickChat.gameObject.SetActive(true);
        expression.gameObject.SetActive(false);
    }
    //表情
    public void OnShowExpressionPanelClick()
    {
        expression.gameObject.SetActive(true);
        quickChat.gameObject.SetActive(false);
    }
}
