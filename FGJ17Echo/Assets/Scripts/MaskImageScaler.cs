using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskImageScaler : MonoBehaviour
{
    [SerializeField]
    private Canvas _canvas;

    [SerializeField]
    private float _imageAspectRatio = 2;

    private RectTransform _tr;

    private float _aspectRatio;

    private void Awake()
    {
        _tr = GetComponent<RectTransform>();
        _aspectRatio = Screen.width / Screen.height;
        UpdateSize(_aspectRatio);
    }

    void Update ()
    {
        var currentRatio = Screen.width / Screen.height;

        if (!Mathf.Approximately(_aspectRatio, currentRatio))
        {
            _aspectRatio = currentRatio;
            UpdateSize(_aspectRatio);
        }
    }

    public void UpdateSize(float aspectRatio)
    {
        if (aspectRatio > _imageAspectRatio)
        {
            _tr.sizeDelta = new Vector2(Screen.width, Screen.width / _imageAspectRatio) / _canvas.scaleFactor;
        }
        else
        {
            _tr.sizeDelta = new Vector2(Screen.height * _imageAspectRatio, Screen.height) / _canvas.scaleFactor;
        }
    }
}
