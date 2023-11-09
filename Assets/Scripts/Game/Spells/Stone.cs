using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : Spell
{
    [Header("This spell")]
    [SerializeField] CircleCollider2D col;

    float damage;
    public float durationOfStun;

    void Start()
    {
        damage = -Settings.damageOfStone * damageMultiplier;

        col.radius = size / 2;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (worksForPlayer && collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponentInParent<Player>();
            player.Stun(durationOfStun);
            player.ChangeHP(damage, typeOfDamage);
            Destroy(gameObject);
        }
        
        if (worksForEnemy && collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponentInParent<Enemy>();
            enemy.Stun(durationOfStun);
            enemy.ChangeHP(damage, typeOfDamage);
            Destroy(gameObject);
        }
    }
}
