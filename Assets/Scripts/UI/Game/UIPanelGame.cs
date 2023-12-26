using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// base script for UI interaction of Upgrades, Multipliers, Spells and GameMenu.
/// </summary>

public class UIPanelGame : UIPanel
{
    //main methods
    public virtual void OpenTab()
    {
        MoveTab(1, 0.5f);
        GameManager.I.Open(panelCG, 0.5f);

        UIInGame.I.StopTimeScale();
    }

    public virtual void CloseTab()
    {
        MoveTab(0, 0.15f);
        GameManager.I.Close(panelCG, 0.15f);

        UIInGame.I.ToggleTimeScale(true);
    }
}
