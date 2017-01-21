using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SpiderController : MonoBehaviour
{
    [SerializeField]
    private Transform _guardTarget;

    [SerializeField]
    private Transform _leftMaxPosition;

    [SerializeField]
    private Transform _rightMaxPosition;

    [SerializeField]
    private float _moveSpeed = 3;

    private Transform _followTarget;

    private Rigidbody2D _rb2d;

    private void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (_followTarget && transform.position.x > _leftMaxPosition.position.x && transform.position.x < _rightMaxPosition.position.x)
        {
            Move(_followTarget.position);
        }
        else if (_guardTarget)
        {
            Move(_guardTarget.position);
        }
    }

    private void Move(Vector3 target)
    {
        float direction = 0;
        if (target.x > transform.position.x)
        {
            direction = 1;
        }
        else if (target.x < transform.position.x)
        {
            direction = -1;
        }

        _rb2d.AddForce(Vector2.right * direction * _moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        var go = collider.attachedRigidbody ? collider.attachedRigidbody.gameObject : collider.gameObject;

        var bat = go.GetComponent<BatController>();
        if (bat != null)
        {
            _followTarget = bat.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        var go = collider.attachedRigidbody ? collider.attachedRigidbody.gameObject : collider.gameObject;

        if (_followTarget == go.transform)
        {
            _followTarget = null;
        }
    }
}
