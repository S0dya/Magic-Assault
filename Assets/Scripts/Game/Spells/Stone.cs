using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : Spell
{
    //[Header("This spell")]

    float damage;

    void Start()
    {
        damage = -Settings.damageOfStone * damageMultiplier;
    }

    void OnParticleCollision(GameObject collisionObj)
    {
        if (collisionObj.CompareTag("Player"))
        {
            Player player = collisionObj.GetComponent<Player>();
            player.ChangeHP(damage);
        }

    }
}
