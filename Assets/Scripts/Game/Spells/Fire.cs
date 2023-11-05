using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Spell
{
    [Header("This spell")]
    [SerializeField] BoxCollider2D collider;

    float damage;

    void Start()
    {
        collider.size = new Vector2(size, size);
        damage = -Settings.damageOfFire * damageMultiplier;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            player.StartBurning(damage);
        }

    }
}
