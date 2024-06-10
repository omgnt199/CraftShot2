using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IModeState
{

    void EnterMode();
    void UpdateMode();
    void EndMode();
}
