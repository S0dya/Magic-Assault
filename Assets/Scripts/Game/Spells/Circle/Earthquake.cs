using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earthquake : CircleSpell
{
    //local
    Player player;
    bool isPlayerFound;
    bool playerOnTrigger;

    List<Enemy> enemies = new List<Enemy>();

    protected override void Start()
    {
        base.Start();

        StartCoroutine(QuakeCor());
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                //we dont need to add player script since we can reach it with tag
                if (!isPlayerFound) FindPlayer();
                playerOnTrigger = true;
                break;
            case "Enemy":
                //add enemy to list
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                enemies.Add(enemy);
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
                playerOnTrigger = false;
                break;
            case "Enemy":
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                enemies.Remove(enemy);
                break;
            default: break;
        }
    }

    IEnumerator QuakeCor()
    {
        //each specified time, earthquake deals damage
        while (true)
        {
            foreach (Enemy enemy in enemies)
            {
                enemy.Stun(0.2f);
                enemy.ChangeHP(-3, 2);
            }
            if (playerOnTrigger)
            {
                player.Stun(0.2f);
                player.ChangeHP(-3, 2);
            }


            yield return new WaitForSeconds(1f);
        }
    }

    void FindPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        isPlayerFound = true;
    }
}
