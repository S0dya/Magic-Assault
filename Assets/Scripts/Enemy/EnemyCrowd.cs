using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCrowd : Enemy
{
    [HideInInspector] public Vector2 directionOfMove;
    
    protected override void Start()
    {
        base.Start();

        CheckLookingDirection(directionOfMove.x);
    }

    //movement
    protected override void Update()
    {
        DirectionOfMovement = directionOfMove;

        base.Update();
    }


}
