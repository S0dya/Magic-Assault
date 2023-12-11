using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// base script for UI interaction of Upgrades, Multipliers, Spells and GameMenu.
/// </summary>

public class UIPanelGame : UIPanel
{
    [Header("Other scripts base")]
    public DrawManager drawManager;

    //main methods
    public virtual void OpenTab()
    {
        MoveTab(1, 0.45f);
        GameManager.I.Open(panelCG, 0.75f);
        
        ToggleTimeScale(false);
        drawManager.StopCreatingSpell();
    }

    public virtual void CloseTab()
    {
        MoveTab(0, 0.46f);
        GameManager.I.Close(panelCG, 0.25f);

        ToggleTimeScale(true);
    }


    //other methods
    void ToggleTimeScale(bool val)
    {
        float floatVal = val ? 1 : 0;

        StartCoroutine(ToggleOnUICor(!val));
        Time.timeScale = floatVal;

        UIInGame.I.ToggleJoystickVisibility(floatVal);
    }

    //coroutine will make sure ui interaction is not considered in-game interaction
    IEnumerator ToggleOnUICor(bool val)
    {
        yield return null;

        drawManager.isOnUI = val;
        if (!val) drawManager.StopCreatingSpell();
    }
}
