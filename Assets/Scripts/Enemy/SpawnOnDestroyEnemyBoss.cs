using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnDestroyEnemyBoss : SpawnOnDestroyEnemy
{
    [SerializeField] GameObject[] upgradesPrefabs;

    protected override void Awake()
    {
        base.Awake();

        SetEnemy();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        levelManager.InstantiateUpgrade(upgradesPrefabs[Random.Range(0, upgradesPrefabs.Length)], GetRandomPos());
    }
}
