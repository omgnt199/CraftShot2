using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovementInput
{
    Vector3 MovementInputVector { get; }
    event Action OnMovementEvent;
}
