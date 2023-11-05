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

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            player.ChangeStats(damage, 0);
        }

    }
}
