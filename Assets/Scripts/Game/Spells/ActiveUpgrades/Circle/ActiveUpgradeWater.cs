using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveUpgradeWater : CircleSpell
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        enemy.EnterWater();
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        enemy.ExitWater();
    }
}
