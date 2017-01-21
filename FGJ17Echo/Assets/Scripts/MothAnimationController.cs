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
		var direction = new Vector3(_rb2d.velocity.x, _rb2d.velocity.y, 0);

		var angle = Vector3.Angle(direction, Vector3.up);
		if (_rb2d.velocity.x < 0) angle = 360 - angle;
		transform.eulerAngles = new Vector3(0, 0, angle);
	}
}
