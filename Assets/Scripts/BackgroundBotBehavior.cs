using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundBotBehavior : MonoBehaviour
{
    public Animator ControlAnimator;

    public Transform StartPatrolPos;
    public Transform EndPatrolPos;

    public VSGun Gun;
    public Transform GunHolder;
    public BulletPool bulletPool;
    public Transform Muzzle;
    bool isReachingEndPos = false;
    bool isAnim = false;
    Vector3 endPos;

    private VSGun _gunUsing;
    private void OnEnable()
    {
        transform.position = StartPatrolPos.position;
        endPos = EndPatrolPos.position;
        GunHolder = GetComponentInChildren<GunHolder>().gameObject.transform;
        _gunUsing = (VSGun)GlobalData.Instance.EquipmentPool.GetEquipmentByName(PlayerPrefs.GetString("VSPrimaryWeaponUsing"));
        if (GunHolder.transform.childCount == 1) Destroy(GunHolder.transform.GetChild(0).gameObject);
        Instantiate(_gunUsing.ModelForBot, GunHolder);
        ControlAnimator.runtimeAnimatorController = _gunUsing.AnimatorControllerForBot;
        Muzzle = GetComponentInChildren<VSMuzzle>().gameObject.transform;

    }
    private void OnDisable()
    {

    }
    private void Update()
    {
        if (!isReachingEndPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos, Time.deltaTime * 5f);
            transform.forward = (endPos - transform.position).normalized;
        }

        if ((transform.position - endPos).magnitude == 0 && !isAnim)
        {
            isReachingEndPos = true;
            isAnim = true;
            transform.eulerAngles = new Vector3(0, Random.Range(-45f, 45f), 0);
            ShootBehaviour();
            Commons.SetTimeout(this, Random.Range(1.5f, 2.5f), () =>
            {
                isReachingEndPos = false;
                isAnim = false;
                endPos = endPos == EndPatrolPos.position ? StartPatrolPos.position : EndPatrolPos.position;
                ControlAnimator.SetBool("IsIdle", true);
                ControlAnimator.SetBool("IsShoot", false);
                StopAllCoroutines();
            });
        }
    }

    void ShootBehaviour()
    {
        if (_gunUsing.Bullet.BulletTrail != null)
            _gunUsing.Bullet.BulletTrail.LifeTime = 0.05f;
        bulletPool.PickFromPool(_gunUsing, Muzzle.position, Muzzle.forward * _gunUsing.FirePower);
        Commons.SetTimeout(this, _gunUsing.FireSpeed, () =>
        {
            ShootBehaviour();
        });
    }
}
