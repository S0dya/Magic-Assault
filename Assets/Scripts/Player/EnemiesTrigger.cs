using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesTrigger : MonoBehaviour
{
    //local
    List<Transform> enemiesTransforms = new List<Transform>();
    float timeForCheckTransforms;


    void Awake()
    {
        timeForCheckTransforms = Settings.timeForCheckTransforms;
    }

    void Start()
    {
    }

    //trigers
    void OnTriggerEnter2D(Collider2D collision)
    {
        enemiesTransforms.Add(collision.transform);
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        enemiesTransforms.Remove(collision.transform);
    }

    //main methods for active upgrades 
    public Vector2 GetNearestEnemyPosition()
    {
        //set private values
        float nearestDistance = float.MaxValue;
        Vector2 curNearest = new Vector2();

        //check nearest transform based on distance
        foreach (Transform transf in enemiesTransforms)
        {
            float curDistance = Vector2.Distance(transf.position, transform.position);
            if (nearestDistance > curDistance)
            {
                nearestDistance = curDistance;
                curNearest = transf.position;
            }
        }

        return curNearest;
    }

    public Vector2 GetRandomEnemyPosition()
    {
        //get random enemy near player
        return enemiesTransforms[Random.Range(0, enemiesTransforms.Count)].position;
    }

    public Vector2 GetRandomPosition()
    {
        //get random position based on current position
        Vector2 pos = transform.position;
        return new Vector2(Random.Range(pos.x - 1,pos.x + 1), Random.Range(pos.y - 1,pos.y + 1));
    }

    public bool HasEnemiesNearPlayer()
    {
        return enemiesTransforms.Count > 0;
    }
}
