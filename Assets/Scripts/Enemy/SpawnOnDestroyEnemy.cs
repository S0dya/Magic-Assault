using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnDestroyEnemy : MonoBehaviour
{
    [SerializeField] Enemy enemy;
    [SerializeField] GameObject expPrefab;
    [SerializeField] GameObject coinPrefab;

    //local
    Transform expParent;
    Transform coinParent;


    protected virtual void Awake()
    {
        expParent = GameObject.FindGameObjectWithTag("ExpParent").GetComponent<Transform>();
        coinParent = GameObject.FindGameObjectWithTag("CoinParent").GetComponent<Transform>();
    }

    public void SetEnemy() => enemy = GetComponent<Enemy>();

    protected virtual void OnDestroy()
    {
        if (!enemy.isKilled) return;

        if (Random.Range(0, 2) == 1) InstantiateAfterDeath(expPrefab, expParent);
        if (Random.Range(0, 20) == 1) InstantiateAfterDeath(coinPrefab, coinParent);
    }

    public void InstantiateAfterDeath(GameObject prefab, Transform parent) => Instantiate(prefab, transform.position, Quaternion.identity, parent);
}