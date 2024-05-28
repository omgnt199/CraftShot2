using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "GlobalMethod",menuName ="ScriptableObject/GlobalMethod")]
public class GlobalStaticMethod : ScriptableObject
{
    public void ShowPopUp(string popupName) => GlobalUI.Instance.ShowPopUp(popupName);
    public void HidePopUp(string popupName) => GlobalUI.Instance.HidePopUp(popupName);
    public void LoadScene(string scenename) => SceneManager.LoadScene(scenename);
    public void EnterGameMode(string mode) => VSGlobals.MODE = mode;
}
