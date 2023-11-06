using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : Spell
{
    [Header("This spell")]
    [SerializeField] BoxCollider2D col;

    float damage;

    void Start()
    {
        col.size = new Vector2(size, size);
        damage = -Settings.damageOfFire * damageMultiplier;

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                Player player = collision.gameObject.GetComponent<Player>();
                player.Push(transform.position);
                player.ChangeHP(damage);
                break;
            case "Enemy":
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                enemy.ChangeHP(damage);
                break;
            case "Water":
                Destroy(gameObject);
                break;
            default: break;
        }
    }
}
