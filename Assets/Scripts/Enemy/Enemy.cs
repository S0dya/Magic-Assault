using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Creature
{
    Transform playerTransform;
    
    [Header("Enemy settings")]
    public float speedOnTriggerWithPlayer;
    public float damageOnTriggerWithPlayer;

    //local
    //bool canMove = true;

    //cors
    Coroutine burningCor;
    Coroutine waitForPushEndCor;

    void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    protected override void Update()
    {
        DirectionOfMovement = (playerTransform.position - transform.position).normalized;

        base.Update();
    }

    public void ToggleMovement(bool val) => CanMove = val;

    public void ToggleMovementSpeedOnPlayerTrigger(bool val)
    {
        if (val) CurMovementSpeed = speedOnTriggerWithPlayer;
        else CurMovementSpeed = movementSpeed;
    }

    //health
    public override void ChangeHP(float val)
    {
        base.ChangeHP(val);

        if (curHp == 0)
        {
            Destroy(gameObject);
        }
    }
}
