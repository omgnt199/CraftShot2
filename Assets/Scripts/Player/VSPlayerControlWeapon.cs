using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;


public enum VSAttackState
{
    Shoot,
    Knife,
    ThrowNade
}

public class VSPlayerControlWeapon : MonoBehaviour
{
    public Transform PlayerTransform;
    [Header("Script")]
    public VSPlayerController PlayerController;
    public VSPlayerControlAnimator controlAnimator;
    [SerializeField] private VSPlayerMovement _playerMovement;
    [SerializeField] private VSPlayerInfo _playerInfo;
    [SerializeField] private MouseLookAround _mls;
    [SerializeField] private VSTouchLookAround _tla;
    [SerializeField] private PlayerSoundManager _playerSoundManager;
    [Header("Setting Fire")]
    public VSGun GunUsing;
    [SerializeField] private Transform _fpCamera;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _firePower;
    [SerializeField] private float _fireSpeed;
    [SerializeField] private float _fireTimer;
    [SerializeField] private float _timeReload;
    [SerializeField] private LayerMask _fireMask;
    [SerializeField] private LayerMask _aimMask;
    [SerializeField] private GameObject BulletDecalVfx;
    [SerializeField] private GameObject BloodVfx;
    private bool _isShooting;
    private bool _isReloading = false;
    private bool _isAutoAim = false;
    public bool _isAttackPressed = false;
    public Image CrossHairUI;
    [Header("Setting Aim/Scope")]
    public VSCrossHairController Crosshair;
    public VSCrossHair CrossHairUsing;
    private bool isAim = false;
    private bool isScope = false;
    [Header("Ammo")]
    [SerializeField] private int _Magazine = 30;
    [SerializeField] private int _totalAmmo = 90;
    [SerializeField] private int _currentMagazine;
    [SerializeField] private int _currentAmmo;
    [SerializeField] private int _nadeAmount = 3;
    public VSWeaponAmmoInfoIngame VSWpAmmoIngameInfo;
    [Header("Recoil")]
    public VSWeaponRecoil Recoil_Script;
    [SerializeField] private float _recoilAmountY;
    [SerializeField] private float _recoilAmountX;
    private float timePressed = 0f;
    [Header("Nade")]
    public VSNade Nadeusing;
    public Transform ThrowNadePos;
    public float ThrowUpForce;
    public float ThrowForwardForce;
    public LineRenderer ProjectileTrajectoryNade;
    public float LinePoints;
    public float TimeBetweenPoint;
    private float _currentThrowUpForce = 0;
    private float _currentThrowForwardForce = 0;
    [Header("Support")]
    public VSSupportWeapon KnifeUsing;
    private float _knifeAttackDelay = 0.5f;
    private float _knifeAttackTimer = 0;

    //Constt
    private const float DEFEAULT_FOV = 60f;
    private const float AIM_FOV = 45f;
    private const float SCOPE_FOV = 30f;

    //AttackState
    private VSAttackState _attackState;

    private bool _isAutoAimMode = true;
    public GameObject Vfx_shoot;
    //Getter/Setter
    public int Magazine { get => _Magazine; }
    public int TotalAmmo { get => _totalAmmo; }
    public int CurrentMagazine { get => _currentMagazine; }
    public int CurrentAmmo { get => _currentAmmo; }
    public int NadeAmount { get => _nadeAmount; set => _nadeAmount = value; }
    public float RecoilAmountX { get => _recoilAmountX; }
    public float RecoilAmountY { get => _recoilAmountY; }

    public Transform FirePoint { get => _firePoint; set => _firePoint = value; }
    // Start is called before the first frame update
    void Start()
    {
        _fireTimer = 0;
    }
    private void OnDisable()
    {

    }

