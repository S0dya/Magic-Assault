using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayer : Enemy
{
    protected override void Start()
    {
        base.Start();

        CheckLookingDirection();
    }

    //movement
    protected override void Update()
    {
        DirectionOfMovement = (playerTransform.position - transform.position).normalized;

        base.Update();
    }
}
