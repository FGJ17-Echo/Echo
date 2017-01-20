using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EchoMask : MonoBehaviour
{
    public float delay;
    public float lifetime = 1;
    public float startScale = 1;
    public float endScale = 10;

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

        _transform.localScale = _initialScale * startScale;

        _renderer.enabled = false;
    }

	void Update ()
    {
        

        _time += Time.deltaTime;

        var progress = (_time - delay) / lifetime;

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

            _transform.localScale = _initialScale * Mathf.Lerp(startScale, endScale, progress);
        }
    }
}
