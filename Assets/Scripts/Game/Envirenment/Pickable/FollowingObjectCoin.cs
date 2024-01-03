using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingObjectCoin : FollowingObject
{
    public float coinsOnCollect;

    public void PerformAction() => UIInGame.I.ChangeMoney(coinsOnCollect);
}
