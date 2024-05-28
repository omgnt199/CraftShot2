using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class HomeUI : MonoBehaviour,IPopUp
{
    public static HomeUI Instance;
    [Header("PopUp")]
    public List<GameObject> ListPopUp;
    private Dictionary<string, GameObject> _dictPopUpName;
    private GameObject _currentPopUp;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        Init();
    }
    void Init()
    {
        _dictPopUpName = new Dictionary<string, GameObject>();
        foreach (var popup in ListPopUp) _dictPopUpName.Add(popup.name, popup);
        _currentPopUp = _dictPopUpName["MainPopUp"];
    }
    public void HidePopUp(string popupName)
    {
        var popup = Instance._dictPopUpName[popupName];
        popup.SetActive(false);
    }

    public void ShowPopUp(string popupName)
    {
        var popup = Instance._dictPopUpName[popupName];
        popup.SetActive(true);
        popup.transform.SetAsLastSibling();
        _currentPopUp.SetActive(false);
        _currentPopUp = popup;
    }

}
