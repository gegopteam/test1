using UnityEngine;
using System.Collections;

public class IllustratedPanel : MonoBehaviour {

    public void Btn_ClosePanel(){
        AudioManager._instance.PlayEffectClip(AudioManager.effect_closePanel);
        Destroy(this.gameObject);
    }
}
