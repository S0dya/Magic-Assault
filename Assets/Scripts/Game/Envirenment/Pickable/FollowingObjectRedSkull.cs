using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingObjectRedSkull : FollowingObject
{
    public void PerformAction() => LevelManager.I.KillEnemies();
}
