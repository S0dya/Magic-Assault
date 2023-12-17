using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : BoxSpell
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                Player player = collision.gameObject.GetComponent<Player>();
                player.ChangeHP(damage, typeOfDamage);
                player.Burn();
                
                Destroy(gameObject);
                break;
            case "Enemy":
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
            case "CircleEffect":
            case "WaterEffect":
            case "EarthEffect":
            case "ObjectCollider":
                Destroy(gameObject);
                break;
            default: break;
        }
    }
}
