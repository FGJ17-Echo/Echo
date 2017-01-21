using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MoveController : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 15.0f;
    [SerializeField]
    private float _maxVelocity = 100f;

    private Rigidbody2D _rb2d;

    private Vector2 _direction;

    void Awake()
    {
        _rb2d = gameObject.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (_direction != Vector2.zero)
        {
            if (_rb2d.velocity.magnitude < _maxVelocity)
            {
                _rb2d.AddForce(Vector2.right * _direction.x * _moveSpeed * Time.deltaTime);
                _rb2d.AddForce(Vector2.up * _direction.y * _moveSpeed * Time.deltaTime);
            }

            _direction = Vector2.zero;
        }
    }

    public void Move(Vector2 direction)
    {
        _direction = direction;
    }
}
