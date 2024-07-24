using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float startHealth;
    public float maxHealth;

    public GameObject instantiateOnDie;
    public float timeToDestroyInsta = 2;
    public bool destroyOnDie;
    public GameObject instantiateOnHit;
    public float timeToDestroyInstaHit = 2;

    float health;

    void Start()
    {
        health = startHealth;
    }

    void Update()
    {
        
    }

    public void Damage(float amount)
    {
        health -= amount;
        if(health <= 0)
            Die();
        else
        {
            if (instantiateOnHit)
            {
                GameObject obj = Instantiate(instantiateOnHit, transform.position, transform.rotation);
                Destroy(obj, timeToDestroyInstaHit);
            }
        }
    }

    void Die()
    {
        if (instantiateOnDie)
        {
            GameObject obj = Instantiate(instantiateOnDie, transform.position, transform.rotation);
            Destroy(obj, timeToDestroyInsta);
        }
        if (destroyOnDie)
            Destroy(gameObject);
    }
}
