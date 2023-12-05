using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIMultiplierButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Settings")]
    [SerializeField] Button button;
    
    //local 
    Coroutine buttonPressedCor;

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressedCor = StartCoroutine(ButtonPressedCor());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (buttonPressedCor != null) StopCoroutine(buttonPressedCor);
    }

    IEnumerator ButtonPressedCor()
    {
        yield return new WaitForSecondsRealtime(0.1f);

        while (true)
        {
            button.onClick.Invoke();

            yield return new WaitForSecondsRealtime(0.25f);
        }
    }
}
