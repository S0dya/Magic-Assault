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
    //[HideInInspector] public 
    float luck;

    protected virtual void Awake()
    {
        levelManager = LevelManager.I;
        luck = GameData.I.luck;
    }

    public void SetEnemy() => enemy = GetComponentInParent<Enemy>();

    protected virtual void OnDestroy()
    {
        if (!enemy.isKilled) return;

        if (expChance + luck > Random.value) levelManager.InstantiateExp(expPrefab, GetRandomPos());
        if (coinChance + luck > Random.value) levelManager.InstantiateCoin(coinPrefab, GetRandomPos());
    }

    public Vector2 GetRandomPos()
    {
        return (Vector2)transform.position + levelManager.GetRandomPos(0.25f, 0.25f);
    }
}