using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindLine : CircleSpell
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                Player player = collision.gameObject.GetComponent<Player>();
                player.ChangeHP(damage, typeOfDamage);
                player.Push(((Vector2)player.transform.position - (Vector2)transform.position).normalized, 0.2f);

                Destroy(gameObject);
                break;
            case "Enemy":
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                enemy.ChangeHP(damage, typeOfDamage);
                enemy.Push(((Vector2)enemy.transform.position - (Vector2)transform.position).normalized, 0.2f);

                Destroy(gameObject);
                break;
            case "CircleEffect":
            case "EarthEffect":
                Destroy(gameObject);
                break;
            default: break;
        }
    }
}
