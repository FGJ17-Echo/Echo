using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxMover : MonoBehaviour
{
    [SerializeField]
    private Transform _referenceCamera;    

    [SerializeField]
    private float _speed;

    private Vector3 _origin;
    private Vector3 _referenceOrigin;
    private Vector3 _offset;

    private void Start()
    {
        _origin = transform.position;
        _referenceOrigin = _referenceCamera.position;
        _offset = _referenceOrigin - _origin;
        _offset.z = 0;
    }

    void Update ()
    {
        var cameraMoved = _referenceCamera.position - _referenceOrigin;
        var delta = cameraMoved * _speed;
        delta.z = 0;
        transform.position = _origin + delta + _offset;
	}
}
