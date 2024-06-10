using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Counter", menuName = "ScriptableObject/CounterSO")]
public class CounterSO : ScriptableObject
{
    [SerializeField] private int _initialTime;
    [SerializeField] private int _currentTime;
    public int InitialTime => _initialTime;
    public int CurrentTime => _currentTime;

    public void SetInitialTime(int value) => _initialTime = value;
    public void SetCurrentTime(int value) => _currentTime = value;
    public void UpdateCurrenTime(int delta) => _currentTime -= delta;
}
