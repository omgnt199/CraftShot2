using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="TimePlayManager",menuName = "MinigameScriptable/TimePlayManager")]
public class TimePlayManager : ScriptableObject
{
    public TimePlayMgr VoxelStrikeTimePlayMgr;

    public static float CurrentTime;


    public void Initialize()
    {

        VoxelStrikeTimePlayMgr.Initialize();

        VoxelStrikeTimePlayMgr.TimePlayModeDict.Add(VSGameMode.Domination.ToString(), 0);
        VoxelStrikeTimePlayMgr.TimePlayModeDict.Add(VSGameMode.Deathmatch.ToString(), 0);
    }
    public void Tracking()
    {
        VoxelStrikeTimePlayMgr.Tracking();
    }
}