    // Update is called once per frame
    void Update()
    {

#if UNITY_EDITOR
        MyInput();
#endif
        if (Physics.Raycast(_fpCamera.position, _fpCamera.transform.forward, out RaycastHit hit, Mathf.Infinity, _aimMask))
        {
            if (LayerMask.LayerToName(hit.collider.gameObject.layer) == "BodyPart")
            {
                CrossHairUI.color = Color.red;
            }
            else CrossHairUI.color = Color.white;
        }
        else CrossHairUI.color = Color.white;
        //Auto aim with rifle gun
        if (_attackState == VSAttackState.Shoot && GunUsing.CrossHair == VSCrossHair.Rifle && _isAutoAimMode) HandleAutoAim();
        //AttackTimer which count time(s) delay between 2 consecutive attacks
        AttackTimer();

        if (_isAttackPressed) Attack();
    }
    void AttackTimer()
    {
        //Gun Shoot Timer
        if (_attackState == VSAttackState.Shoot) _fireTimer = Mathf.Max(0f, _fireTimer - Time.deltaTime);

        //Knife Attack Timer
        if (_attackState == VSAttackState.Knife) _knifeAttackTimer = Mathf.Max(0, _knifeAttackTimer - Time.deltaTime);

    }
    void MyInput()
    {
        //Reload input
        if (Input.GetKeyDown(KeyCode.R)) Reload();
        //Attack input
        if (Input.GetMouseButton(0))
        {
            _isAttackPressed = true;
        }
        else if (Input.GetMouseButtonUp(0)) StopAttack();
        //Aim input
        if (Input.GetMouseButtonUp(1))
        {
            HandleAim();
        }

        if (Input.GetKey(KeyCode.Alpha4)) DrawNadeProjectileTrajectory();

        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            ThrowGrenade();
        }
        if (Input.GetKey(KeyCode.Z)) _isAutoAimMode = !_isAutoAimMode;

