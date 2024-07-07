using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool instance;
    public GameObject BulletPrefab;
    public int PoolSize;
    private List<Bullet> availableBullets;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        availableBullets = new List<Bullet>();
        for(int i = 1; i <= PoolSize; i++)
        {
            Bullet b = Instantiate(BulletPrefab, transform).GetComponent<Bullet>();
            b.gameObject.SetActive(false);
            availableBullets.Add(b);
        }
    }

    public void PickFromPool(GameObject whoShoot,VSGun gun,Vector3 position, Vector3 velocity)
    {
        //Debug.Log("PickBullet");
        if (availableBullets.Count < 1) return;
        availableBullets[0].Activate(whoShoot, gun, position, velocity);
        availableBullets.RemoveAt(0);
    }
    public void AddToPool(Bullet bullet)
    {
        if (!availableBullets.Contains(bullet)) availableBullets.Add(bullet);
    }
}
