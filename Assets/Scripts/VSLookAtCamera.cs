using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VSLookAtCamera : MonoBehaviour
{
    public Transform Camera;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Camera.rotation;
    }
}
