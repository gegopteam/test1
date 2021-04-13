using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CoinEffect : MonoBehaviour {

	Vector3 targetPos;
	float duration;
	GunInUI uiGun;
    public Transform childCoin;
    Vector3 arcDir = Vector3.zero;
    Vector3 vertexPos;
    bool doArcMove = false;

    Vector3 center;
    Vector3 riseRelCenter;
    Vector3 setRelCenter;
    float timer = 0;
    float fracComplete;

    public float radPercent = 0.6f;
    public float radDistance = 0.1f;

    private void Start()
    {
        //MoveToPos(new Vector3(-3.16f, -4.02f,0), 1.3f, 0.4f);
    }

    public void MoveToPos(GunInUI uiGun,float delay, float duration) //delay=1.3 duration=0.4
	{
		this.uiGun = uiGun;

		switch (GameController._instance.myGameType) {
		case GameType.Classical:
			targetPos = uiGun.goldImage.transform.position;
			break;
		case GameType.Bullet:
			targetPos = uiGun.scoreText.transform.position;
			break;
		case GameType.Point:
			targetPos = uiGun.scoreText.transform.position;
			break;
		case GameType.Time:
			targetPos = uiGun.scoreText.transform.position;
			break;
		default:
			break;
		}
        targetPos = new Vector3(targetPos.x, targetPos.y, 51f);
		this.duration = duration;
		Invoke ("StartToMove", delay);
	}

    public void MoveToPos(Vector3 pos, float delay, float duration) //delay=1.3 duration=0.4
    {
        targetPos = pos;
        this.duration = duration;
        Invoke("StartToMove", delay);
    }


	void StartToMove()
	{
        //Tweener moveTweener= transform.DOMove (targetPos, duration);
        //moveTweener.SetEase (Ease.InQuad);
        doArcMove = true;

        if (doArcMove){
           
            Vector3 straightDir = targetPos - transform.position;
            center = transform.position + radPercent * straightDir;
            straightDir = new Vector3(straightDir.x, straightDir.y, 0);
            arcDir = Vector3.Cross(straightDir, new Vector3(0, 0, 1));
            center -= arcDir* radDistance;
            riseRelCenter = transform.position - center;
            setRelCenter = targetPos - center;
        }else{
            Vector3 straightDir = targetPos - transform.position;
            straightDir = new Vector3(straightDir.x, straightDir.y, 0);
            arcDir = -Vector3.Cross(straightDir, new Vector3(0, 0, 1));
            vertexPos = 0.5f * (targetPos + transform.position) + arcDir.normalized * 1;
            //transform.DOMove(vertexPos, duration * 0.5f);
            childCoin.DOLocalMove(arcDir.normalized, duration * 0.5f);
            Invoke("DelayArcMove", duration * 0.5f);
            //Invoke("DelayScale", 0.3f);
        }


		Destroy (this.gameObject, duration);
	}

    void DelayArcMove(){
        //transform.DOMove(targetPos, duration * 0.5f);
        childCoin.DOLocalMove(-arcDir.normalized, duration * 0.5f);
    }

    private void Update()
    {
        
        if (doArcMove)
        {
            timer += Time.deltaTime;
            fracComplete = timer / duration;
            transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, fracComplete);
            transform.position += center;
        }
    }

    private void FixedUpdate2()
    {
        if (doArcMove)
        {
            timer += Time.fixedTime;
            fracComplete = timer / duration;
            transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, fracComplete);
            transform.position += center;
        }
       
    }

    void DelayScale(){
        this.transform.DOScale(0.75f, 0.3f);
    }


	void OnDestroy()
	{
		uiGun.PlayGoldAcceptEffect ();
	}
}
