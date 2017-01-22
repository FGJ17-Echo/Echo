using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingHeadSpider : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private float _minSpeed = 0.2f;
    [SerializeField]
    private float _maxSpeed = 1;
    [SerializeField]
    private float _speedChangeVelocity = 0.2f;

    private void Update()
    {
        var lerp = 0.5f * (Mathf.Sin(Time.time * _speedChangeVelocity) + 1);
        _animator.speed = Mathf.Lerp(_minSpeed, _maxSpeed, lerp);
    }
}
