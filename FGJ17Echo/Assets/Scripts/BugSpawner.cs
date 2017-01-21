using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _prefab;

    [SerializeField]
    private float _spawnInterval = 30;

    private float _lastSpawnTime;

    private GameObject _bug;

    private void Update()
    {
        if (_bug == null &&_lastSpawnTime + _spawnInterval < Time.time)
        {
            Spawn();
        }
    }

    void Spawn()
    {
        _lastSpawnTime = Time.time;

        _bug = Instantiate(_prefab) as GameObject;
        _bug.transform.position = transform.position;
    }
}
