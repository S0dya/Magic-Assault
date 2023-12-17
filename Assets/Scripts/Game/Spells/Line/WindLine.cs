using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindLine : CircleSpell
{
    int amountOfPassTroughTriggers;

    protected override void Start()
    {
        base.Start();

        amountOfPassTroughTriggers = GameData.I.amountOfPassTroughTriggers;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                Player player = collision.gameObject.GetComponent<Player>();
                player.ChangeHP(damage, typeOfDamage);
                player.Push(((Vector2)player.transform.position - (Vector2)transform.position).normalized, 0.2f);

                DestroyObj();
                break;
            case "Enemy":
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                enemy.ChangeHP(damage, typeOfDamage);
                enemy.Push(((Vector2)enemy.transform.position - (Vector2)transform.position).normalized, 0.2f);

                DestroyObj();
                break;
            case "ObstacleSpawner":
                Spawner spawner = collision.gameObject.GetComponent<Spawner>();
                spawner.ChangeHP(damage);

                DestroyObj();
                break;
            case "CircleEffect":
            case "EarthEffect":
            case "ObjectCollider":
                DestroyObj();
                break;
            default: break;
        }
    }

    void DestroyObj()
    {
        if (amountOfPassTroughTriggers == 0) Destroy(gameObject);
        amountOfPassTroughTriggers--;
    }
}
