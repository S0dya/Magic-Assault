using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnDestroyEnemyBoss : SpawnOnDestroyEnemy
{
    [SerializeField] GameObject[] upgradesPrefabs;

    //local
    Transform upgradesParent;

    protected override void Awake()
    {
        base.Awake();

        upgradesParent = GameObject.FindGameObjectWithTag("UpgradesParent").GetComponent<Transform>();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        InstantiateAfterDeath(upgradesPrefabs[Random.Range(0, upgradesPrefabs.Length)], upgradesParent);
    }
}
