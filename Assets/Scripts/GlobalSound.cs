using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalSound : Singleton<GlobalSound>
{
    public AudioSource ClickButton;
    public void Click() => ClickButton.Play();
}
