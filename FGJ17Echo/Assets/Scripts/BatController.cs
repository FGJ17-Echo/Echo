using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : MonoBehaviour {
	[SerializeField]
	private float moveSpeed = 15.0f;
	[SerializeField]
	private float maxVelocity = 100f;
	private Rigidbody2D rb2d;

	void Awake () {
		rb2d = gameObject.GetComponent<Rigidbody2D>();
	}

	void FixedUpdate () {
		if(rb2d.velocity.magnitude < maxVelocity) {
			rb2d.AddForce(Vector2.right * Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime);
			rb2d.AddForce(Vector2.up * Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime);
		}
	}
}
