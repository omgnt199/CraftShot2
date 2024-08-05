using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRotation : MonoBehaviour
{
    private Vector3 initialEulerAngles;
    // Start is called before the first frame update
    void Start()
    {
        initialEulerAngles = transform.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = initialEulerAngles;
    }
}
