using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VSZoneUIOverlay : MonoBehaviour
{
    public Transform Player;
    public GameObject ZoneUI;
    public float Distance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Player);
        if ((transform.position - Player.position).magnitude >= Distance) ZoneUI.SetActive(true);
        else ZoneUI.SetActive(false);
    }
}
