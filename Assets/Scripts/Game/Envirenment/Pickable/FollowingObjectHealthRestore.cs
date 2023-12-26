using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingObjectHealthRestore : FollowingObject
{
    public float hpRestores;

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

        GetPlayer().ChangeHP(hpRestores, -1);

        Destroy(gameObject);
    }
}
