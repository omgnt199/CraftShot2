using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MouseLookAround : MonoBehaviour
{
    public Transform MyCamera;
    public Transform Weapon;
    public float sensitivity;
    float xRotation = 0f;
    public float yRotationSpeed, xCameraSpeed;
    [HideInInspector] public float zRotation;
    private float rotationYVelocity, cameraXVelocity;
    [HideInInspector] public float wantedCameraXRotation;
    [HideInInspector] public float currentCameraXRotation;
    [HideInInspector] public float wantedYRotation;
    [HideInInspector] public float currentYRotation;

    public float topAngleView;
    public float bottomAngleView;
    void Start()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        Cursor.lockState = CursorLockMode.Locked;

#endif

    }
    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        MouseControl();
#endif
    }
    private void OnDestroy()
    {
        Cursor.lockState = CursorLockMode.None;
    }
    void MouseControl()
    {
        wantedYRotation += Input.GetAxis("Mouse X") * sensitivity;
        wantedCameraXRotation -= Input.GetAxis("Mouse Y") * sensitivity;
        wantedCameraXRotation = Mathf.Clamp(wantedCameraXRotation, bottomAngleView, topAngleView);
        currentYRotation = Mathf.SmoothDamp(currentYRotation, wantedYRotation, ref rotationYVelocity, yRotationSpeed);
        currentCameraXRotation = Mathf.SmoothDamp(currentCameraXRotation, wantedCameraXRotation, ref cameraXVelocity, xCameraSpeed);

        transform.rotation = Quaternion.Euler(0, currentYRotation, 0);
        MyCamera.localRotation = Quaternion.Euler(currentCameraXRotation, 0, zRotation);

    }
}
