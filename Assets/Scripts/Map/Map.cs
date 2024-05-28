using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Map : MonoBehaviour
{
    public List<VSGameMode> ModesCanPlay;
    public List<Transform> ZonesPositionDominationMode;
    public Transform FirstTeamSpawn;
    public Transform SecondTeamSpawn;
    public Transform DeathmatchSpawn;
    public Vector3 LargeMapCameraPosition;
    public float LargeMapCameraSize;
    public Sprite LargeMapSprite;
    public void ActiveZone() => ZonesPositionDominationMode.ForEach(zone => zone.gameObject.SetActive(true));
    public void DeactiveZone() => ZonesPositionDominationMode.ForEach(zone => zone.gameObject.SetActive(false));
}
