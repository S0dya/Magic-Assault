using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUpgradePanel : SingletonMonobehaviour<UIUpgradePanel>
{
    [Header("Settings")]
    [SerializeField] UIGameMenu uiGameMenu;
    [SerializeField] UIInGame uiInGame;

    [Header("Upgrade")]
    [SerializeField] GameObject upgradeTab;
    [SerializeField] RectTransform upgradeTabTransform;
    [SerializeField] CanvasGroup upgradeTabCG;

    //local

    protected override void Awake()
    {
        base.Awake();

    }

    public void Start()
    {
        upgradeTabTransform.anchoredPosition = new Vector2(0, -Screen.height);
    }

    public void OpenUpgradeTab()
    {
        GameManager.I.MoveTransform(upgradeTabTransform, 0, 0, 0.75f);
        GameManager.I.Open(upgradeTabCG, 0.75f);

        uiGameMenu.ToggleTimeScale(false);
    }

    public void CloseUpgradeTab()
    {
        GameManager.I.MoveTransform(upgradeTabTransform, 0, -Settings.height, 0.25f);
        GameManager.I.Close(upgradeTabCG, 0.25f);

        uiGameMenu.ToggleTimeScale(true);
    }
}
