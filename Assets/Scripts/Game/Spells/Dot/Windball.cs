using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Windball : CircleSpell
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                if (!worksForPlayer) return;

                Player player = collision.gameObject.GetComponent<Player>();
                player.ChangeHP(damage, typeOfDamage);
                player.Push(direction, size);
                Destroy(gameObject);
                break;
            case "Enemy":
                if (!worksForEnemy) return;

                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                enemy.ChangeHP(damage, typeOfDamage);
                enemy.Push(direction, size);
                Destroy(gameObject);
                break;
            case "CircleEffect":
            case "EarthEffect":
                Destroy(gameObject);
                break;
            default: break;
        }
    }
}