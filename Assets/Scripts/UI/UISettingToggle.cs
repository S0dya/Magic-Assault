using UnityEngine;
using UnityEngine.UI;

public class UISettingToggle : MonoBehaviour
{
    [SerializeField] Image checkImage;

    public void Toggle(bool val) => checkImage.enabled = val;
}