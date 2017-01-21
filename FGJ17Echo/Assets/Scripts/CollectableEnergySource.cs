using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableEnergySource : MonoBehaviour
{
    [SerializeField]
    protected float _energy = 10;

    protected bool _isCollected = false;

    public virtual float Collect(BatController bat)
    {
        if (_isCollected) return 0;

        Destroy(gameObject);

        return _energy;
    }
}
