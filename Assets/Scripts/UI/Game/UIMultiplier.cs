using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMultiplier : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] TextMeshProUGUI multiplierText;

    //local
    UIMultipliers uiMultipliers;

    void Start()
    {
        uiMultipliers = GetComponentInParent<UIMultipliers>();
    }

    //buttons 
    public void IncreaseMultiplierButton(int index)
    {
        uiMultipliers.Increase(index);
    }

    public void DecreaseMultiplierButton(int index) 
    {
        uiMultipliers.Decrease(index);
    }

    public void SetMultiplierText(float val) => multiplierText.text = val.ToString();
}
