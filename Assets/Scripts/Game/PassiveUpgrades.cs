using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveUpgrades : SingletonMonobehaviour<PassiveUpgrades>
{
    [Header("Settings")]
    public int increasePlayerSpeedAmount;
    public float increasePlayerSpeedBy;

    [Header("Other")]

    //local
    Player player;

    //cur amounts of passive upgrades
    int curIncreasePlayerSpeedAmount;

    protected override void Awake()
    {
        base.Awake();

    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    //upgrades 
    public bool CanIncreasePlayerSpeed()
    {
        curIncreasePlayerSpeedAmount++;
        player.movementSpeed += increasePlayerSpeedBy;

        return curIncreasePlayerSpeedAmount < increasePlayerSpeedAmount;
    }
}
