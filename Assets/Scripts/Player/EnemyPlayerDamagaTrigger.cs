using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayerDamagaTrigger : MonoBehaviour
{
    [Header("Settings")]
    public float timeBetweenDamage;

    [Header("SerializeFields")]
    [SerializeField] Enemy enemy;
    Player player;

    //local
    bool canDamage = true;

    //cors
    Coroutine givingDamageCor;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (canDamage) TakeDamage(enemy.damage);
    }

    void TakeDamage(float damage)
    {
        givingDamageCor = StartCoroutine(GivingDamageCor());
        player.ChangeHP(-damage);
    }
    IEnumerator GivingDamageCor()
    {
        canDamage = false;
        yield return new WaitForSeconds(timeBetweenDamage);
        canDamage = true;

        givingDamageCor = null;
    }
}
