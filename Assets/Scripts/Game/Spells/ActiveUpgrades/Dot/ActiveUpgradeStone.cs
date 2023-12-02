using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveUpgradeStone : CircleSpell
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.ChangeHP(damage, typeOfDamage);
            enemy.Stun(0.4f);
        }

        Destroy(gameObject);
    }
}
