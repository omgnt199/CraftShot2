using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "GameSettingSO",menuName = "ScriptableObject/GameSetting")]
public class GameSettingSO : ScriptableObject
{
    public float FOV;
    public float Sensitivity;
    public float AudioVolumn;
}
