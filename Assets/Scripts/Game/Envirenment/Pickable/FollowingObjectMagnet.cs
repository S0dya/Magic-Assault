using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingObjectMagnet : FollowingObject
{
    bool isFollowing;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isFollowing)
        {
            isFollowing = true;
            StartCoroutine(FollowPlayerCor());
        }
    }

    protected override IEnumerator FollowPlayerCor()
    {
        yield return base.FollowPlayerCor();

        LevelManager.I.MagnetExp();

        Destroy(gameObject);
    }
}
