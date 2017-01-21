using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatAnimationController : MonoBehaviour {

	[SerializeField]
	private DirectionController _dirCont;
	private Rigidbody2D _rb2d;
	private Animator _animator;
	[SerializeField]
	private float _idleTreshold = 0.1f;

	void Awake () 
	{
		_rb2d = transform.parent.GetComponent<Rigidbody2D>();
		_animator = GetComponent<Animator> ();
	}

	void Update () 
	{
		float angle = _dirCont.GetDirectionAngle();
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

        //animations
        if (_rb2d.velocity.magnitude < _idleTreshold && Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
            _animator.SetBool("idle", true);
        else
        {
            _animator.SetBool("idle", false);
            _animator.speed = 0.5f + new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).magnitude * 0.5f;
        }
	}
}
