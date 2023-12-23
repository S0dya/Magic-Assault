using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingObjectRedSkull : FollowingObject
{
    bool isFollowing;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isFollowing)
        {
            isFollowing = true;
            StartCoroutine(FollowPlayerCor());
            LevelManager.I.KillEnemies();
        }
    }

    protected override IEnumerator FollowPlayerCor()
    {
        yield return base.FollowPlayerCor();

        

        Destroy(gameObject);
    }
}
