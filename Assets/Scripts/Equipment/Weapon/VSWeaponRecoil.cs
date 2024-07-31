using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VSWeaponRecoil : MonoBehaviour
{
    //Rotations
    private Vector3 _currentRotation;
    private Vector3 _targetRotation;

    //Hipfire recoil
    [SerializeField] private float _recoilX;
    [SerializeField] private float _recoilY;

    //Settings
    [SerializeField] private float _Snappiness;
    [SerializeField] private float _returnSpeed;

    //Getter/Setter
    public float Snappiness { get => _Snappiness; set { _Snappiness = value; } }
    public float ReturnSpeed { get => _returnSpeed; set { _returnSpeed = value; } }

    // Update is called once per frame
    void Update()
    {
        _targetRotation = Vector3.Lerp(_targetRotation, Vector3.zero, _returnSpeed * Time.deltaTime);
        _currentRotation = Vector3.Slerp(_currentRotation, _targetRotation, _Snappiness * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(_currentRotation);
    }
    public void Recoil()
    {
        _targetRotation += new Vector3(-_recoilX, Random.Range(-_recoilY, _recoilY), 0);
    }

    public void SetRecoil(float recoilX, float recoilY)
    {
        this._recoilX = recoilX;
        this._recoilY = recoilY;
    }
    public void OnAim()
    {
        ReturnSpeed = 2f;
    }
    public void OffAim()
    {
        ReturnSpeed = 1f;
    }
}
