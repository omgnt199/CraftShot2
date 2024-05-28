using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockFps : MonoBehaviour
{
    private bool isLock = true;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 61;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            isLock = !isLock;
            Application.targetFrameRate = isLock ? 61 : -1;
        }    
    }

    
}
