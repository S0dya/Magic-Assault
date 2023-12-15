using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIResultStatsItem : UIStatsItem
{
    [SerializeField] TextMeshProUGUI amountText;

    public void SetInfo(SO_Item item, int amount)
    {
        SetInfo(item);

        amountText.text = amount.ToString();
    }

    public void SetAmountText(int amount) => amountText.text = amount.ToString();
}
