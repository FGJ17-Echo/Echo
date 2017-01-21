using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionController : MonoBehaviour
{
    [SerializeField]
    private float _deadzone = 0.3f;

	void Update ()
    {
		var euler = transform.eulerAngles;

        var y = Input.GetAxis("Horizontal");
        var x = Input.GetAxis("Vertical");

        if (Mathf.Abs(x) < _deadzone) x = 0;
        if (Mathf.Abs(y) < _deadzone) y = 0;

        if (x == 0 && y == 0) return;

        var direction = new Vector3(x, y, 0);

        var angle = Vector3.Angle(direction, Vector3.up);
        if (x < 0) angle = 360 - angle;
        
        var snapped = Mathf.Round(angle / 45) * 45;

        transform.eulerAngles = new Vector3(0, 0, snapped);
	}
}
