using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PassiveUpgrades : SingletonMonobehaviour<PassiveUpgrades>
{
    [Header("OtherScripts")]
    [SerializeField] SpellsManager spellsManager;

    public List<PassiveUpgrade> upgrades;

    //local
    Player player;
    ActiveUpgrades activeUpgrades;
    GameData gameData;

    //treshhold
    PassiveUpgrade curUpgrade;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        activeUpgrades = player.gameObject.GetComponent<ActiveUpgrades>();
        gameData = GameData.I;
    }

    //main methods
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

        bool reachedLimit = curUpgrade.curAmount == curUpgrade.amountLimit;

        if (reachedLimit) upgrades.Remove(curUpgrade);

        return reachedLimit;
    }

    //general passive upgrades 
    public void IncreasePower()
    {
        gameData.power += 0.1f;
    }

    public void AddArmor()
    {
        player.AddShield();
    }

    public void HealthRecovery()
    {
        player.canRestoreHp = true;
    }
    public void IncreaseHealthRecovery()
    {
        player.amountOfTimeBeforeRestoringHp *= 0.9f;
        player.amountOfRestoringHp *= 1.1f;
    }

    public void IncreaseMaxHealth()
    {
        player.maxHp *= 1.1f;
    }

    public void DecreaseCooldown()
    {
        gameData.cooldown -= 0.1f;
    }

    public void IncreaseArea()
    {
        gameData.area += 0.05f;
    }

    public void IncreaseGeneralAmount()
    {
        spellsManager.IncreaseAmountOfSpells();
        activeUpgrades.IncreaseAmount();
    }

    public void IncreaseSpeed()
    {
        player.movementSpeed *= 1.1f;
    }

    public void IncreaseLifetime()
    {
        gameData.lifetimeMultiplier += 0.1f;
    }

    public void IncreaseGrowth()
    {
        gameData.growth += 0.05f;
    }

    public void IncreaseGreed()
    {
        gameData.greed += 0.1f;
    }

    //active upgrades' passive upgrades


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