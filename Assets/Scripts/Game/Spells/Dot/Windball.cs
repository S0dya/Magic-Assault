using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Windball : Spell
{
    [Header("This spell")]
    [SerializeField] CircleCollider2D col;


    void Start()
    {
        col.radius = size / 2;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (worksForPlayer && collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponentInParent<Player>();
            player.Push(directionOfPush, size);
            player.ChangeHP(damage, typeOfDamage);
            Destroy(gameObject);
        }

        if (worksForEnemy && collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponentInParent<Enemy>();
            enemy.Push(directionOfPush, size);
            enemy.ChangeHP(damage, typeOfDamage);
            Destroy(gameObject);
        }
    }
}
