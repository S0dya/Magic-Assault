using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PassiveUpgrades : SingletonMonobehaviour<PassiveUpgrades>
{
    public List<PassiveUpgrade> upgrades;

    //local
    Player player;

    //treshhold
    PassiveUpgrade curUpgrade;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    //main method
    public bool CanPerformPassiveUpgrade(UpgradeType upgradeType)//find item with same upgrade type and increase its amount. if amount reaches its limit - remove this item from list
    {
        foreach (var upgrade in upgrades)
        {
            if (upgrade.upgradeType == upgradeType)
            {
                curUpgrade = upgrade;

                break;
            }
        }

        return IncreaseAmountAndCheckIfLimitIsReached();
    }

    bool IncreaseAmountAndCheckIfLimitIsReached()
    {
        curUpgrade.upgradeEvent.Invoke();
        curUpgrade.curAmount++;

        bool reachedLimit = curUpgrade.curAmount != curUpgrade.amountLimit;

        if (reachedLimit) upgrades.Remove(curUpgrade);

        return reachedLimit;
    }

    //upgrades 
    public void IncreasePlayerSpeed()
    {
        player.movementSpeed *= 1.1f;
    }
}

[System.Serializable]
public class PassiveUpgrade
{
    [Header("Upgrade info")]
    [SerializeField] public UpgradeType upgradeType;
    [SerializeField] public UnityEvent upgradeEvent;
 
    [Header("Settings")]
    [SerializeField] public int amountLimit = 5;
    [SerializeField] public int curAmount;
}