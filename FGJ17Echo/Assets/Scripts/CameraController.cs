using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform _target;

    [SerializeField]
    private float _minSize = 2;
    [SerializeField]
    private float _velocityZoomThreshold = 1;
    [SerializeField]
    private float _maxSize = 4;
    [SerializeField]
    private float _sizeChangeSpeed = 2;
    [SerializeField]
    private float _velocityForMaxZoom = 20;

    [SerializeField]
    private List<Camera> _cameras;

    [SerializeField]
    private List<Transform> _cameraMoveTransforms;

    [SerializeField]
    private float _maxLead = 3;
    [SerializeField]
    private float _thresholdForLead = 1;
    [SerializeField]
    private float _velocityForMaxLead = 10;

    [SerializeField]
    private float _maxMoveSpeed = 10;

    private Rigidbody2D _targetRigidbody;
	
	void Start ()
    {
        if (_target)
        {
            _targetRigidbody = _target.GetComponent<Rigidbody2D>();
        }
    }

    public void SetTarget(Transform target)
    {
        _target = target;
        if (_target)
        {
            _targetRigidbody = _target.GetComponent<Rigidbody2D>();
        }
    }
	
	void Update ()
    {
		if (_target && _target.gameObject.activeInHierarchy)
        {
            SetZoom();
            SetPosition();
        }
	}

    private void SetPosition()
    {
        var targetPosition = _target.position;

        if (_targetRigidbody)
        {
            var velocity = _targetRigidbody.velocity.magnitude;

            if (velocity > _thresholdForLead)
            {
                var lead = Mathf.Lerp(0, _maxLead, ((velocity - _thresholdForLead) / (_velocityForMaxLead - _thresholdForLead)));
                var direction2d = _targetRigidbody.velocity.normalized;
                targetPosition += lead * new Vector3(direction2d.x, direction2d.y, 0);
            }
        }

        for (int i = 0; i < _cameraMoveTransforms.Count; i++)
        {
            targetPosition.z = _cameraMoveTransforms[i].position.z;
            _cameraMoveTransforms[i].position = Vector3.MoveTowards(_cameraMoveTransforms[i].position, targetPosition, Time.deltaTime * _maxMoveSpeed);
        }
    }

    private void SetZoom()
    {
        var size = _minSize;

        if (_targetRigidbody)
        {
            var velocity = _targetRigidbody.velocity.magnitude;

            if (velocity > _velocityZoomThreshold)
            {
                size = Mathf.Lerp(_minSize, _maxSize, ((velocity - _velocityZoomThreshold) / (_velocityForMaxZoom - _velocityZoomThreshold)));
            }
        }

        for (int i = 0; i < _cameras.Count; i++)
        {
            _cameras[i].orthographicSize = Mathf.MoveTowards(_cameras[i].orthographicSize, size, Time.deltaTime * _sizeChangeSpeed);
        }
    }
}
