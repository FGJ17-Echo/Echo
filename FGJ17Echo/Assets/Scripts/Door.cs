using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    private bool _hasLock;

    [SerializeField]
    private bool _isLocked;

    [SerializeField]
    private SpriteRenderer _door;

    [SerializeField]
    private Collider2D _doorCollider;

    public bool IsOpen { get; private set; }

    private LTDescr _openTween;

    public bool IsLocked
    {
        get
        {
            return _hasLock && _isLocked;
        }
    }

    public void Unlock()
    {
        _isLocked = false;
    }

    public void Open()
    {
        if (IsOpen || IsLocked) return;

        if (_openTween != null) LeanTween.cancel(_door.gameObject);

        var col = _door.color;
        col.a = 0;
        _openTween = LeanTween.color(_door.gameObject, col, 0.5f);

        _doorCollider.enabled = false;

        IsOpen = true;
    }

    public void Close()
    {
        if (!IsOpen) return;

        if (_openTween != null) LeanTween.cancel(_door.gameObject);

        var col = _door.color;
        col.a = 1;
        _openTween = LeanTween.color(_door.gameObject, col, 0.3f);

        _doorCollider.enabled = true;

        IsOpen = false;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        var go = collider.attachedRigidbody ? collider.attachedRigidbody.gameObject : collider.gameObject;

        var bat = go.GetComponent<BatController>();

        if (bat != null)
        {
            if (bat.HasTheKey)
            {
                Unlock();
                bat.UseKey();
            }
            Open();
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        var go = collider.attachedRigidbody ? collider.attachedRigidbody.gameObject : collider.gameObject;

        var bat = go.GetComponent<BatController>();

        if (bat != null)
        {
            Close();
        }
    }
}
