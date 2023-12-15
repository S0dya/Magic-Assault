using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingObjectTimeFreeze : FollowingObject
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(FollowPlayerCor());
    }

    protected override IEnumerator FollowPlayerCor()
    {
        yield return base.FollowPlayerCor();


        LevelManager.I.FreezeEnemies();
     
        Destroy(gameObject);
    }
}
