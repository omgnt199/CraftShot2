using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NoticePopUpUI : MonoBehaviour
{
    public static NoticePopUpUI Instance;
    private void Awake()
    {
        Instance = this;
    }
    [SerializeField] private TextMeshProUGUI _noticeDescription;
    public void ShowDetailsOptions(string detail)
    {
        _noticeDescription.text = detail;
    }

    public void ShowDetailsOptions(string detail, Color colorText)
    {
        _noticeDescription.text = detail;
        _noticeDescription.color = colorText;
    }
}
