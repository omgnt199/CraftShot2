using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPS : MonoBehaviour
{
    public Transform Target;

    private void Update()
    {
        transform.position = new Vector3(Target.position.x, transform.position.y, Target.position.z);
    }
}
