using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemiesTrigger : MonoBehaviour
{
    //local
    LevelManager levelManager;

    void Start() => levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();

    void OnTriggerEnter2D(Collider2D collision)
    {
        levelManager.MoveEnemy(collision.gameObject.transform);

        Enemy enemy = collision.GetComponent<Enemy>(); //change looking direction if needed after moving enemy
        enemy.CheckLookingDirection();
    }
}
