using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterLine : CircleSpell
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                Player player = collision.gameObject.GetComponent<Player>();
                player.ChangeHP(damage, typeOfDamage);
                player.HandleWater();
                Destroy(gameObject);
                break;
            case "Enemy":
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                enemy.ChangeHP(damage, typeOfDamage);
                enemy.HandleWater();
                Destroy(gameObject);
                break;
            case "CircleEffect":
            case "FireEffect":
            case "EarthEffect":
            case "ObjectCollider":
                Destroy(gameObject);
                break;
            default: break;
        }
    }
}
