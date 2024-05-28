using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeMapNavCam : MonoBehaviour
{
    public Camera Cam;
    public Camera LargeMapCam;
    // Start is called before the first frame update
    void Start()
    {
        Cam.orthographicSize = LargeMapCam.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
