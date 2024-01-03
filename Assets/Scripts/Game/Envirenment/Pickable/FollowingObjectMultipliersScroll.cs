using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingObjectMultipliersScroll : FollowingObject
{
    public void PerformAction() => UIInGame.I.OpenMultipliersPanel();
}
