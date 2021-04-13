/* author:KinSen
 * Date:2017.05.23
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDispatch
{
	void OnRcv(int type, object data);
}
