using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class MapPool : MonoBehaviour
{
    public List<Map> ListMap;

    public Map PickRandomMap(string mode)
    {
        VSGameMode gamemode = 0;
        if (mode == "Domination") gamemode = VSGameMode.Domination;
        else if (mode == "Deathmatch") gamemode = VSGameMode.Deathmatch;
        else if (mode == "Adventure") gamemode = VSGameMode.Adventure;
        List<Map> mapsCanPick = new List<Map>(from map in ListMap where map.ModesCanPlay.Contains(gamemode) select map);
        return mapsCanPick[new System.Random().Next(mapsCanPick.Count)];
    }
}
