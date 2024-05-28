using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IDashInput
{
    public Transform LookCamera { get; }
    Vector3 Direction { get; }
    event Action OnDashEvent;
}
public class VSPlayerInput : MonoBehaviour,IMovementInput,IDashInput
{
    public event Action OnMovementEvent;
    public event Action OnDashEvent;

    public Vector3 MovementInputVector { get; private set;}

    public Vector3 Direction { get; private set; }

    public Transform LookCamera { get; set; }

    // Update is called once per frame
    void Update()
    {
        GetMovementInput();
        GetDashInput();
        GetAttackInput();
    }

    void GetMovementInput()
    {
        MovementInputVector = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
    }
    void GetDashInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Direction = LookCamera.forward;
            MovementInputVector = Direction * 2.5f;
        }
    }
    void GetAttackInput()
    {

    }

}
