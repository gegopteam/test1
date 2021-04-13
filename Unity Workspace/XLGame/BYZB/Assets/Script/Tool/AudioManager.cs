using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{

	public static AudioManager _instance;

	AudioSource audioSource;

	public AudioClip[] bgmGroup;
	public AudioClip[] effectClipGroup;
	public AudioClip[] speakClipGroup;

	//背景音乐
	public const int bgm_none = -1;
	//-1代表切换到刚场景时不播放bgm
	public const int bgm_classical = 0;
	public const int bgm_pk = 1;
	public const int bgm_hall = 2;

	//音效
	public const int effect_bossComing = 0;
	public const int effect_changeEquip = 1;
	public const int effect_eliterKilled = 2;
	public const int effect_fire = 3;
	public const int effect_fishTide = 4;
	public const int effect_freeze = 5;
	public const int effect_getCoin = 6;
	public const int effect_getItem = 7;
	public const int effect_levelUp = 8;
	public const int effect_pkFail = 9;
	public const int effect_pkVictory = 10;
	public const int ui_click = 11;
	public const int effect_fireTorpedo = 12;
	public const int effect_countDown = 13;
	public const int effect_redPacketOpen = 14;
	public const int effect_getReward = 15;
	public const int effect_closePanel = 16;
	public const int effect_getCoin2 = 17;
	public const int effect_summon = 18;
	public const int effect_manmon = 19;
	public const int effect_manmongold = 20;

	public bool useBgm = true;
	public bool useEffectClip = true;
	public bool useFireSound = true;

	void Awake ()
	{
		if (null == _instance) {
			_instance = this;
		} else {
			Destroy (this.gameObject);
			return;
		}
          
		DontDestroyOnLoad (this.gameObject);
		audioSource = this.GetComponent<AudioSource> ();
		if (!PlayerPrefs.HasKey ("backmusic")) {
			useBgm = PlayerPrefs.GetInt ("backmusic", 1) == 1 ? true : true;
			useEffectClip = PlayerPrefs.GetInt ("effectmusic", 1) == 1 ? true : true;
			useFireSound = PlayerPrefs.GetInt ("gunmusic", 1) == 1 ? true : true;
		} else {
			useBgm = PlayerPrefs.GetInt ("backmusic", 0) == 1 ? true : false;
			useEffectClip = PlayerPrefs.GetInt ("effectmusic", 0) == 1 ? true : false;
			useFireSound = PlayerPrefs.GetInt ("gunmusic", 0) == 1 ? true : false;
		}
		PlayBgm (AudioManager.bgm_hall);

	}

	public static AudioManager GetInstance ()
	{
		if (null == _instance) {
			_instance = new AudioManager ();
		}
		return _instance;
	}

	public void PlayBgm (int bgmIndex, float valueScale = 1f)
	{
		if (bgmIndex == -1) {
			audioSource.clip = null;
			return;
		}
		if (!useBgm) {
			audioSource.clip = null;
			return;
		}
			
		if (audioSource.clip != null) {
			if (audioSource.clip.name == bgmGroup [bgmIndex].name)
				return;
		}
		if (bgmIndex == AudioManager.bgm_classical || bgmIndex == AudioManager.bgm_pk) {
			audioSource.volume = 0.5f;
		} else {
			audioSource.volume = 1f;
		}
		audioSource.clip = bgmGroup [bgmIndex];
		audioSource.Play ();
	}

	public void StopBgm ()
	{
		audioSource.clip = null;
	}

	public void PlayEffectClip (int audioIndex, float valueScale = 1f) //只播一次，不循环
	{
		if (audioIndex == effect_fire) {
			if (useFireSound)
				audioSource.PlayOneShot (effectClipGroup [audioIndex], valueScale);
			return;
		}
		if (useEffectClip) {
			if (audioIndex == ui_click)
				valueScale = 2f;
			audioSource.PlayOneShot (effectClipGroup [audioIndex], valueScale);
		}
			
	}

	public const int voice_hello = 0;
	//"大家好，很高兴见到各位！"
	public const int voice_sorry = 1;
	//"抱歉！"
	public const int voice_easy = 2;
	//"打打打，看你能得意多久。"
	public const int voice_praise = 3;
	//"技不如人，甘拜下风！"
	public const int voice_provoke = 4;
	//"不好意思，又赢了！"

	public void PlaySpeakClip (int audioIndex, float valueScale = 1f)
	{
		if (useEffectClip)
			audioSource.PlayOneShot (speakClipGroup [audioIndex], valueScale);
	}

    public void PlayRedPackAudio()
    {
        StartCoroutine(playaudio());
    }

    IEnumerator playaudio()
    {
        AudioManager._instance.PlayEffectClip(AudioManager.effect_pkVictory, 2f);
        yield return new WaitForSeconds(2f);
        AudioManager._instance.PlayEffectClip(AudioManager.effect_eliterKilled, 2f);
        yield break;
    }

}