        if (Input.GetKeyDown(KeyCode.T))
        {
            Physics.Raycast(_fpCamera.position, _fpCamera.transform.forward, out RaycastHit hit, Mathf.Infinity, _aimMask);
            Debug.Log(hit.point);
            GetComponent<FireCurveProjectile>().Fire(hit.point);
        }
    }

    public void SetAttackState(VSAttackState state) => _attackState = state;

    public void SetAttackPress(bool value) => _isAttackPressed = value;
    public void Attack()
    {

        switch (_attackState)
        {
            case VSAttackState.Shoot:
                _isShooting = true;
                if (_fireTimer == 0)
                {
                    _fireTimer = _fireSpeed;
                    Shoot();
                }
                break;
            case VSAttackState.Knife:
                if (_knifeAttackTimer == 0)
                {
                    _knifeAttackTimer = _knifeAttackDelay;
                    KnifeAttack();
                }
                break;
        }

    }

    public void StopAttack()
    {
        _isAttackPressed = false;
        if (_attackState == VSAttackState.Shoot)
        {
            _isShooting = false;
            if (_fireSpeed == 0)
            {
                _fireTimer = _fireSpeed;
                Shoot();
            }
            if (!controlAnimator.WeaponAnimator.GetBool("IsAim")) controlAnimator.WeaponAnimator.SetBool("IsIdle", true);
            controlAnimator.WeaponAnimator.SetBool("IsShoot", false);
            controlAnimator.WeaponAnimator.SetFloat("Spray", 0f);
        }
    }
    public void Shoot()
    {
        //Enough ammo
        if (_currentMagazine > 0 && !_isReloading)
        {
            //Anim
            controlAnimator.Shoot();
            //Update Ammo
            _currentMagazine--;
            VSWpAmmoIngameInfo.CurrentMagazine = _currentMagazine;
            VSInGameUIScript.instance.UpdateAmmoUI(_currentMagazine, _currentAmmo);
            //Recoil
            Recoil_Script.Recoil();
            //Shoot vfx sfx
            Vfx_shoot.GetComponent<VSShootVfx>().Spawn();
            _playerSoundManager.EnableBulletSound(GunUsing.Bullet.BulletSound);
            ////Check Raycast hit
            LayerMask mask = LayerMask.GetMask("BodyPart", "Barrier", "Ground", "ObstacleLayer");
            Vector3 startPosition = _fpCamera.transform.position;
            Physics.Raycast(startPosition + _fpCamera.transform.forward * 2f, _fpCamera.transform.forward, out RaycastHit hit, Mathf.Infinity, mask);
            //Cheat Bullet's velocity = Vector between RaycastHit's point and FirePoint
            Vector3 bulletVelocity;
            if (hit.point != Vector3.zero) bulletVelocity = (hit.point - _firePoint.position).normalized * _firePower;
            else bulletVelocity = _firePoint.forward * _firePower;
            BulletPool.instance.PickFromPool(gameObject, GunUsing, _firePoint.position, bulletVelocity);
            //Is Sniper?
            if (CrossHairUsing == VSCrossHair.Sniper)
            {
                if (isScope) HandleAim();
            }
        }
        //Out of ammo
        if (_currentMagazine == 0 && _currentAmmo > 0)
        {
            controlAnimator.WeaponAnimator.SetBool("IsShoot", false);
            controlAnimator.WeaponAnimator.SetFloat("Spray", 0f);
            Reload();
        }
    }

    private void HitPlayer(RaycastHit hit)
    {
        VSPlayerInfo victim = hit.collider.gameObject.GetComponentInParent<VSPlayerInfo>();
        if (GetComponent<VSPlayerInfo>().Team != victim.Team)
        {
            //Blood Vfx
            Instantiate(BloodVfx, hit.point, Quaternion.identity);
            //Calculate damage
            int dam = 0;
            if (hit.collider.CompareTag(VSBodyPart.Body.ToString())) dam = GunUsing.DamageToBody;
            else if (hit.collider.CompareTag(VSBodyPart.Leg.ToString()) || hit.collider.CompareTag(VSBodyPart.Hand.ToString())) dam = GunUsing.DamageToHandLeg;
            else dam = GunUsing.DamageToHead;
            victim.UpdateHP(-dam);
            if (victim.HP <= 0)
            {
                //Update player's kills
                _playerInfo.Kills++;
                //Show kill report
                KillType killType = KillType.None;
                if (hit.collider.CompareTag(VSBodyPart.Head.ToString())) killType = KillType.HeadShot;
                VSInGameUIScript.instance.ShowKillReport(_playerInfo.Name, _playerInfo.Team.TeamSide, victim.Name, victim.Team.TeamSide, GunUsing.GunKillIcon, killType);
                victim.OnDeath();
                //'Kill' Event trigger
                Kill(killType);
            }
            //Show damage UI
            VSInGameUIScript.instance.ShowDamgeScore(dam);
        }
    }
    void Kill(KillType type)
    {
        if (type != KillType.None) PlayerEventListener.RaiseSpecialKillEvent(type);
        PlayerEventListener.RaiseKillByEvent();
    }
    public void KnifeAttack()
    {
        controlAnimator.Knife();
        //Check Raycast hit
        LayerMask mask = LayerMask.GetMask("Player", "Enemy");
        Vector3 startPosition = _fpCamera.transform.position;
        if (Physics.Raycast(startPosition, _fpCamera.transform.forward, out RaycastHit hit, 1f, mask))
        {
            if (hit.collider.gameObject.Equals(gameObject)) return;
            VSPlayerInfo victim = hit.collider.gameObject.GetComponent<VSPlayerInfo>();
            if (victim.Team != _playerInfo.Team)
            {
                //Blood Vfx
                Instantiate(BloodVfx, hit.point, Quaternion.identity);
                //Calculate damage
                int dam = 0;
                Vector3 hitPoint = hit.point;
                //Debug.Log(Vector3.Angle(victim.transform.forward, transform.forward));
                if (Vector3.Angle(victim.transform.forward, transform.forward) >= 75f && Vector3.Angle(victim.transform.forward, transform.forward) <= 180f) dam = KnifeUsing.DamageFace;
                else if (Vector3.Angle(victim.transform.forward, transform.forward) < 75f) dam = KnifeUsing.DamageBack;
                //
                victim.UpdateHP(-dam);
                VSInGameUIScript.instance.ShowDamgeScore(dam);
                if (victim.HP <= 0)
                {
                    _playerInfo.Kills++;
                    victim.OnDeath();
                    VSInGameUIScript.instance.ShowKillReport(_playerInfo.Name, _playerInfo.Team.TeamSide, victim.Name, victim.Team.TeamSide, KnifeUsing.KnifeKillIcon, KillType.None);
                }

            }
        }

    }
    public void ThrowGrenade()
    {
        ProjectileTrajectoryNade.enabled = false;
        if (_nadeAmount > 0)
        {
            GameObject nade = Instantiate(Nadeusing.Model, ThrowNadePos.position, Quaternion.identity);
            nade.GetComponent<VSNadeHandle>().WhoThrow = _playerInfo;
            nade.GetComponent<Rigidbody>().AddForce(_fpCamera.transform.up * _currentThrowUpForce + _fpCamera.transform.forward * _currentThrowForwardForce, ForceMode.Impulse);
            _nadeAmount--;
            VSInGameUIScript.instance.UpdateNadeUI(_nadeAmount);
        }
        _currentThrowUpForce = _currentThrowForwardForce = 0;
    }
    public void HandleAim()
    {
        switch (CrossHairUsing)
        {
            case VSCrossHair.Rifle:
                HandleRifleAim();
                break;
            case VSCrossHair.Sniper:
                HandleSniperAim();
                break;
        }

    }
    void OnRifleAim()
    {
        isAim = true;
        controlAnimator.OnAim();
        Crosshair.RifleAim();
        _playerMovement.SpeedOnAiming();
        _fpCamera.GetComponent<Camera>().fieldOfView = AIM_FOV;
        _mls.sensitivity = 0.3f;
        _tla.cameraSensitivity = 7f;
        Recoil_Script.OnAim();
    }
    void OffRifleAim()
    {
        isAim = false;
        controlAnimator.OffAim();
        Crosshair.RifleNotAim();
        _playerMovement.SpeedOnWalking();
        _fpCamera.GetComponent<Camera>().fieldOfView = DEFEAULT_FOV;
        _mls.sensitivity = 1f;
        _tla.cameraSensitivity = 14f;
        Recoil_Script.OffAim();
    }
    void HandleRifleAim()
    {
        if (!_isReloading)
        {
            if (!isAim) OnRifleAim();
            else OffRifleAim();
        }
    }
    void OnScope()
    {
        isScope = true;
        //controlAnimator.Aim();
        Crosshair.SniperScope();
        _playerMovement.SpeedOnWalking();
        _fpCamera.GetComponent<Camera>().fieldOfView = SCOPE_FOV;
        _mls.sensitivity = 0.3f;
        _tla.cameraSensitivity = 3f;
    }
    void OffScope()
    {
        isScope = false;
        //controlAnimator.Aim();
        Crosshair.SniperNotScope();
        _playerMovement.SpeedOnWalking();
        _fpCamera.GetComponent<Camera>().fieldOfView = DEFEAULT_FOV;
        _mls.sensitivity = 1f;
        _tla.cameraSensitivity = 10f;
    }
    void HandleSniperAim()
    {
        if (!_isReloading)
        {
            if (!isScope) OnScope();
            else OffScope();
        }
    }
    public void Reload()
    {
        if (!_isReloading)
        {
            if (_currentAmmo > 0 && _currentMagazine < _Magazine) StartCoroutine(ReloadDelay());
            else if (_currentAmmo == 0) VSInGameUIScript.instance.ShowOutOfAmmoPopUp();
        }
    }
    IEnumerator ReloadDelay()
    {
        VSInGameUIScript.instance.ShowReloadingPopUp();
        _isReloading = true;
        yield return new WaitForSeconds(_timeReload);
        //Calculate Ammo
        if (_currentAmmo + _currentMagazine >= _Magazine && _currentMagazine < _Magazine)
        {
            _currentAmmo = _currentAmmo + _currentMagazine - _Magazine;
            _currentMagazine = _Magazine;

        }
        else if (_currentAmmo + _currentMagazine < _Magazine)
        {
            _currentMagazine += _currentAmmo;
            _currentAmmo = 0;
        }
        VSWpAmmoIngameInfo.CurrentAmmo = _currentAmmo;
        VSWpAmmoIngameInfo.CurrentMagazine = _currentMagazine;
        VSInGameUIScript.instance.HideRelaodingPopUp();
        VSInGameUIScript.instance.UpdateAmmoUI(_currentMagazine, _currentAmmo);
        _isReloading = false;
    }

    public void CancleReload()
    {
        StopCoroutine(ReloadDelay());
        _isReloading = false;
        VSInGameUIScript.instance.HideRelaodingPopUp();
    }

    public void OnSwitchWeapon()
    {
        if (GunUsing.CrossHair == VSCrossHair.Sniper) Crosshair.SniperNotScope();
        controlAnimator.RebindAnim();
        _fpCamera.GetComponent<Camera>().fieldOfView = DEFEAULT_FOV;
        _playerMovement.SpeedOnWalking();
        _isAutoAim = false;
        _isShooting = false;
        _fireTimer = 0;
        Recoil_Script.ReturnSpeed = 3f;
        controlAnimator.WeaponAnimator.SetBool("IsShoot", false);
        controlAnimator.WeaponAnimator.SetFloat("Spray", 0f);

    }

    public void EquipGun(VSGun gun, VSWeaponAmmoInfoIngame gunAmmoIngame)
    {
        GunUsing = gun;
        _firePower = gun.FirePower;
        _fireSpeed = gun.FireSpeed;
        _Magazine = gun.Magazine;
        _totalAmmo = gun.TotalAmmo;
        _timeReload = gun.TimeReload;
        _recoilAmountX = gun.RecoilAmountX;
        _recoilAmountY = gun.RecoilAmountY;
        CrossHairUsing = gun.CrossHair;
        VSWpAmmoIngameInfo = gunAmmoIngame;
        _currentMagazine = gunAmmoIngame.CurrentMagazine;
        _currentAmmo = gunAmmoIngame.CurrentAmmo;

        Recoil_Script.SetRecoil(_recoilAmountX, _recoilAmountY);
        VSInGameUIScript.instance.UpdateNadeUI(_nadeAmount);
    }
    void OnAutoAim()
    {
        _isAutoAim = true;
        _isShooting = true;
        Recoil_Script.OnAim();
        if (_fireTimer == 0)
        {
            _fireTimer = _fireSpeed;
            Shoot();
        }
    }
    void OffAutoAim()
    {
        CrossHairUI.color = Color.white;
        if (!_isAttackPressed && _isAutoAim)
        {
            _isAutoAim = false;
            _isShooting = false;
            Recoil_Script.OnAim();
            controlAnimator.WeaponAnimator.SetBool("IsShoot", false);
            controlAnimator.WeaponAnimator.SetFloat("Spray", 0f);
        }
    }
    void HandleAutoAim()
    {
        if (Physics.Raycast(_fpCamera.position + _fpCamera.transform.forward * 2f, _fpCamera.transform.forward, out RaycastHit hit, Mathf.Infinity, _aimMask))
        {
            if (LayerMask.LayerToName(hit.collider.gameObject.layer) == "BodyPart")
            {
                CrossHairUI.color = Color.red;
                GameObject hitCharacter = hit.collider.gameObject;
                if (_playerInfo.Team != hitCharacter.GetComponentInParent<VSPlayerInfo>().Team)
                {
                    if (!_isAttackPressed) OnAutoAim();
                    else Recoil_Script.OffAim();
                }
            }
            else OffAutoAim();
        }
        else OffAutoAim();
    }

    public void DrawNadeProjectileTrajectory()
    {
        _currentThrowUpForce = Mathf.Lerp(_currentThrowUpForce, ThrowUpForce, Time.deltaTime * 2f);
        _currentThrowForwardForce = Mathf.Lerp(_currentThrowForwardForce, ThrowForwardForce, Time.deltaTime * 2f);

        ProjectileTrajectoryNade.enabled = true;
        ProjectileTrajectoryNade.positionCount = Mathf.CeilToInt(LinePoints / TimeBetweenPoint) + 1;
        Vector3 startPosition = ThrowNadePos.position;
        Vector3 startVelocity = (_fpCamera.forward * _currentThrowForwardForce + _fpCamera.up * _currentThrowUpForce) / Nadeusing.Model.GetComponent<Rigidbody>().mass;
        int i = 0;
        ProjectileTrajectoryNade.SetPosition(i, startPosition);
        for (float time = 0; time < LinePoints; time += TimeBetweenPoint)
        {
            i++;
            Vector3 point = startPosition + time * startVelocity;
            //Recipe: d = v*t + 1/2 * t * t;
            point.y = startPosition.y + startVelocity.y * time + (Physics.gravity.y / 2f * time * time);
            ProjectileTrajectoryNade.SetPosition(i, point);

            LayerMask mask = LayerMask.GetMask("Ground", "Barrier", "ObstacleLayer", "Floor");
            Vector3 lastPosition = ProjectileTrajectoryNade.GetPosition(i - 1);
            if (Physics.Raycast(lastPosition, (point - lastPosition).normalized, out RaycastHit hit, (point - lastPosition).magnitude, mask))
            {
                ProjectileTrajectoryNade.SetPosition(i, hit.point);
                ProjectileTrajectoryNade.positionCount = i + 1;
                break;
            }

        }
    }
}
