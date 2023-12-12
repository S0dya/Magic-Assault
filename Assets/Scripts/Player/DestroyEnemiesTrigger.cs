using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEnemiesTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject enemyObj = collision.gameObject;
        Destroy(collision.gameObject);
    }
}
