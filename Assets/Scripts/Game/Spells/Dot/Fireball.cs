using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : CircleSpell
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                if (!worksForPlayer) return;

                Player player = collision.gameObject.GetComponent<Player>();
                player.ChangeHP(damage, typeOfDamage);
                player.Burn();

                Destroy(gameObject);
                break;
            case "Enemy":
                if (!worksForEnemy) return;

                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                enemy.ChangeHP(damage, typeOfDamage);
                enemy.Burn();

                Destroy(gameObject);
                break;
            case "ObstacleSpawner":
                Spawner spawner = collision.gameObject.GetComponent<Spawner>();
                spawner.ChangeHP(damage);

                Destroy(gameObject);
                break;
            case "WaterEffect":
            case "EarthEffect":
            case "ObjectCollider":
                Destroy(gameObject);
                break;
            default: break;
        }
    }
}
