using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Spell
{
    [Header("This spell")]
    [SerializeField] BoxCollider2D col;

    float damageOfBurning;
    int timeOfBurning;

    void Start()
    {
        col.size = new Vector2(size, size);
        damage *= damageMultiplier;
        damageOfBurning = -Settings.damageOfBurning;
        timeOfBurning = Settings.timeOfBurning;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                Player player = collision.gameObject.GetComponent<Player>();
                player.ChangeHP(damage, typeOfDamage);
                player.Burn(damageOfBurning, timeOfBurning);
                break;
            case "Enemy":
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                enemy.ChangeHP(damage, typeOfDamage);
                enemy.Burn(damageOfBurning, timeOfBurning);
                break;
            case "Water":
                Destroy(gameObject);
                break;
            default: break;
        }
    }
}
