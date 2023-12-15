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
        MoveTab(1, 0.45f);
        GameManager.I.Open(panelCG, 0.75f);

        UIInGame.I.StopTimeScale();
    }

    public virtual void CloseTab()
    {
        MoveTab(0, 0.46f);
        GameManager.I.Close(panelCG, 0.25f);

        UIInGame.I.ToggleTimeScale(true);
    }
}
