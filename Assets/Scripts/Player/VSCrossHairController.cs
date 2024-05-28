using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VSCrossHairController : MonoBehaviour
{
    public GameObject RifleCrossHair;
    public GameObject SniperCrossHair;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RifleAim()
    {
        RifleCrossHair.SetActive(false);
        SniperCrossHair.SetActive(false);
    }
    public void RifleNotAim()
    {
        RifleCrossHair.SetActive(true);
        SniperCrossHair.SetActive(false);
    }

    public void SniperScope()
    {
        SniperCrossHair.SetActive(true);
        RifleCrossHair.SetActive(false);
    }
    public void SniperNotScope()
    {
        SniperCrossHair.SetActive(false);
        RifleCrossHair.SetActive(false);
    }
}
