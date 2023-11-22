using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveUpgrades : SingletonMonobehaviour<ActiveUpgrades>
{
    [Header("Settings")]
    [SerializeField] EnemiesTrigger enemiesTrigger;

    //shoot 
    [Header("shoot enemy")]
    [SerializeField] GameObject[] spellsOfShoot;


    //local
    int[] indexOfSpellOfShoot = new int[3] { 3, 1, 2 }; // 0 - nearest. 1 - random enemy. 2 - random position
    float[] sizeOfSpellsOnShoot = new float[3] { 0.5f, 0.5f, 0.5f };
    float[] timeBetweenShoot = new float[3] { 3, 4, 2 };

    protected override void Awake()
    {
        base.Awake();

        
    }

    void Start() //test
    {
        //EnableShootingNearestEnemy();
        //EnableShootingRandomEnemy();
        //EnableShootingRandomPosition();
    }

    //enabling active upgrades
    public void EnableShootingNearestEnemy(int indexOfSpell)
    {
        indexOfSpellOfShoot[0] = indexOfSpell;
        StartCoroutine(ShootNearestEnemy());
    }
    public void EnableShootingRandomEnemy(int indexOfSpell)
    {
        indexOfSpellOfShoot[1] = indexOfSpell;
        StartCoroutine(ShootInRandomEnemyCor());
    }
    public void EnableShootingRandomPosition(int indexOfSpell)
    {
        indexOfSpellOfShoot[2] = indexOfSpell;
        StartCoroutine(ShootInRandomPositionCor());
    }

    // main coroutines
    IEnumerator ShootNearestEnemy()
    {
        while (true)
        {
            if (enemiesTrigger.HasEnemiesNearPlayer()) InstantiateSpell(indexOfSpellOfShoot[0], enemiesTrigger.GetNearestEnemyPosition(), sizeOfSpellsOnShoot[0]);

            yield return new WaitForSeconds(timeBetweenShoot[0]);
        }
    }
    IEnumerator ShootInRandomEnemyCor()
    {
        while (true)
        {
            if (enemiesTrigger.HasEnemiesNearPlayer()) InstantiateSpell(indexOfSpellOfShoot[1], enemiesTrigger.GetRandomEnemyPosition(), sizeOfSpellsOnShoot[1]);

            yield return new WaitForSeconds(timeBetweenShoot[1]);
        }
    }
    IEnumerator ShootInRandomPositionCor()
    {
        while (true)
        {
            InstantiateSpell(indexOfSpellOfShoot[2], enemiesTrigger.GetRandomPosition(), sizeOfSpellsOnShoot[2]);

            yield return new WaitForSeconds(timeBetweenShoot[2]);
        }
    }

    void InstantiateSpell(int indexOfSpell, Vector2 pos, float size)
    {
        Vector2 startPos = transform.position;
        Vector2 direction = (pos - startPos).normalized;
        float rotation = (Mathf.Atan2(direction.y, direction.x) - 1.5f) * Mathf.Rad2Deg;

        SpellsManager.I.InstantiateEffect(spellsOfShoot[indexOfSpell], startPos + direction, size, direction, rotation);
    }
}
