using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Windball : CircleSpell
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
                if (!worksForPlayer) return;

                Player player = collision.gameObject.GetComponent<Player>();
                player.ChangeHP(damage, typeOfDamage);
                player.Push(direction, size);
                DestroyObj();
                break;
            case "Enemy":
                if (!worksForEnemy) return;

                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                enemy.ChangeHP(damage, typeOfDamage);
                enemy.Push(direction, size);
                DestroyObj();
                break;
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
