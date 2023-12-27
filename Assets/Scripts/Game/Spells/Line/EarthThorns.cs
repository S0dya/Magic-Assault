using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthThorns : BoxSpell
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                Player player = collision.gameObject.GetComponent<Player>();
                player.DecreaseHP(damage, typeOfDamage);
                player.Stun(0.1f);
                
                Destroy(gameObject);
                break;
            case "Enemy":
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                enemy.ChangeHP(damage, typeOfDamage);
                enemy.Stun(0.1f);
                
                Destroy(gameObject);
                break;
            case "ObstacleSpawner":
                Spawner spawner = collision.gameObject.GetComponent<Spawner>();
                spawner.ChangeHP(damage);

                Destroy(gameObject);
                break;
            case "CircleEffect":
            case "ObjectCollider":
                Destroy(gameObject);
                break;
            default: break;
        }
    }
}
