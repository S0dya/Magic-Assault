using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : Spell
{
    [Header("This spell")]
    [SerializeField] CircleCollider2D col;

    void Start()
    {
        col.radius = size/2;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                Player player = collision.gameObject.GetComponent<Player>();
                player.EnterWater();
                break;
            case "Enemy":
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                enemy.EnterWater();
                break;
            default: break;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                Player player = collision.gameObject.GetComponent<Player>();
                player.ExitWater();
                break;
            case "Enemy":
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                enemy.ExitWater();
                break;
            default: break;
        }
    }
}
