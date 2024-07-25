using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmoryPopUpUI : MonoBehaviour
{
    public static ArmoryPopUpUI Instance;
    [SerializeField] private List<ValueKeyPair<string, GameObject>> ListPopUp;
    private void Awake()
    {
        Instance = this;
    }
    public void ShowPopUp(string popupName)
    {
        foreach(var item in ListPopUp)
        {
            if (item.Key == popupName) item.Value.SetActive(true);
            else item.Value.SetActive(false);
        }
    }
}
