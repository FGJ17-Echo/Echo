using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleKey : CollectableEnergySource
{
    private BatController _carrier;

    private float _firstAllowedCollectTime;

    public override float Collect(BatController bat)
    {
        if (_isCollected || Time.time < _firstAllowedCollectTime) return 0;

        _carrier = bat;

        gameObject.SetActive(false);

        bat.CollectKey();

        return 0;
    }

    private void Drop()
    {
        _firstAllowedCollectTime = Time.time + 1;
        transform.position = _carrier.transform.position - Vector3.up;
        gameObject.SetActive(true);
        _isCollected = false;
        _carrier.DropKey();
        _carrier = null;
    }
}
