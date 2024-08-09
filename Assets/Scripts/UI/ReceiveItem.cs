using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReceiveItem : MonoBehaviour
{
    [SerializeField] private Image _itemIcon;
    [SerializeField] private TextMeshProUGUI _itemAmount;

    public void Set(Sprite icon, int amount)
    {
        _itemIcon.sprite = icon;
        _itemAmount.text = "x" + amount.ToString();
    }

}
