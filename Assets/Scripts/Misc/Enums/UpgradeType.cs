public enum UpgradeType 
{
    //active
    ActiveDotInNearestEnemy,
    ActiveDotInRandomEnemy,
    ActiveDotInRandomPosition,
    ActiveDotInMovementDirection,

    ActiveCircleInRandomPosition,

    //passive
    PassiveIncreasePower,
    PassiveAddArmor,
    PassiveToggleHealthRecovery,
    PassiveIncreaseHealthRecovery,
    PassiveIncreaseMaxHealth,
    PassiveDecreaseCooldown,
    PassiveIncreaseArea,
    PassiveIncreaseGeneralAmount,
    PassiveIncreaseSpeed,
    PassiveIncreaseLifetime,
    PassiveIncreaseGrowth,
    PassiveIncreaseGreed,

    //passive-active
    /*
    PassiveActiveIncreaseAmount,
    PassiveActiveIncreaseArea,
    PassiveActiveDecreaseCooldown,
    */

    PassiveActiveIncreaseAmountDotInNearestEnemy,
    PassiveActiveIncreaseAreaDotInNearestEnemy,
    PassiveActiveDecreaseCooldownDotInNearestEnemy,

    PassiveActiveIncreaseAmountDotInRandomEnemy,
    PassiveActiveIncreaseAreaDotInRandomEnemy,
    PassiveActiveDecreaseCooldownDotInRandomEnemy,

    PassiveActiveIncreaseAmountDotInRandomPosition,
    PassiveActiveIncreaseAreaDotInRandomPosition,
    PassiveActiveDecreaseCooldownDotInRandomPosition,

    PassiveActiveIncreaseAmountDotInMovementDirection,
    PassiveActiveIncreaseAreaDotInMovementDirection,
    PassiveActiveDecreaseCooldownDotInMovementDirection,

    PassiveActiveIncreaseAmountCircleInRandomPosition,
    PassiveActiveIncreaseAreaCircleInRandomPosition,
    PassiveActiveDecreaseCooldownCircleInRandomPosition,

    //additional upgrades items/ pick ups
    PickUpsManaPotion,
    PickUpsCoinsBag,
    PickUpsHealthPotion,
}

