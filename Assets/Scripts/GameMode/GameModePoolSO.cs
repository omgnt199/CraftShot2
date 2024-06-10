using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ModePool", menuName = "ScriptableObject/GameMode/GameModePool")]
public class GameModePoolSO : ScriptableObject
{
    public List<GameModeSO> GameModeList;

    public GameModeSO GetGameModeByName(string name) => GameModeList.Find(mode => mode.name == name);
}
