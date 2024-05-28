using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VSMiniMap : MonoBehaviour
{
    public Transform Target;
    Vector3 newPostion;
    Vector3 velocity = Vector3.zero;
    private void LateUpdate()
    {
        newPostion = Target.position;
        transform.position = new Vector3(newPostion.x, transform.position.y, newPostion.z);
    }
}
