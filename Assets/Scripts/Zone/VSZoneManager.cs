using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VSZoneManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _zoneList;

    public void ActiveZone()
    {
        foreach (var zone in _zoneList) zone.SetActive(true);
    }
    public void DeactiveZone()
    {
        foreach (var zone in _zoneList) zone.SetActive(false);
    }
}
