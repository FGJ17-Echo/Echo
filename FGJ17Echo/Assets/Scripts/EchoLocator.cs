using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoLocator : MonoBehaviour
{
    [SerializeField]
    private EchoMask _echoMaskPrefab;

    [SerializeField]
    private float _cooldown = 0.5f;

    [SerializeField]
    private int _rays = 13;

    [SerializeField]
    private float _angle = 90;

    [SerializeField]
    private float _range = 10;

    [SerializeField]
    private LayerMask _layerMask;

    private Transform _aimTransform;

    private float _cooldownTimer = 0;

    private void Start()
    {
        _aimTransform = new GameObject("Echo Aim").transform;
        _aimTransform.SetParent(transform, false);
    }

    private void Update()
    {
        _cooldownTimer = Mathf.MoveTowards(_cooldownTimer, 0, Time.deltaTime);
        if (Input.GetKeyUp(KeyCode.R)) Echo();
    }

    public void Echo()
    {
        if (_cooldownTimer > 0) return;

        _cooldownTimer = _cooldown;
        var direction = transform.forward;
        var origin = transform.position;

        var angleDiff = _rays > 1 ? _angle / (_rays - 1) : 0;

        var angle = new Vector3(-_angle / 2, 90, 0);
        _aimTransform.localEulerAngles = angle;

        for (int i = 0; i < _rays; i++)
        {
            var hit = Physics2D.Raycast(origin, _aimTransform.forward, _range, _layerMask);

            angle.x += angleDiff;
            _aimTransform.localEulerAngles = angle;
            
            if (hit.collider != null)
            {
                var echo = Instantiate(_echoMaskPrefab);
                echo.transform.position = hit.point;
                echo.Init(hit.distance / 8f);
            }
        }
    }
}
