using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingObjectCoin : FollowingObject
{
    [Header("Settings of coin")]
    public int coinsOnCollect;

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

        UIInGame.I.ChangeMoney(coinsOnCollect);

        Destroy(gameObject);
    }
}
