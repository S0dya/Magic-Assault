using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : Spell
{
    //[Header("This spell")]

    float damage;
    Vector2 posOfPush;

    void Start()
    {
        damage = -Settings.damageOfStone * damageMultiplier;

        float radians = (angle * Mathf.Deg2Rad) + 1.5f;
        posOfPush = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
    }

    void OnParticleCollision(GameObject collisionObj)
    {
        switch (collisionObj.tag)//change later
        {
            case "Enemy":
                Enemy enemy = collisionObj.GetComponentInParent<Enemy>();
                enemy.Push(posOfPush, size);
                enemy.ChangeHP(damage);
                break;
            default: break;
        }

        Destroy(gameObject);
    }
}
