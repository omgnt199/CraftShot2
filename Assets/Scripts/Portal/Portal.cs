using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Transform _portalLinked;
    [SerializeField] private GameObject TeleportVfx;
    public bool IsPortaling = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (IsPortaling) return;
            Instantiate(TeleportVfx, transform.position, Quaternion.LookRotation(transform.forward));
            other.gameObject.transform.position = transform.position;
            other.gameObject.GetComponent<VSPlayerMovement>().enabled = false;
            _portalLinked.GetComponent<Portal>().IsPortaling = true;
            StartCoroutine(WaitForPortaling(other.gameObject));
        }
    }

    IEnumerator WaitForPortaling(GameObject player)
    {
        yield return new WaitForSeconds(1.5f);
        player.transform.position = _portalLinked.position;
        //Debug.Log("Teleport");
        Commons.WaitNextFrame(this, () => player.GetComponent<VSPlayerMovement>().enabled = true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            IsPortaling = false;
        }
    }
}
