using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchDamage : MonoBehaviour
{
    [SerializeField]
    private float _damage = 10;
    [SerializeField]
    private float _damageInterval = 0.5f;
    [SerializeField]
    private bool _isLethal = true;

    private float _lastDamageTime;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleDamage(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        HandleDamage(collision);
    }

    void HandleDamage(Collision2D collision)
    {
        if (_lastDamageTime + _damageInterval < Time.time)
        {
            var go = collision.collider.attachedRigidbody ? collision.collider.attachedRigidbody.gameObject : collision.collider.gameObject;

            IDamageReceiver damageReceiver = go.GetComponent<IDamageReceiver>();

            if (damageReceiver != null)
            {
                damageReceiver.TakeDamage(_damage, _isLethal);

                _lastDamageTime = Time.time;
            }
        }
    }
}
