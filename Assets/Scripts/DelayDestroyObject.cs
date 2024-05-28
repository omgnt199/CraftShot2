using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayDestroyObject : MonoBehaviour
{
    public float DelayTime;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyObject", DelayTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void DestroyObject()
    {
        Destroy(gameObject);
    }
}
