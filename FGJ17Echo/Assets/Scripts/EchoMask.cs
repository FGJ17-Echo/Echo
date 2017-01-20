using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EchoMask : MonoBehaviour
{
    private float _delay;

    [SerializeField]
    private float _lifetime = 1;
    [SerializeField]
    private float _startScale = 0.2f;
    [SerializeField]
    private float _endScale = 1;

    private SpriteRenderer _renderer;

    private float _time;

    private Vector3 _initialScale;
    private Color _initialColor;
    private Transform _transform;

    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _transform = GetComponent<Transform>();
        _initialScale = _transform.localScale;
        _initialColor = _renderer.color;

        _transform.localScale = _initialScale * _startScale;

        _renderer.enabled = false;
    }

    public void Init(float delay)
    {
        _delay = delay;
    }

	void Update ()
    {
        _time += Time.deltaTime;

        var progress = (_time - _delay) / _lifetime;

        if (progress > 1)
        {
            Destroy(gameObject);
        }
        else if (progress > 0)
        {
            _renderer.enabled = true;

            var color = _renderer.color;
            color.a = _initialColor.a * (1 - progress);
            _renderer.color = color;

            _transform.localScale = _initialScale * Mathf.Lerp(_startScale, _endScale, progress);
        }
    }
}
