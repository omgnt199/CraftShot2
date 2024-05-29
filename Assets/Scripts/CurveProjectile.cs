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
    private float _Radian = 0f;

    private void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        _Radian += Time.deltaTime * _Speed;
        if(_Radian >= 360f) _Radian = 0f;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * _Speed);
        Projectile.transform.position = new Vector3(_Radius * Mathf.Cos(_Radian) + transform.position.x, _Radius * Mathf.Sin(_Radian) + transform.position.y, transform.position.z);
        //transform.position = new Vector3(_Radius * Mathf.Sin(_phiRadian) * Mathf.Cos(_thetaRadian), _Radius * Mathf.Sin(_phiRadian) * Mathf.Sin(_thetaRadian), _Radius * Mathf.Cos(_phiRadian));
        if (Vector3.Distance(transform.position, targetPosition) <= 0.2f)
        {
            IsReach = false;
            Destroy(gameObject);
        }
    }
}
