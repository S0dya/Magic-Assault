using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [HideInInspector] public Vector2 position;

    public void TriggerEntered(Vector2 direction)//create new level
    {
        LevelManager.I.GenerateLevel(direction + position);
    }
}
