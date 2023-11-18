using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayerDamagaTriggerDistance : MonoBehaviour
{
    [Header("Settings")]
    public float timeBetweenAttacks;
    public float timeForAttack;
    public float timeAfterAttack;

    public float size;

    [Header("SerializeFields")]
    [SerializeField] GameObject spell;
    [SerializeField] Enemy enemy;

    Transform effectsParent;
    Player player;
    Transform playerTransform;

    //local
    bool canAttack;
    bool onTrigger;

    //cors
    Coroutine attackingCor;

    void Awake()
    {
        effectsParent = GameObject.FindGameObjectWithTag("EffectsParent").GetComponent<Transform>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        playerTransform = player.transform;
    }

    //triggers
    void OnTriggerEnter2D(Collider2D Collision)
    {
        ToggleOnTrigger(true);

        if (attackingCor == null) attackingCor = StartCoroutine(AttackingCor());
    }
    void OnTriggerExit2D(Collider2D Collision)
    {
        ToggleOnTrigger(false);
    }

    //other methods
    void Attack()
    {
        if (onTrigger && !enemy.isPushed)
        {
            Vector2 posOfEnemy = transform.position;
            Vector2 direction = ((Vector2)playerTransform.position - posOfEnemy).normalized;
            float rotation = (Mathf.Atan2(direction.y, direction.x) - 1.5f) * Mathf.Rad2Deg;
            SpellsManager.I.InstantiateEffect(spell, posOfEnemy + (direction/2), size, direction, rotation);
        }
    }
    IEnumerator AttackingCor()
    {
        while (onTrigger)
        {
            yield return new WaitForSeconds(timeBetweenAttacks);
            enemy.ToggleMovement(false);
            yield return new WaitForSeconds(timeForAttack);
            Attack();
            yield return new WaitForSeconds(timeAfterAttack);
            enemy.ToggleMovement(true);
        }

        attackingCor = null;
    }

    void ToggleOnTrigger(bool val)
    {
        onTrigger = val;
        enemy.ToggleMovement(val);
    }
}
