using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatAnimationController : MonoBehaviour {

	[SerializeField]
	private DirectionController dirCont;

	void Update () 
	{
		float angle = dirCont.GetDirectionAngle();
		var euler = transform.localEulerAngles;
		if(angle > 90 && angle < 280)
		{
			euler.y = 180;
			euler.z = -angle-180;
		}
		else
		{
			euler.y = 0;
			euler.z = angle;
		}
		transform.localEulerAngles = euler;
	}
}
