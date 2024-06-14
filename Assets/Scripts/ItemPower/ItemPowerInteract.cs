using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPowerInteract : MonoBehaviour
{
    [SerializeField] private ItemPowerSO _itemPower;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _itemPower.Apply(other.gameObject);
            StartCoroutine(WaitForPowerFinish());
        }
    }
    IEnumerator WaitForPowerFinish()
    {
        transform.position += new Vector3(0, 1000f, 0);
        yield return new WaitForSeconds(_itemPower.Duration);
        _itemPower.Deactive();
        Destroy(gameObject);
    }
}
