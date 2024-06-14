using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperJumpAura : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<VSPlayerMovement>().SuperJump();
        }
    }
}
