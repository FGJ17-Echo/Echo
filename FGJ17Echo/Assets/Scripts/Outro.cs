using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Outro : MonoBehaviour
{
    [SerializeField]
    private List<SpriteRenderer> _images;

    [SerializeField]
    private float _shake = 0.2f;

    private int _current;

    private bool _isOver;

    private void Start()
    {
        for (int i = 1; i < _images.Count; i++)
        {
            _images[i].gameObject.SetActive(false);
            _images[i].color = Color.black;
        }
    }

    public void Next()
    {
        if (_isOver) return;

        _current++;
        if (_current >= _images.Count)
        {
            SceneManager.LoadScene(1);
            _isOver = true;
        }
        else
        {
            _images[_current - 1].gameObject.SetActive(false);
            _images[_current].gameObject.SetActive(true);
            LeanTween.color(_images[_current].gameObject, Color.white, 0.8f);
        }
    }
}
