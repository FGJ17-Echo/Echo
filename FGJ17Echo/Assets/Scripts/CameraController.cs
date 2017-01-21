using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform _target;
    [SerializeField]
    private Transform _targetLookDirection;

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
    private float _lookDistance = 2;

    [SerializeField]
    private float _maxMoveSpeed = 10;

    private Rigidbody2D _targetRigidbody;

	[SerializeField]
	private float _shakeSpeed = 0.5f;
	[SerializeField]
	private float _shakeAmplifier = 0.3f;
	private float _cameraShakeX = 0f;
	private float _cameraShakeY = 0f;

    private float _shake = 0;

    [SerializeField]
    private float _maxShake = 10;

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

    public void AddShake(float amount)
    {
        _shake = Mathf.Clamp(_shake + amount, 0, _maxShake);
    }
	
	void Update ()
    {
        if (_shake > 0)
        {
            CameraShake();
            _shake -= Time.deltaTime  * _shakeSpeed;
        }
        else
        {
            _cameraShakeX = 0;
            _cameraShakeY = 0;
        }

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
            else
            {
                var direction2d = _targetLookDirection.forward;
                targetPosition += _lookDistance * new Vector3(direction2d.x, direction2d.y, 0);
            }
        }

        for (int i = 0; i < _cameraMoveTransforms.Count; i++)
        {
            targetPosition.z = _cameraMoveTransforms[i].position.z;
			_cameraMoveTransforms[i].position = Vector3.MoveTowards(_cameraMoveTransforms[i].position + new Vector3(_cameraShakeX, _cameraShakeY, 0f), targetPosition, Time.deltaTime * _maxMoveSpeed);
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

	void OnEnable() 
	{
		BatController.EnergyChanged += BatController_EnergyChanged;
	}

	void OnDisable() 
	{
		BatController.EnergyChanged -= BatController_EnergyChanged;
	}

	void BatController_EnergyChanged (BatController.EnergyChangedEventArgs args)
	{
        if (args.Delta < 0 && args.CanDie)
        {
            AddShake(-args.Delta);
        }
    }

	void CameraShake()
    {
		_cameraShakeX = UnityEngine.Random.value * 2.0f - 1.0f;
		_cameraShakeY = UnityEngine.Random.value * 2.0f - 1.0f;
		_cameraShakeX *= _shakeAmplifier * _shake;
		_cameraShakeY *= _shakeAmplifier * _shake;	
	}
}
