using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIItem : MonoBehaviour
{
    [SerializeField] Image outsideImage;

    [SerializeField] GameObject imageObj;
    [SerializeField] Image image;

    [SerializeField] CanvasGroup textCG;
    [SerializeField] TextMeshProUGUI nameOfUpgrade;
    [SerializeField] TextMeshProUGUI description;

    [HideInInspector] public int amountOfClicks;

    protected int AmountOfClicks { get { return amountOfClicks; } set { amountOfClicks = value; } }

    //set
    public virtual void SetInfo(SO_GameItem item)
    {
        image.sprite = item.ItemImage;
        nameOfUpgrade.text = item.Name;
        description.text = item.Description;
    }

    //visualisation
    public virtual void Open()
    {
        GameManager.I.ChangeScale(imageObj, 1.5f, 0.4f);
        GameManager.I.Open(textCG, 0.4f);
        outsideImage.enabled = true;
    }

    public virtual void Hide()
    {
        amountOfClicks = 0;

        GameManager.I.ChangeScale(imageObj, 1, 0.2f);
        GameManager.I.Close(textCG, 0.2f);
        outsideImage.enabled = false;
    }
}
