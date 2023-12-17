using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveUpgradeEarthquake : CircleSpell
{
    List<Enemy> enemies = new List<Enemy>();
    List<Spawner> spawners = new List<Spawner>();

    protected override void Start()
    {
        base.Start();

        StartCoroutine(QuakeCor());
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        switch(collision.tag)
        {
            case "Enemy":
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                enemies.Add(enemy);
                break;
            case "ObstacleSpawner":
                Spawner spawner = collision.gameObject.GetComponent<Spawner>();
                spawners.Add(spawner);
                break;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Enemy":
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                enemies.Remove(enemy);
                break;
            case "ObstacleSpawner":
                Spawner spawner = collision.gameObject.GetComponent<Spawner>();
                spawners.Remove(spawner);
                break;
        }
    }

    IEnumerator QuakeCor()
    {
        while (true)
        {
            foreach (Enemy enemy in new List<Enemy>(enemies))
            {
                if (enemy == null) continue;

                enemy.Stun(0.2f);
                enemy.ChangeHP(-3, 2);
            }
            foreach (Spawner spawner in new List<Spawner>(spawners)) if (spawner == null) spawner.ChangeHP(-3);

            yield return new WaitForSeconds(1f);
        }
    }
}
