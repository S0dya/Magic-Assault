using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveUpgradeTornado : CircleSpell
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        enemy.EnterTornado(transform.position);
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        enemy.ExitTornado();
    }
}
