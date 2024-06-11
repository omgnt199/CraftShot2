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
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _itemPower.Apply(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
