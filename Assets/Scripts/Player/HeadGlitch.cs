using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadGlitch : MonoBehaviour
{

    public bool IsGlitch = false;
    private void OnDisable()
    {
        IsGlitch = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(" Weapon glitch " + other.gameObject.name);
        IsGlitch = true;
    }
    private void OnTriggerStay(Collider other)
    {
        IsGlitch = true;
    }
    private void OnTriggerExit(Collider other)
    {
        IsGlitch = false;
    }
}
