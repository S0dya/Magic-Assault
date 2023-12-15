using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingObjectHealthRestore : FollowingObject
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(FollowPlayerCor());
    }

    protected override IEnumerator FollowPlayerCor()
    {
        yield return base.FollowPlayerCor();

        GetPlayer().ChangeHP(20, -1);

        Destroy(gameObject);
    }
}
