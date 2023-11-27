using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayer : Enemy
{
    //movement
    protected override void Update()
    {
        DirectionOfMovement = (playerTransform.position - transform.position).normalized;

        base.Update();
    }
}
