using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : Spell
{
    [Header("This spell")]
    [SerializeField] BoxCollider2D col;

    Vector2 posOfPush;

    void Start()
    {
        col.size = new Vector2(size, size);
        damage *= damageMultiplier;

        // Calculate the direction vector using trigonometry
        float radians = (rotation * Mathf.Deg2Rad) + 1.5f;
        posOfPush = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                Player player = collision.gameObject.GetComponent<Player>();
                player.Push(posOfPush, size);
                player.ChangeHP(damage, typeOfDamage);
                break;
            case "Enemy":
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                enemy.Push(posOfPush, size);
                enemy.ChangeHP(damage, typeOfDamage);
                break;
            default: break;
        }
    }
}
