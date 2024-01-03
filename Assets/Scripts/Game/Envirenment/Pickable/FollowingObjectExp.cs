using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingObjectExp : FollowingObject
{
    public float expOnCollect;

    public void PerformAction() => UIInGame.I.ChangeExp(expOnCollect);
}
