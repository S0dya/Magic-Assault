using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingObjectMultipliersScroll : FollowingObject
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(FollowPlayerCor());
    }

    protected override IEnumerator FollowPlayerCor()
    {
        yield return base.FollowPlayerCor();

        UIInGame.I.OpenMultipliersPanel();

        Destroy(gameObject);
    }
}
