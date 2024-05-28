using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGamesMoneyUIScript : MonoBehaviour
{
    public static MiniGamesMoneyUIScript instance;
    [SerializeField] GameObject GoldText, DiamondText;
    public static int GoldAmount, DiamondAmount;
    private SaveLoad userData;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {

        SetMoneyUI(GoldAmount, DiamondAmount);
    }

    public void SetMoneyUI(int goldAmount, int diamondAmount)
    {
        GoldText.GetComponent<Text>().text = goldAmount.ToString();
        DiamondText.GetComponent<Text>().text = diamondAmount.ToString();
    }
    public void UpdateGoldUI()
    {
        GoldAmount++;
        GoldText.GetComponent<Text>().text = GoldAmount.ToString();
    }
}
