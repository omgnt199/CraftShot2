using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeUI : MonoBehaviour
{
    public static GameModeUI Instance;
    public List<GameObject> ListPopUp;
    private Dictionary<string, GameObject> _dictPopUpName;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        _dictPopUpName = new Dictionary<string, GameObject>();
        foreach (var popup in ListPopUp) _dictPopUpName.Add(popup.name, popup);
    }


    // Update is called once per frame
    void Update()
    {

    }
    private void OnDisable()
    {
        foreach (var popup in _dictPopUpName) popup.Value.SetActive(false);
    }
    public void ShowHidePopUp(string popupName)
    {
        var popup = Instance._dictPopUpName[popupName];
        popup.SetActive(!popup.activeSelf);
    }
}
