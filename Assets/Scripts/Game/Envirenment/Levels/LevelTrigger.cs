using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTrigger : MonoBehaviour
{
    [SerializeField] Level level;
    public Vector2 directionOfCollider;

    void OnTriggerEnter2D(Collider2D collision) //only collision is player's trigger
    {
        level.TriggerEntered(directionOfCollider);
        Destroy(gameObject);
    }
}
