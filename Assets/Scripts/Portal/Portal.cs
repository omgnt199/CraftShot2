using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Transform _portalLinked;
    public bool IsPortaling = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (IsPortaling) return;
            Debug.Log("Portaling");
            other.gameObject.GetComponent<VSPlayerMovement>().enabled = false;
            other.gameObject.transform.position = _portalLinked.position;
            _portalLinked.GetComponent<Portal>().IsPortaling = true;
            StartCoroutine(WaitForPortaling(other.gameObject));
        }
    }

    IEnumerator WaitForPortaling(GameObject player)
    {
        yield return new WaitForSeconds(0.5f);
        player.GetComponent<VSPlayerMovement>().enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            IsPortaling = false;
        }
    }
}
