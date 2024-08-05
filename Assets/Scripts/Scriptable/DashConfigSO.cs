using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "DashConfig",menuName ="ScriptableObject/DashConfigSO")]
public class DashConfigSO : ScriptableObject
{
    public float DashTime;
    public int DashStack;
    public float DashStackCooldown;
}
