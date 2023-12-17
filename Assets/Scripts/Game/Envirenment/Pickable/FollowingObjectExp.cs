using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingObjectExp : FollowingObject
{
    [Header("Settings of exp")]
    public float expOnCollect;

    bool isFollowing;

    void OnTriggerEnter2D(Collider2D collision)
    {
        StartFollowingPlayer();
    }

    //outside method
    public void StartFollowingPlayer()
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

        UIInGame.I.ChangeExp(expOnCollect);

        Destroy(gameObject);
    }
}
