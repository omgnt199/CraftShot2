using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float velocity;
    public float gravity;
    public float damage = 1;
    public float force;
    public float collisionSize;
    public float timer = 0;
    public enum Type
    {
        Normal, Physics
    }
    public Type type;
    public Vector3 collisionOffset;
    public LayerMask collisionLayer;
    public GameObject fx_Hit;
    public GameObject fx_Die;

    float time;

    CharacterController controller;
    Rigidbody rig;

    public Weapon dono;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        rig = GetComponent<Rigidbody>();

        if (type == Type.Physics)
            rig.AddForce(transform.forward * force * velocity * Time.deltaTime * 100);
    }

    void Update()
    {
        if(type == Type.Normal)
            controller.Move(transform.TransformDirection(0,-gravity,velocity) * Time.deltaTime);

        if (timer != 0)
            time += Time.deltaTime;

        if (timer == 0 || time > timer)
        {
            Collider[] cols = Physics.OverlapSphere(transform.position + transform.TransformDirection(collisionOffset), collisionSize, collisionLayer);
            bool hitted = false;
            for (int i = 0; i < cols.Length; i++)
            {
                Health vid = cols[i].GetComponentInParent<Health>();
                if (vid)
                    vid.Damage(damage);
                Rigidbody rig = cols[i].GetComponentInParent<Rigidbody>();
                if (rig)
                    rig.AddForce((rig.position - dono.transform.position) * force * 50);

                if(fx_Hit)
                    Destroy((Instantiate(fx_Hit, transform.position, transform.rotation) as GameObject), 10);
                
                hitted = true;
            }
            if (hitted)
            {
                if (timer == 0)
                    Destroy(gameObject);
            }
            if (timer != 0)
            {
                if(fx_Die)
                    Destroy((Instantiate(fx_Die, transform.position, transform.rotation) as GameObject), 10);
                Destroy(gameObject);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position+transform.TransformDirection(collisionOffset), collisionSize);
    }
}
