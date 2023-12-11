using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// base script for UI interaction of all main menu and game menu tabs.
/// </summary>

public class UIPanelMenu : UIPanel
{

    //main methods
    public virtual void OpenTab()
    {
        MoveTab(1, 0.25f);
        ToggleCGRaycast(true);
    }

    public virtual void CloseTab()
    {
        MoveTab(0, 0.26f);
        ToggleCGRaycast(false);
    }

    //other methods
    void ToggleCGRaycast(bool val) => panelCG.blocksRaycasts = val;

}
