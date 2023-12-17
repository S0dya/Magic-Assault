using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : CircleSpell
{
    [Header("Additional logic")]
    [SerializeField] float skipTriggersAmount;

    void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                if (!worksForPlayer) return;

                Player player = collision.gameObject.GetComponent<Player>();
                player.ChangeHP(damage, typeOfDamage);
                player.Stun(0.4f);

                Destroy(gameObject);
                break;
            case "Enemy":
                if (!worksForEnemy) return;
                //logic for additional spells
                if (skipTriggersAmount != 0)
                {
                    skipTriggersAmount--;
                    return;
                }

                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                enemy.ChangeHP(damage, typeOfDamage);
                enemy.Stun(0.4f);

                Destroy(gameObject);
                break;
            case "ObstacleSpawner":
                Spawner spawner = collision.gameObject.GetComponent<Spawner>();
                spawner.ChangeHP(damage);

                Destroy(gameObject);
                break;
            case "ObjectCollider":
                Destroy(gameObject);
                break;
            default: break;
        }
    }
}
