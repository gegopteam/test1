using UnityEngine;
using System.Collections;

public class ReplicationGun : MonoBehaviour {

    GameObject lockTarget = null;
    Vector3 targetPos;
    float posZ;
    GunSeat thisSeat;
     
	// Use this for initialization
	void Start () {
        posZ = this.transform.position.z;
	}
	
	// Update is called once per frame
	void Update () {
        if(lockTarget!=null){
            
        }
	}

    void FollowTarget(){
         targetPos= GetPlanePos(lockTarget .transform.position);
    }

    void SetFollowTarget(GameObject lockObj){
        lockTarget = lockObj;
    }

    Vector3 GetPlanePos(Vector3 pos){
        return new Vector3(pos.x, pos.y, posZ);
    }

    void GunRotate(Vector3 pos)
    {
        pos = new Vector3(pos.x, pos.y, transform.position.z);
        targetPos = pos;
        if (thisSeat == GunSeat.LB || thisSeat == GunSeat.RB)
        {
           // changeAngle = Vector3.Angle(Vector3.right, pos - invisibleGun.position);
            //invisibleGun.localEulerAngles = new Vector3(0, 0, changeAngle - 90f);
        }
        else
        {
           // changeAngle = Vector3.Angle(Vector3.left, pos - invisibleGun.position);
           // invisibleGun.localEulerAngles = new Vector3(0, 0, changeAngle - 90f);
        }
    }

}
