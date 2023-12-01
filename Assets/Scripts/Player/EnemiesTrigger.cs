using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesTrigger : SingletonMonobehaviour<EnemiesTrigger>
{
    //local
    [HideInInspector] public List<Transform> enemiesTransforms = new List<Transform>();
    float timeForCheckTransforms;


    protected override void Awake()
    {
        base.Awake();

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
            if (transf == null) continue;
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
        Transform trans = enemiesTransforms[Random.Range(0, enemiesTransforms.Count)];
        return trans != null ? trans.position : LevelManager.I.GetRandomPos(1, 1);

    }

    public Vector2 GetRandomOffsetPos(float offset)
    {
        //get random position based on current position
        Vector2 pos = transform.position;
        return LevelManager.I.GetRandomOffsetPos(pos.x + offset, pos.y + offset, offset);
    }

    public bool HasEnemiesNearPlayer()
    {
        return enemiesTransforms.Count > 0;
    }
}
