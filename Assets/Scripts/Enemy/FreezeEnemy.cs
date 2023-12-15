using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeEnemy : MonoBehaviour
{
    [SerializeField] Enemy enemy;

    [Header("sprite visuals")]
    [SerializeField] Color freezedColor;

    //cor
    Coroutine freezeCor;

    public void Freeze()
    {
        enemy.ChangeSrColor(freezedColor);

        ToggleScriptsAndColliders(false);

        if (freezeCor != null) StopCoroutine(freezeCor); 
        freezeCor = StartCoroutine(FreezeCor());
    }

    IEnumerator FreezeCor()
    {
        yield return new WaitForSeconds(Settings.freezeTime);

        Unfreeze();
        freezeCor = null;
    }
    
    void Unfreeze()
    {
        enemy.SetNormalColor();
     
        ToggleScriptsAndColliders(true);
    }

    //other
    void ToggleScriptsAndColliders(bool toggle)
    {
        enemy.enabled = toggle;
        enemy.ToggleDamageColliders(toggle);
    }
}
