using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : CircleSpell
{
    protected override void Start()
    {
        base.Start();

        damage *= gameData.explosionDamageMultiplier;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                Player player = collision.gameObject.GetComponent<Player>();
                player.DecreaseHP(damage, typeOfDamage);
                break;
            case "Enemy":
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                enemy.ChangeHP(damage, typeOfDamage);
                break;
            case "ObstacleSpawner":
                Spawner spawner = collision.gameObject.GetComponent<Spawner>();
                spawner.ChangeHP(damage);
                break;
            default: break;
        }
    }
}
