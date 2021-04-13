using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 闪电链
/// </summary>
[RequireComponent (typeof(LineRenderer))]
//[ExecuteInEditMode]
public class ChainLightning : MonoBehaviour
{
	//进行调整波动细节
	float detail = .1f;
	//增加后，线条数量会减少，每个线条会更长。
	float displacement = 1.2f;
	//位移量，也就是线条数值方向偏移的最大值
	public	List<Transform> target;
	//链接目标
	public	Transform start;
	float yOffset = 0;
	LineRenderer _lineRender;
	List<Vector3> _linePosList;

	List<Transform> endPos = new List<Transform> ();
	Vector3 startPos = Vector3.zero;
	public static ChainLightning Instance;

	private void Awake ()
	{
		Instance = this;
		_lineRender = GetComponent<LineRenderer> ();
		_linePosList = new List<Vector3> ();
		endPos.Clear ();
	}

	private void Update ()
	{
		if (Time.timeScale != 0) {
			_linePosList.Clear ();
//			Debug.Log ("target.Count = " + target.Count);
			if (target.Count > 0) {
				for (int i = 0; i < target.Count; i++) {
					if (!endPos.Contains (target [i])) {
						//						Debug.Log ("是否执行");
						//						Debug.Log ("target [i] = " + target [i]);
						endPos.Add (target [i]);
					}
					//					Debug.Log ("target.count = " + target.Count);
					//					Debug.Log ("endPos.count = " + endPos.Count);
					//					Debug.Log ("target[i].transform.position = " + target [i].position);	 
					//					Debug.Log ("endPos[i].transform.position = " + endPos [i].position);	 
					endPos [i].position = target [i].position + Vector3.up * yOffset;
					CollectLinPos (startPos, endPos [i].position, displacement);
					if (!_linePosList.Contains (endPos [i].position)) {
						_linePosList.Add (endPos [i].position);
					}
				}
			}
			if (start != null) {
				startPos = start.position + Vector3.up * yOffset;
			}


			_lineRender.SetVertexCount (_linePosList.Count);
			for (int i = 0, n = _linePosList.Count; i < n; i++) {
				_lineRender.SetPosition (i, _linePosList [i]);
			}
		}
	}

	/// <summary>
	/// 收集顶点，中点分形法插值抖动  原理  https://krazydad.com/bestiary/bestiary_lightning.html
	/// </summary>
	/// <param name="startPos">Start position.</param>
	/// <param name="destPos">Destination position.</param>
	/// <param name="displace">Displace.</param>
	private void CollectLinPos (Vector3 startPos, Vector3 destPos, float displace)
	{
		if (displace < detail) {
			_linePosList.Add (startPos);
		} else {

			float midX = (startPos.x + destPos.x) / 2;
			float midY = (startPos.y + destPos.y) / 2;
			float midZ = (startPos.z + destPos.z) / 2;

			midX += (float)(UnityEngine.Random.value - 0.5) * displace;
			midY += (float)(UnityEngine.Random.value - 0.5) * displace;
			midZ += (float)(UnityEngine.Random.value - 0.5) * displace;

			Vector3 midPos = new Vector3 (midX, midY, midZ);

			CollectLinPos (startPos, midPos, displace / 2);
			CollectLinPos (midPos, destPos, displace / 2);
		}
	}
}
