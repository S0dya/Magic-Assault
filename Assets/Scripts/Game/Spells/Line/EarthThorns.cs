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
                player.ChangeHP(damage, typeOfDamage);
                player.Stun(0.1f);
                
                Destroy(gameObject);
                break;
            case "Enemy":
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                enemy.ChangeHP(damage, typeOfDamage);
                enemy.Stun(0.1f);
                
                Destroy(gameObject);
                break;
            case "CircleEffect":
                Destroy(gameObject);
                break;
            default: break;
        }
    }
}
