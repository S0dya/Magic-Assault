using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingObjectManaRestore : FollowingObject
{
    public float manaRestores;

    public void PerformAction() => GetPlayer().ChangeMana(manaRestores);
}
