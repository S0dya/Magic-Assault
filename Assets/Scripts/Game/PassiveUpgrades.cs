using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PassiveUpgrades : SingletonMonobehaviour<PassiveUpgrades>
{
    [Header("OtherScripts")]
    [SerializeField] SpellsManager spellsManager;

    public List<PassiveUpgrade> upgrades;
    public List<PassiveUpgrade> passiveActiveUpgrades;

    //local
    Player player;
    ActiveUpgrades activeUpgrades;
    GameData gameData;

    //treshhold
    PassiveUpgrade curUpgrade;

    void Start()
    {
        foreach (var passiveActiveUpgrade in passiveActiveUpgrades) upgrades.Add(passiveActiveUpgrade);

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        activeUpgrades = player.gameObject.GetComponent<ActiveUpgrades>();
        gameData = GameData.I;
    }

    //main methods
    public bool CanPerformPassiveUpgrade(UpgradeType upgradeType)//find item with same upgrade type and increase its amount. if amount reaches its limit - remove this item from list
    {
        SetUpgrade(upgradeType);

        return PerformPassiveUpgrade();
    }
    public bool CanPerformActivePassiveUpgrade(UpgradeType upgradeType)
    {
        SetUpgrade(upgradeType);

        return PerformActivePassiveUpgrade();
    }

    bool PerformPassiveUpgrade()
    {
        curUpgrade.upgradeEventPassive.Invoke();

        return IncreaseAmountAndCheckIfLimitIsReached();
    }

    bool PerformActivePassiveUpgrade()
    {
        curUpgrade.upgradeEventActivePassive.Invoke(curUpgrade.upgradeTypeActivePassive);

        return IncreaseAmountAndCheckIfLimitIsReached();
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
    public void IncreaseActiveAmount(UpgradeType upgradeType)
    {
        activeUpgrades.IncreaseAmount(upgradeType);
    }

    public void IncreaseActiveArea(UpgradeType upgradeType)
    {
        activeUpgrades.IncreaseArea(upgradeType);
    }

    public void DecreaseActiveCooldown(UpgradeType upgradeType)
    {
        activeUpgrades.DecreaseCooldown(upgradeType);
    }

    //other
    void SetUpgrade(UpgradeType upgradeType)
    {
        foreach (var upgrade in upgrades)
        {
            if (upgrade.upgradeType == upgradeType)
            {
                curUpgrade = upgrade;

                break;
            }
        }
    }

    bool IncreaseAmountAndCheckIfLimitIsReached()
    {
        curUpgrade.curAmount++;

        bool reachedLimit = curUpgrade.curAmount == curUpgrade.amountLimit;

        if (reachedLimit) upgrades.Remove(curUpgrade);

        return reachedLimit;
    }
}

[System.Serializable]
public class PassiveUpgrade
{
    
    [Header("Upgrade info")]
    [SerializeField] public UpgradeType upgradeType;

    [Header("Passive")] [SerializeField] public UnityEvent upgradeEventPassive;
    [Header("Active-Passive")]
    [SerializeField] public UnityEvent<UpgradeType> upgradeEventActivePassive;
    [SerializeField] public UpgradeType upgradeTypeActivePassive;

    [Header("Settings")]
    [SerializeField] public int amountLimit = 5;
    [SerializeField] public int curAmount;
}