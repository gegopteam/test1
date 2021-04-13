using UnityEngine;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;


public class MyTrack : MonoBehaviour {

	DOTweenPath thisPath;
	Vector3 originPos;

    public bool isOverTurnTrack = false;
    bool hasInit = false;
    List<Vector3> overTurnList = new List<Vector3>();
    Tween overTurnTween;

    void Awake()
    {
        if(!hasInit)
            Init();
        //if (this.gameObject.name == "Track_A_20 (1)")
            //PlayPath();
        //thisPath.onComplete.AddListener (MyTrackOnComplete);

	}

    void Init(){
        thisPath = this.GetComponent<DOTweenPath>();
        originPos = transform.position;
        isOverTurnTrack = GameController._instance.isOverTurn;
        if (isOverTurnTrack){
            //originPos = new Vector3(-originPos.x, -originPos.y, originPos.z);
            OverTurnPath();
        }

        hasInit = true;
    }

    private void Start()
    {
       
    }

    public Vector3 GetOriginPos()
	{
		return originPos;
	}

	public void ClearChildObj()
	{
		EnemyBase[] enemys = transform.GetComponentsInChildren<EnemyBase> ();
		for (int i = 0; i < enemys.Length; i++) {
			Debug.LogWarning ("ClearChild groupId:"+enemys[i].groupID+" fishId:"+enemys[i].id);
			//UIFishingMsg.GetInstance ().fishPool.Remove (enemys[i].groupID , enemys[i].id, 0);
			UIFishingObjects.GetInstance ().fishPool.Remove (enemys[i].groupID , enemys[i].id, 0);
			//Destroy (transform.GetChild (i).gameObject);
		}
	}

	public void DelayMove(float delayTime)
	{
		Invoke ("PlayPath", delayTime);
	}
		

	public void ResetPath()
	{
        if (thisPath != null){
            if(isOverTurnTrack){
                transform.position = originPos;
            }else{
                thisPath.DORewind();
            }
        }
			
		else
			Debug.LogError ("Error!Path:" + this.name + " =null");
	}
	public void PausePath()
	{
        if (thisPath != null){
            if(isOverTurnTrack){
                overTurnTween.Pause();
            }else{
                thisPath.DOPause();
            }
        }	
		else
			Debug.LogError ("Error!Path:" + this.name + " =null");
	}

	public void PlayPath()
	{
        if (!hasInit)
            Init();
        if(!isOverTurnTrack ){
            
            if (thisPath != null)
                thisPath.DOPlay();
            else
                Debug.LogError("Error!Path:" + this.name + " =null");
        }else{
            Vector3[] temp = new Vector3[overTurnList.Count];
            for (int i = 0; i < overTurnList.Count;i++){
                temp[i] = overTurnList[i];
            }
            //Debug.LogError("Speed=" + thisPath.tween.PathLength() / thisPath.duration);
            //transform.localPosition = new Vector3(-transform.localPosition.x, -transform.localPosition.y, transform.localPosition.z);
            transform.position = new Vector3(-originPos.x, -originPos.y, originPos.z);
            overTurnTween = transform.DOPath(temp, thisPath.tween.PathLength() / thisPath.duration, thisPath.pathType, thisPath.pathMode,
                                               thisPath.pathResolution, Color.blue).
                                       SetEase(thisPath.easeType);   
           //Debug.LogError("PathDoOverTurnPlay:"+this.gameObject.name);
            
        }
		
	}

    public void ThawPlayPath(){//解冻后的播放路径
        if(!isOverTurnTrack){
            thisPath.DOPlay();
        }else{
            overTurnTween.Play();
        }
    }

	public void SetPathAutoKill(bool killFlag)
	{
		thisPath.autoKill = killFlag;
	}
	public void DoKillPath()
	{
        if(isOverTurnTrack){
            
        }else{
            thisPath.DOKill();
        }
		
	}


    void OverTurnPath(){


        //Debug.LogError("OverTurnLocalPos="+transform.localPosition );
        overTurnList.Clear();
        for (int i = 0; i < thisPath.wps.Count;i++){
            Vector3 tempVector = thisPath.wps[i];
            tempVector = new Vector3(-tempVector.x, -tempVector.y, tempVector.z);
            overTurnList.Add(tempVector);
        }
         

        return;
        thisPath.wps.Clear();

        for (int i = 0; i < overTurnList.Count;i++){
            thisPath.wps.Add(overTurnList [i]);
        }
    }

    [ContextMenu("SetData")]
    public void SetTrackData(){
        thisPath = this.GetComponent<DOTweenPath>();
        Debug.LogError("SetEase");
        thisPath.easeType = Ease.Linear;
        return;
        thisPath.wps.Clear();
        Debug.Log("AddWps "+thisPath.wps.Count);
        thisPath.wps.Add(new Vector3(0, 1, 1));
        thisPath.wps.Add(new Vector3(12, 12, 21));
        
        for (int i = 0; i < thisPath.wps.Count; i++)
        {
            
            //thisPath.wps.Add(new Vector3(i,i,i));
        }
     
    }

    public float GetDelayTime(){
        return thisPath.delay;
    }

    public float GetDurationTime(){
        thisPath = this.GetComponent<DOTweenPath>();
        if(thisPath .isSpeedBased){
           // Debug.LogError("pathLength="+thisPath.tween.PathLength() + " duration="+thisPath.duration);

            return thisPath.tween.PathLength() / thisPath.duration;
        }else{
            Debug.LogError("Error! Path.isSpeeedBaesd=false");
            return thisPath.duration;
        }
    }
}
