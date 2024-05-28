using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameTag : MonoBehaviour
{
    private Vector3 _eulerAnglesOrigin;
    // Start is called before the first frame update
    void Start()
    {
        _eulerAnglesOrigin = transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = _eulerAnglesOrigin;
    }
}
