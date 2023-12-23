using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnDestroyEnemy : MonoBehaviour
{
    [Header("Settings")]
    [Range(0, 1)] public float expChance;
    [Range(0, 1)] public float coinChance;

    [Header("Other")]
    [SerializeField] Enemy enemy;
    [SerializeField] GameObject expPrefab;
    [SerializeField] GameObject coinPrefab;

    //local
    [HideInInspector] public LevelManager levelManager;


    protected virtual void Awake()
    {
        levelManager = LevelManager.I;
    }

    public void SetEnemy() => enemy = GetComponentInParent<Enemy>();

    protected virtual void OnDestroy()
    {
        if (!enemy.isKilled) return;

        if (expChance > Random.value) levelManager.InstantiateExp(expPrefab, transform.position);
        if (coinChance > Random.value) levelManager.InstantiateCoin(coinPrefab, transform.position);
    }
}