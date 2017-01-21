using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveController))]
public class WaspSwarm : MonoBehaviour
{
    [SerializeField]
    private Transform _guardTarget;

    [SerializeField]
    private float _maxDistanceFromGuardTarget = 20;

    private MoveController _moveController;

    private Transform _followTarget;

    private void Awake()
    {
        _moveController = GetComponent<MoveController>();
    }
    
	void Update ()
    {
		if (_followTarget && _guardTarget)
        {
            if (Vector3.Distance(_guardTarget.position, transform.position) > _maxDistanceFromGuardTarget)
            {
                _followTarget = null;
            }
        }

        if (_followTarget)
        {
            var direction = (_followTarget.position - transform.position).normalized;
            _moveController.Move(direction);
        }
        else if (_guardTarget)
        {
            var direction = (_guardTarget.position - transform.position).normalized;
            _moveController.Move(direction);
        }
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
