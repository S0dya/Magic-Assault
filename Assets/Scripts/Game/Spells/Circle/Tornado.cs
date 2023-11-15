using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : CircleSpell
{
    //local
    Player player;
    bool isPlayerFound;

    void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                if (!isPlayerFound) FindPlayer();
                player.EnterTornado(transform.position);
                break;
            case "Enemy":
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                enemy.EnterTornado(transform.position);
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
                player.ExitTornado();
                break;
            case "Enemy":
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                enemy.ExitTornado();
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
