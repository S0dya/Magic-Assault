using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesTrigger : SingletonMonobehaviour<EnemiesTrigger>
{
    //local
    LevelManager levelManager;

    [HideInInspector] public List<Transform> enemiesTransforms = new List<Transform>();
    float timeForCheckTransforms;


    protected override void Awake()
    {
        base.Awake();

        timeForCheckTransforms = Settings.timeForCheckTransforms;
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
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
        return trans != null ? trans.position : levelManager.GetRandomPos(1, 1);

    }

    public bool HasEnemiesNearPlayer()
    {
        return enemiesTransforms.Count > 0;
    }
}
