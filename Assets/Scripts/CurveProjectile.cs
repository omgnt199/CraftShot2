using Cinemachine.Utility;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CurveProjectile : MonoBehaviour
{
    public Transform Projectile;
    public Vector3 targetPosition;
    public bool IsReach = false;
    [SerializeField] private float _Speed;
    [SerializeField] private float _Radius;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _lifeTime;

    private void Start()
    {
        Invoke("SelfDestroy", _lifeTime);
    }
    // Update is called once per frame
    void Update()
    {
        //transform.position = Vector3.Lerp(transform.position,targetPosition,Time.deltaTime * _Speed);
        transform.Rotate(new Vector3(0, 0, _rotateSpeed) * Time.deltaTime);
    }
    void SelfDestroy() => Destroy(gameObject);
}
