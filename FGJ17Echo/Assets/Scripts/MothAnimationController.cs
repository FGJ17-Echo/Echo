using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MothAnimationController : MonoBehaviour {

	private Rigidbody2D _rb2d;

	void Awake () 
	{
		_rb2d = transform.parent.GetComponent<Rigidbody2D>();
	}

	void Update () {
//		Quaternion rotation = Quaternion.LookRotation(_rb2d.velocity, transform.TransformDirection(Vector3.up));
//		transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
		var euler = transform.localEulerAngles;
		euler.x = _rb2d.velocity.x;
		euler.y = _rb2d.velocity.y;
		transform.localEulerAngles = euler;
	}
}
