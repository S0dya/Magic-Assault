using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayerDamagaTrigger : MonoBehaviour
{
    [Header("Settings")]
    public float timeBetweenDamage;

    [Header("SerializeFields")]
    [SerializeField] Player player;

    //local
    bool canTakeDamage = true;

    //cors
    Coroutine takingDamageCor;

    void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("as");
        if (canTakeDamage && collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            TakeDamage(enemy.damage);
        }
    }

    void TakeDamage(float damage)
    {
        takingDamageCor = StartCoroutine(TakingDamageCor());
        player.ChangeHP(-damage);
    }
    IEnumerator TakingDamageCor()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(timeBetweenDamage);
        canTakeDamage = true;

        takingDamageCor = null;
    }
}
