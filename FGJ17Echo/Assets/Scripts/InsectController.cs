using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveController))]
public class InsectController : MonoBehaviour
{
    private MoveController _moveController;

    [SerializeField]
    private Transform _areaCenter;

    [SerializeField]
    private float _areaRadius;
    
    private Vector3 _targetLocation;

    [SerializeField]
    private float _refreshTime = 4;

    private float _refreshTimer;

    private void Awake()
    {
        _moveController = GetComponent<MoveController>();
    }

    void Update()
    {
        _refreshTimer -= Time.deltaTime;

        if (_refreshTimer <= 0)
        {
            _targetLocation = GenerateWaypoint();
        }

        var direction = (_targetLocation - transform.position).normalized;
        _moveController.Move(direction);
    }

    public Vector3 GenerateWaypoint()
    {
        var center = transform.position;

        _refreshTimer = _refreshTime;

        if (_areaCenter)
        {
            center = _areaCenter.position;
        }

        var point = Random.insideUnitCircle;

        return new Vector3(point.x, point.y, 0) * _areaRadius + center;
    }
}
