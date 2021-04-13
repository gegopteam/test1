using UnityEngine;
using System.Collections;
using DG.Tweening;

/*鱼游动的路线
 *鱼的类型
 *鱼游动线路(线路决定水域范围，防止鱼之间穿插)
 *鱼游动的速度
 *线路是一个空对象，添加一条鱼，鱼随空对象在设定好的路线上移动
 *路线由配置文件来设置
 */
public class LineToMove : MonoBehaviour {
	public int indexLine; //路线编号

	public GameObject fish;
	public int type; //鱼类型
	public float speed; //鱼的游动速度
	DOTweenPath path;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
