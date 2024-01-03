using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingObjectHealthRestore : FollowingObject
{
    public float hpRestores;

    public void PerformAction() => GetPlayer().RestoreHPWithItem(hpRestores);
}
