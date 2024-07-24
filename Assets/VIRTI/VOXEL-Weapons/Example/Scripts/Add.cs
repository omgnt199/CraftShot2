using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Add : MonoBehaviour
{
    public WpnHolder.InventoryItem.Type type;
    public int index;
    public int qtd;

    public enum OnDie
    {
        Disable, Destroy
    }
    public OnDie onDie;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Interact(WpnHolder who)
    {
        switch (type)
        {
            case WpnHolder.InventoryItem.Type.Ammo:
                who.AddAmmo(index, qtd);
                break;
        }
        switch (onDie)
        {
            case OnDie.Disable:
                gameObject.SetActive(false);
                break;
            case OnDie.Destroy:
                Destroy(gameObject);
                break;
        }
    }
}
