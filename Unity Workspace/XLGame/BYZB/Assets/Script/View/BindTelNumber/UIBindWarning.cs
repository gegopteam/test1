using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 游客每次进入游戏大厅时弹出安全提醒
/// </summary>
public class UIBindWarning : MonoBehaviour 
{
    public Button BtnSure;
    public Button BtnEsc;

	void Start ()
    {
        
        //去绑定
        BtnSure.onClick.AddListener(() => {
            AudioManager._instance.PlayEffectClip(AudioManager.ui_click);
            BindWindowCtrl.Instense.GenerateWindow();
            Destroy(gameObject);
        });
        //关闭弹窗
        BtnEsc.onClick.AddListener(()=>{
            AudioManager._instance.PlayEffectClip(AudioManager.effect_closePanel);
            Destroy(gameObject);
        });
	}

}
