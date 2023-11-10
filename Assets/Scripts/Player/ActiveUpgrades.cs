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
    float[] damageOfSpells = new float[4] { Settings.damageOfFireball, 0, Settings.damageOfStone, 0 };
    //shoot nearest
    int typeOfShootNearestEnemySpell = 0;
    float sizeOfSpellShootingNearestEnemy = 0.5f;
    float timeBetweenShootingNearestEnemy = 3;

    protected override void Awake()
    {
        base.Awake();

        
    }

    void Start() //test
    {
        StartCoroutine(ShootNearestEnemy());
    }

    IEnumerator ShootNearestEnemy()
    {
        while (true)
        {
            InstantiateSpell(spellsOfShoot[0], enemiesTrigger.GetRandomEnemyPosition(), sizeOfSpellShootingNearestEnemy);

            yield return new WaitForSeconds(timeBetweenShootingNearestEnemy);
        }
    }

    void InstantiateSpell(GameObject spell, Vector2 pos, float size)
    {
        if (pos == null) return;
        
        Vector2 startPos = transform.position;
        Vector2 direction = (pos - startPos).normalized;
        float rotation = (Mathf.Atan2(direction.y, direction.x) - 1.5f) * Mathf.Rad2Deg;

        SpellsManager.I.InstantiateEffect(spell, startPos + direction, size, damageOfSpells[typeOfShootNearestEnemySpell], direction, rotation);
    }
}
