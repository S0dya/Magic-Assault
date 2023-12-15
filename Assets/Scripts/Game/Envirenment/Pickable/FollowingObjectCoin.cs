using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingObjectCoin : FollowingObject
{
    [Header("Settings of coin")]
    public int coinsOnCollect;

    void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(FollowPlayerCor());
    }

    protected override IEnumerator FollowPlayerCor()
    {
        yield return base.FollowPlayerCor();

        UIInGame.I.ChangeMoney(coinsOnCollect);

        Destroy(gameObject);
    }
}
