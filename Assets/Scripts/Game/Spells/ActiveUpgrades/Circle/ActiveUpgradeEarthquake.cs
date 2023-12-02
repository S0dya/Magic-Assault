using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveUpgradeEarthquake : CircleSpell
{
    List<Enemy> enemies = new List<Enemy>();

    protected override void Start()
    {
        base.Start();

        StartCoroutine(QuakeCor());
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        enemies.Add(enemy);
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        enemies.Remove(enemy);
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

            yield return new WaitForSeconds(1f);
        }
    }
}
