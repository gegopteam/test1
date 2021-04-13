using UnityEngine;
using System.Collections;

public class LockTrack : MonoBehaviour {

    public Transform lockTrackEndPoint;

    public Transform[] lockCircleGroup;

    Vector3 startPos=Vector3.zero;


    public void SetStartPoint(Vector3 pos){
        startPos = pos;
    }

    private void FixedUpdate()
    {
        Vector3 perOffset = (startPos-transform.position) * 0.167f;
        for (int i = 0; i < 5;i++){
            lockCircleGroup[i].position = transform.position + (i + 1) * perOffset;
        }
    }
}
