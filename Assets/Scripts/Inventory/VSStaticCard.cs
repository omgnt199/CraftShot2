using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VSStaticCard : MonoBehaviour
{
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Unit;
    public Image Load;

    public void Set(string name, string unit, float rate)
    {
        Name.text = name;
        Unit.text = unit;
        Load.fillAmount = rate;
    }
}
