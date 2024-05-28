using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VSDeathCamera : Singleton<VSDeathCamera>
{
    public CinemachineVirtualCamera VirtualCamera;
    public void SetTarget(Transform target)
    {
        VirtualCamera.Follow = target;
        VirtualCamera.LookAt = target;
    }
}
