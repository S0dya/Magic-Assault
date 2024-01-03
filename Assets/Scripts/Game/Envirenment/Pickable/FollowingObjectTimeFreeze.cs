using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingObjectTimeFreeze : FollowingObject
{
    public void PerformAction() => LevelManager.I.FreezeEnemies();
}
