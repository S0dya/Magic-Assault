using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveUpgradeLava : CircleSpell
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        enemy.EnterLava();
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        enemy.ExitLava();
    }
}
