using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingStone : CircleSpell
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                Player player = collision.gameObject.GetComponent<Player>();
                player.Stun(1);
                player.ChangeHP(damage, typeOfDamage);
                break;
            case "Enemy":
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                enemy.Stun(1);
                enemy.ChangeHP(damage, typeOfDamage);
                break;
            default: break;
        }
    }
}
