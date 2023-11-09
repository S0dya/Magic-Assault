using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Spell
{
    [Header("This spell")]
    [SerializeField] CircleCollider2D col;

    float damage;
    float damageOfBurning;
    int timeOfBurning;

    void Start()
    {
        damage = -Settings.damageOfFireball * damageMultiplier;

        col.radius = size / 2;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (worksForPlayer && collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponentInParent<Player>();
            player.ChangeHP(damage, typeOfDamage);
            player.Burn(damageOfBurning, timeOfBurning);
            Destroy(gameObject);
        }

        if (worksForEnemy && collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponentInParent<Enemy>();
            enemy.ChangeHP(damage, typeOfDamage);
            enemy.Burn(damageOfBurning, timeOfBurning);
            Destroy(gameObject);
        }
    }
}
