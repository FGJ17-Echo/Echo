using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoLocator : MonoBehaviour
{
    [SerializeField]
    private EchoMask _echoMaskPrefab;

    [SerializeField]
    private EchoMask _echoPriotityMaskPrefab;

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

    [SerializeField]
    private float _energyUsage = 1;
    public float EnergyUsage
    {
        get
        {
            return _energyUsage;
        }
    }

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
                var go = hit.collider.attachedRigidbody ? hit.collider.attachedRigidbody.gameObject : hit.collider.gameObject;

                var collectible = go.GetComponent<CollectableEnergySource>();
                var damage = go.GetComponent<TouchDamage>();
                var wasp = go.GetComponent<WaspSwarm>();

                var echo = Instantiate(collectible || damage ? _echoPriotityMaskPrefab : _echoMaskPrefab);
                echo.transform.position = hit.point;
                echo.Init(hit.distance / 8f);

                SoundManager.SoundEffect effect = SoundManager.SoundEffect.NeutralPing;

                if (wasp) effect = SoundManager.SoundEffect.WaspPing;
                else if (damage) effect = SoundManager.SoundEffect.DangerPing;
                else if (collectible) effect = SoundManager.SoundEffect.BonusPing;

                SoundManager.Instance.PlaySound(effect, new Vector3(hit.point.x, hit.point.y, 0));
            }
        }
    }
}
