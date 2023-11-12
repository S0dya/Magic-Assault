using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayerDamagaTriggerMelee : MonoBehaviour
{
    [Header("Settings")]
    public float timeBetweenDamage;

    [Header("SerializeFields")]
    [SerializeField] Enemy enemy;
    Player player;

    //local
    bool canDamage = true;
    bool onTrigger;

    //cors
    Coroutine givingDamageCor;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    //triggers
    void OnTriggerEnter2D(Collider2D Collision)
    {
        ToggleOnTrigger(true);
        
        if (givingDamageCor == null) givingDamageCor = StartCoroutine(GivingDamageCor());
    }
    void OnTriggerExit2D(Collider2D Collision)
    {
        ToggleOnTrigger(false);
    }

    //other methods
    void GiveDamage()
    {
        if (canDamage)
        {
            player.ChangeHP(-enemy.damageOnTriggerWithPlayer, enemy.typeOfDamageOnTriggerWithPlayer);

        }

    }
    IEnumerator GivingDamageCor()
    {
        while (onTrigger)
        {
            GiveDamage();

            canDamage = false;
            yield return new WaitForSeconds(timeBetweenDamage);
            canDamage = true;
        }

        givingDamageCor = null;
    }

    void ToggleOnTrigger(bool val)
    {
        onTrigger = val;
        enemy.ToggleMovementSpeedOnPlayerTrigger(val);
    }
}
