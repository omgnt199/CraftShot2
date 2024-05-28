
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameController : MonoBehaviour
{

    private void OnEnable()
    {
        GlobalData.Instance.TimePlayManager.Tracking();
    }

}
