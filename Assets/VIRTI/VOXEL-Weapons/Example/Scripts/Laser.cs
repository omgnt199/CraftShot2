using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    LineRenderer lineRenderer;
    RaycastHit hit;
    public Transform point;

    void Start () {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update () {
        lineRenderer.SetPosition(0, point.position);

        if(Physics.Raycast(transform.position, transform.forward, out hit)) {
            if (hit.collider)
                lineRenderer.SetPosition(1, hit.point);
        }
        else lineRenderer.SetPosition(1, transform.forward*5000);
	}
}