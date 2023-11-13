using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : Spell
{
    [Header("This spell")]
    [SerializeField] CircleCollider2D col;

    //local
    Player player;
    bool isPlayerFound;

    void Start()
    {
        col.radius = size / 2;
    }

    //disable for proper trigger detection
    void OnDisable()
    {
        col.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                if (!isPlayerFound) FindPlayer();
                player.EnterLava();
                break;
            case "Enemy":
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                enemy.EnterLava();
                break;
            default: break;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                if (!isPlayerFound) FindPlayer();
                player.ExitLava();
                break;
            case "Enemy":
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                enemy.ExitLava();
                break;
            default: break;
        }
    }

    void FindPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        isPlayerFound = true;
    }
}
