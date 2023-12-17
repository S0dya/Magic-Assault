using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveUpgradeWaterball : CircleSpell
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Enemy":
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                enemy.ChangeHP(damage, typeOfDamage);
                enemy.HandleWater();
                break;
            case "ObstacleSpawner":
                Spawner spawner = collision.gameObject.GetComponent<Spawner>();
                spawner.ChangeHP(damage);
                break;
            default: break;
        }

        Destroy(gameObject);
    }
}
