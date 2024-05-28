using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Heath", menuName = "ScriptableObject/HeathSOConfig")]
public class HealthConfigSO : ScriptableObject
{
    [SerializeField] private int _initialHealth;

    public int InitialHealth => _initialHealth;

}
