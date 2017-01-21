using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableEnergySource : MonoBehaviour
{
    [SerializeField]
    private float _energy = 10;

    private bool _isCollected = false;

    public float Collect()
    {
        if (_isCollected) return 0;

        Destroy(gameObject);

        return _energy;
    }
}
