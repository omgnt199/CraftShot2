using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VSTouchLookAround : MonoBehaviour
{
    // References
    [SerializeField] private Transform cameraTransform;

    // Player settings
    public float cameraSensitivity;
    [SerializeField] private float moveInputDeadZone;

    // Touch detection
    private int rightFingerId;
    private float halfScreenWidth;

    // Camera control
    public Vector2 lookInput;
    public float cameraPitch;

    // Player movement
    private Vector2 moveTouchStartPosition;
    private Vector2 moveInput;


    // Start is called before the first frame update
    void Start()
    {
        // id = -1 means the finger is not being tracked
        rightFingerId = -1;

        // only calculate once
        halfScreenWidth = Screen.width / 2;

        // calculate the movement input dead zone
        moveInputDeadZone = Mathf.Pow(Screen.height / moveInputDeadZone, 2);
    }

    // Update is called once per frame
    void Update()
    {
        // Handles input
        GetTouchInput();


        if (rightFingerId != -1)
        {
            // Ony look around if the right finger is being tracked
            Debug.Log("Rotating");
            LookAround();
        }

    }

    void GetTouchInput()
    {
        // Iterate through all the detected touches
        for (int i = 0; i < Input.touchCount; i++)
        {

            Touch t = Input.GetTouch(i);

            // Check each touch's phase
            switch (t.phase)
            {
                case TouchPhase.Began:

                    if (t.position.x > halfScreenWidth && rightFingerId == -1)
                    {
                        // Start tracking the rightfinger if it was not previously being tracked
                        rightFingerId = t.fingerId;
                    }

                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:

                    if (t.fingerId == rightFingerId)
                    {
                        // Stop tracking the right finger
                        rightFingerId = -1;
                        Debug.Log("Stopped tracking right finger");
                    }

                    break;
                case TouchPhase.Moved:

                    // Get input for looking around
                    if (t.fingerId == rightFingerId)
                    {
                        lookInput = t.deltaPosition * cameraSensitivity * Time.deltaTime;
                    }
                    break;
                case TouchPhase.Stationary:
                    // Set the look input to zero if the finger is still
                    if (t.fingerId == rightFingerId)
                    {
                        lookInput = Vector2.zero;
                    }
                    break;
            }
        }
    }

    void LookAround()
    {

        // vertical (pitch) rotation
        cameraPitch = Mathf.Clamp(cameraPitch - lookInput.y, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(cameraPitch, 0, 0);

        // horizontal (yaw) rotation
        transform.Rotate(transform.up, lookInput.x);
    }

    private void OnDisable()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        lookInput = Vector2.zero;
        cameraPitch = 0f;
        cameraTransform.localRotation = Quaternion.Euler(0, 0, 0);
    }
}
