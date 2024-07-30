using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProfileUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI UserNameText;
    [SerializeField] private TextMeshProUGUI LevelText;
    [SerializeField] private TextMeshProUGUI ExpBarTxt;
    [SerializeField] private Image ExpProgressImg;

    private void OnEnable()
    {
        UpdatePlayerLevelUI(PlayerGlobalInfo.Level);
        UpdateExpBarUI(CurrencyData.GetCurrencyValue(CurrencyType.Exp));
        UpdateUserNameUI(PlayerGlobalInfo.Name);

        PlayerGlobalInfo.UpdatePlayerLevel += UpdatePlayerLevelUI;
        PlayerGlobalInfo.UpdatePlayerName += UpdateUserNameUI;
        CurrencyData.ExpCurrencyChange += UpdateExpBarUI;
    }
    private void OnDisable()
    {
        PlayerGlobalInfo.UpdatePlayerLevel -= UpdatePlayerLevelUI;
        PlayerGlobalInfo.UpdatePlayerName -= UpdateUserNameUI;
        CurrencyData.ExpCurrencyChange -= UpdateExpBarUI;
    }

    void UpdateUserNameUI(string name) => UserNameText.text = name; 
    void UpdatePlayerLevelUI(int level) => LevelText.text = level.ToString();
    void SetExpBarUI(int currentExp, int expToLevelUp)
    {
        ExpBarTxt.text = currentExp.ToString() + "/" + expToLevelUp.ToString();
        ExpProgressImg.DOFillAmount((float)currentExp / (float)expToLevelUp, 0.4f);
    }
    void UpdateExpBarUI(int totalExpValue)
    {
        int currentExp = totalExpValue - PlayerGlobalInfo.TotalExpToLevelUp(PlayerGlobalInfo.Level);
        int expToLevelUp = PlayerGlobalInfo.TotalExpToLevelUp(PlayerGlobalInfo.Level + 1) - PlayerGlobalInfo.TotalExpToLevelUp(PlayerGlobalInfo.Level);
        ExpBarTxt.text = totalExpValue.ToString() + "/" + expToLevelUp.ToString();
        SetExpBarUI(currentExp, expToLevelUp);
    }
}
