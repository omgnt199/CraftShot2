using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameModeSO : ScriptableObject
{
    public string ModeName;
    public abstract void EnterMode();
    public abstract void UpdateMode();
    public abstract void EndMode();
    public abstract void Revive(GameObject player);
}
