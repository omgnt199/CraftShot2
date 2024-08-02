using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
public class GlobalUI : Singleton<GlobalUI>,IPopUp
{

    public List<GameObject> ListPopUp;
    private Dictionary<string, GameObject> _dictPopUpName;

    private void Start()
    {
        _dictPopUpName = new Dictionary<string, GameObject>();
        foreach (var popup in ListPopUp) _dictPopUpName.Add(popup.name, popup);
    }
    public void ShowPopUp(string popupName)
    {
        ServiceManager.ShowInter();
        var popup = Instance._dictPopUpName[popupName];
        popup.SetActive(true);
        popup.transform.SetAsLastSibling();
    }

    public void HidePopUp(string popupName)
    {
        var popup = Instance._dictPopUpName[popupName];
        popup.SetActive(false);
    }
}
public interface IPopUp
{
    public void ShowPopUp(string popupName);
    public void HidePopUp(string popupName);
}
