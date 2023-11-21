using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIUpgrade : MonoBehaviour
{
    [Header("SerialzieFields")]
    [SerializeField] GameObject imageObj;
    [SerializeField] Image image;

    [SerializeField] CanvasGroup textCG;
    [SerializeField] TextMeshProUGUI nameOfUpgrade;
    [SerializeField] TextMeshProUGUI description;

    UIUpgradePanel uiUpgradePanel;

    //local
    [HideInInspector] public int amountOfClicks;

    void Start()
    {
        uiUpgradePanel = UIUpgradePanel.I;
    }

    public void SetInfo(SO_Item item)
    {
        image.sprite = item.ItemImage;
        nameOfUpgrade.text = item.Name;
        description.text = item.Description;
    }

    //button
    public void OnImageButton(int index)
    {
        if (amountOfClicks == 1)
        {
            uiUpgradePanel.ChooseUpgrade(index);

            Hide();
            return;
        }
        amountOfClicks++;

        uiUpgradePanel.VisualiseChooseOfAnItem(index);
    }

    public void Open()
    {
        GameManager.I.ChangeScale(imageObj, 1.5f, 0.4f);
        GameManager.I.Open(textCG, 0.4f);
    }

    public void Hide()
    {
        amountOfClicks = 0;

        GameManager.I.ChangeScale(imageObj, 1, 0.2f);
        GameManager.I.Close(textCG, 0.2f);
    }

}
