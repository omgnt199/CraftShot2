using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSizeBoxCollider : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        BoxCollider[] allBoxColliders = gameObject.GetComponentsInChildren<BoxCollider>();
        float maxX = 0, maxZ = 0;
        //Debug.Log(allBoxColliders[0].bounds.size);
        for (int i = 1; i < allBoxColliders.Length; i++)
        {
            if (maxX <= allBoxColliders[i].bounds.size.x) maxX = allBoxColliders[i].bounds.size.x;
            if (maxZ <= allBoxColliders[i].bounds.size.z) maxZ = allBoxColliders[i].bounds.size.z;
            Destroy(allBoxColliders[i]);
        }
        //Debug.Log(maxX + "," + maxZ);
        Vector3 sizeBound = new Vector3(maxX, 42f, maxZ);
        GetComponent<BoxCollider>().size = sizeBound;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
