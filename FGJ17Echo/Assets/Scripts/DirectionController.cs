using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionController : MonoBehaviour {

	void Update () {
		var euler = transform.localEulerAngles;
		euler.z = Mathf.Atan2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * (180 / Mathf.PI);
		transform.localEulerAngles = euler;
	}
}
