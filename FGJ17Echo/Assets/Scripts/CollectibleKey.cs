using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleKey : CollectableEnergySource
{
    private BatController _carrier;

    private float _firstAllowedCollectTime;
    
    private void BatController_EnergyChanged(BatController.EnergyChangedEventArgs args)
    {
        if (_isCollected && _carrier == args.Bat && args.Delta < 0 && args.CanDie)
        {
            Drop(); 
        }
    }

    public override float Collect(BatController bat)
    {
        if (_isCollected || Time.time < _firstAllowedCollectTime) return 0;

        _carrier = bat;

        _isCollected = true;

        gameObject.SetActive(false);

        bat.CollectKey(this);

        BatController.EnergyChanged += BatController_EnergyChanged;

        SoundManager.Instance.PlaySound(SoundManager.SoundEffect.Pickup, transform.position);

        return 0;
    }

    private void Drop()
    {
        _firstAllowedCollectTime = Time.time + 1;
        transform.position = _carrier.transform.position - Vector3.up * 0.5f;
        gameObject.SetActive(true);
        _isCollected = false;
        _carrier.DropKey();
        _carrier = null;
        BatController.EnergyChanged -= BatController_EnergyChanged;
    }

    private void OnDestroy()
    {
        if (_isCollected)
        {
            BatController.EnergyChanged -= BatController_EnergyChanged;
        }
    }

    internal void Use()
    {
        Destroy(gameObject);
    }
}
