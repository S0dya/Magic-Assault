using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRestoreItem : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        player.ChangeHP(20, -1);
        Destroy(gameObject);
    }
}
