using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentSetup : MonoBehaviour
{
    private void OnEnable()
    {
        RenderSettings.fog = Random.Range(1, 10) % 2 == 0;
    }
}
