using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingObjectSpellChest : FollowingObject
{
    public void PerformAction() => UIInGame.I.OpenSpellsPanel();
}
