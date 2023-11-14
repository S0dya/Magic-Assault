using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterLine : Spell
{
    [Header("This spell")]
    [SerializeField] BoxCollider2D col;

    void Start()
    {
        col.size = new Vector2(size, size);
        damage *= damageMultiplier;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                Player player = collision.gameObject.GetComponent<Player>();
                player.ChangeHP(damage, typeOfDamage);
                player.HandleWater();
                Destroy(gameObject);
                break;
            case "Enemy":
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                enemy.ChangeHP(damage, typeOfDamage);
                enemy.HandleWater();
                Destroy(gameObject);
                break;
            case "CircleEffect":
                Destroy(gameObject);
                break;
            default: break;
        }
    }
}
