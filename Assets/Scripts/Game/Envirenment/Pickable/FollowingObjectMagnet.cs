using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingObjectMagnet : FollowingObject
{
    public void PerformAction() => LevelManager.I.MagnetExps();
}
