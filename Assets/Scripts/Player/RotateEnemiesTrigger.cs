using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateEnemiesTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        enemy.ChangeLookingDirection();
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        enemy.ChangeLookingDirection();
    }
}
