using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpFollowingObject : FollowingObject
{
    [Header("Settings of exp")]
    public float expOnCollect;

    void OnTriggerEnter2D(Collider2D collision)
    {
        StartFollowingPlayer();
    }

    public void StartFollowingPlayer()
    {
        StartCoroutine(FollowPlayerCor());
    }

    protected override IEnumerator FollowPlayerCor()
    {
        yield return base.FollowPlayerCor();

        UIInGame.I.ChangeExp(expOnCollect);
    }
}
