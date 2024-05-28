using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "GlobalMethod", menuName = "ScriptableObject/GlobalSoundMethod")]
public class GlobalSoundMethod : ScriptableObject
{
    public void ClickSoundButton() => GlobalSound.Instance.Click();
}
