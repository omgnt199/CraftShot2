using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VSDeathCamera : MonoBehaviour
{
    public static VSDeathCamera Instance;
    public CinemachineVirtualCamera DeathCam;
    private void Awake()
    {
        Instance = this;
    }
    public void SetTarget(Vector3 pos, Transform target)
    {
        DeathCam.gameObject.transform.position = pos + new Vector3(0, 15f, 0);
        DeathCam.LookAt = target;
    }
}
