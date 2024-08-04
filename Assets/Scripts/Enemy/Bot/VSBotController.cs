
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using Unity.Burst.Intrinsics;
using Assets.Scripts.Common;

public class VSBotController : MonoBehaviour
{
    public NavMeshAgent agent;
    private Vector3 walkPoint;
    private Vector3 distanceToWalkPoint;
    private bool isReachingWalkPoint = true;
    [Header("Scripts")]
    public VSPlayerInfo PlayerInfo;
    public VSWeaponRecoil WeaponRecoil;
    public PlayerSoundManager SoundManager;
    [Header("AI")]
    public float TimeReact;
    private float _timerReact;
    [Header("Gun")]
    public VSGun GunUsing;
    [Header("Attack")]
    public LayerMask MaskAttack;
    public float attackRange;
    public float sightDistance;
    private float fireSpeed = 0.4f;
    private float fireTimer = 0f;
    private float timeCheckAttack = 3f;
    private bool isAlreadyAttack = false;
    public GameObject BulletDecalPrefab;

    [Header("Setting Fire")]
    public Transform fpCamera;
    public Transform firePoint;
    public float firePower;
    public VSShootVfx shootVfx;
    private BulletPool bulletPool;

    [Header("Nade")]
    public GameObject GrenadePrefab;
    public Transform ThrowNadePos;
    private int _nadeAmount = 3;

    [Header("Zone")]
    public List<Transform> ZonePoints;
    private Transform zoneStaying;
    private float zoneRadius = 4f;

    [Header("Script")]
    public VSPlayerInfo BotInfo;
    public BotControlAnimator ControlAnimator;
    [Header("Costume")]
    public Transform SkinModelLocate;
    //public CostumeController costume;

    Collider[] hitcolliders;
    float minDistance = 1000f;
    public GameObject closestObj = null;
    Vector3 attackPosition;

    bool _isReadyShot = false;

    private void Awake()
    {
        GunUsing = (VSGun)GlobalData.Instance.EquipmentPool.GetRandomEquipmentByType(VSEquipmentType.PrimaryWeapon);
        //fireSpeed = 0.4f;
        bulletPool = BulletPool.instance;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 10f;

    }
    private void OnEnable()
    {
        SearchWalkPoint();
    }
    // Start is called before the first frame update
    void Start()
    {
        _timerReact = TimeReact;
        //costume.LoadMixCostumeSet(GlobalData.Instance.costumePartDefine, Random.Range(5, System.Enum.GetValues(typeof(CostumeType)).Length) - 1);
        SearchWalkPoint();
    }

    // Update is called once per frame
    void Update()
    {
        //Reach walkpoint?
        isReachingWalkPoint = IsReachingWalkPoint();
        if (isReachingWalkPoint)
        {
            if (!isAlreadyAttack)
                SearchWalkPoint();
        }
        //attacking?
        if (isAlreadyAttack)
        {
            timeCheckAttack -= Time.deltaTime;
            if (timeCheckAttack <= 0)
            {
                isAlreadyAttack = false;
                timeCheckAttack = 1f;
                closestObj = null;
            }
            else
            {
                fireTimer += Time.deltaTime;
                if (fireTimer >= fireSpeed)
                {
                    fireTimer = 0f;
                    Shoot();
                }
            }
        }
        else
        {
            if (_timerReact <= 0)
            {
                _timerReact = TimeReact;
                // Check Enemy In Attack Range
                hitcolliders = Physics.OverlapSphere(transform.position, attackRange, MaskAttack);
                minDistance = 1000f;
                foreach (var collider in hitcolliders)
                {
                    GameObject temp = collider.gameObject;
                    if (!GameObject.ReferenceEquals(temp, gameObject))
                    {
                        if (LayerMask.LayerToName(temp.layer).Equals("Player", System.StringComparison.Ordinal) || LayerMask.LayerToName(temp.layer).Equals("Enemy", System.StringComparison.Ordinal))
                        {
                            float distance = (transform.position - temp.transform.position).magnitude;
                            if (distance <= minDistance)
                            {
                                closestObj = temp;
                                minDistance = distance;
                                break;
                            }
                        }
                    }
                }
            }
            else _timerReact -= Time.deltaTime;
            //Have enemy in range
            if (closestObj != null)
            {
                if (BotInfo.Team != closestObj.GetComponent<VSPlayerInfo>().Team)
                {
                    LayerMask maskCheck = LayerMask.GetMask("Barrier", "Border", "Ground", "ObstacleLayer");
                    if (!Physics.Raycast(transform.position + new Vector3(0, 2f, 0), (-transform.position + closestObj.transform.position).normalized, (transform.position - closestObj.transform.position).magnitude, maskCheck))
                    {
                        if (!isAlreadyAttack)
                        {
                            transform.DOLookAt(closestObj.transform.position, 0.2f);
                            attackPosition = closestObj.transform.position;
                            Attack();
                        }
                    }
                }
            }
            else agent.SetDestination(walkPoint);
        }

        //In Zone?
        if (isInZone() && VSGlobals.MODE == "Domination")
        {
            if (zoneStaying.GetComponent<VSZoneController>().TeamOccupying != PlayerInfo.Team.TeamSide)
            {
                agent.SetDestination(transform.position);
                ControlAnimator.Idle();
            }
        }

    }
    public void SearchWalkPoint()
    {
        isReachingWalkPoint = false;

        int chance = Random.Range(1, 100);
        if (VSGlobals.MODE == "Domination")
        {
            if (chance % 2 == 0 && ZonePoints.Count != 0) walkPoint = ZonePoints[new System.Random().Next(ZonePoints.Count)].position;
            else if (chance % 2 != 0)
            {
                for (int i = 0; i < 30; i++)
                {
                    Vector3 randomPoint = transform.position + Random.insideUnitSphere * 200f;
                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(randomPoint, out hit, 1f, NavMesh.AllAreas))
                    {
                        walkPoint = hit.position;
                        break;
                    }
                }
            }
        }
        else if (VSGlobals.MODE == "Deathmatch")
        {
            for (int i = 0; i < 30; i++)
            {
                Vector3 randomPoint = transform.position + Random.insideUnitSphere * 200f;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPoint, out hit, 1f, NavMesh.AllAreas))
                {
                    walkPoint = hit.position;
                    break;
                }
            }
        }

        agent.SetDestination(walkPoint);
        ControlAnimator.Idle();
    }
    bool IsReachingWalkPoint()
    {
        //distanceToWalkPoint = transform.position - walkPoint;

        if (agent.remainingDistance < 4f) return true;
        else return false;
    }

    void Attack()
    {
        isAlreadyAttack = true;
        //Throw Nade??
        int chanceThrowNade = Random.Range(1, 11);
        if (chanceThrowNade <= 3) ThrowNade();
        //
        agent.SetDestination(transform.position);
        //ControlAnimator.Idle();
    }
    void Shoot()
    {
        ControlAnimator.Shoot();
        //Shoot bullet
        shootVfx.Spawn();
        SoundManager.EnableBulletSound(GunUsing.Bullet.BulletSound);
        //Recoil
        WeaponRecoil.Recoil();
        //Check Raycast hit
        LayerMask mask = LayerMask.GetMask("BodyPart", "Barrier", "Border", "Ground", "Smoke", "ObstacleLayer", "MainPlayerBodyPart");
        Vector3 startPosition = fpCamera.transform.position;
        Vector3 direction = (closestObj.transform.position - transform.position).normalized;
        Physics.Raycast(startPosition, direction, out RaycastHit hit, Mathf.Infinity, mask);
        //Cheat Bullet's velocity = Vector between RaycastHit's point and FirePoint
        Vector3 bulletVelocity;
        //if (hit.point != Vector3.zero) bulletVelocity = (hit.point - fpCamera.position).normalized * firePower;
        bulletVelocity = direction * firePower;
        BulletPool.instance.PickFromPool(gameObject, GunUsing, firePoint.position, bulletVelocity, LayerMask.NameToLayer("BulletBot"));
        //CheckRaycastHit(mask, startPosition);
    }


    private void SpawnDecal(RaycastHit hit)
    {
        //Decal on surface
        GameObject decal = Instantiate(BulletDecalPrefab, hit.point, Quaternion.LookRotation(hit.normal));
        decal.transform.position += 0.1f * hit.normal;
    }

    void ThrowNade()
    {
        if (_nadeAmount > 0)
        {
            GameObject nade = Instantiate(GrenadePrefab, ThrowNadePos.position, Quaternion.identity);
            nade.GetComponent<VSNadeHandle>().WhoThrow = PlayerInfo;
            nade.GetComponent<Rigidbody>().AddForce(fpCamera.transform.up * 10f, ForceMode.Impulse);
            nade.GetComponent<Rigidbody>().AddForce(fpCamera.transform.forward * 30f, ForceMode.Impulse);
            _nadeAmount--;
        }
    }
    bool isInZone()
    {
        foreach (Transform zone in ZonePoints)
        {
            //Debug.Log((transform.position - zone.position).magnitude)
            if ((transform.position - zone.position).magnitude <= zoneRadius)
            {
                zoneStaying = zone;
                return true;
            }
        }
        return false;
    }
    void EquipGun(VSGun gun)
    {
        if (gun == null) return;

        firePower = gun.FirePower;
        fireSpeed = gun.FireSpeed * 5f;
        WeaponRecoil.SetRecoil(gun.RecoilAmountX, gun.RecoilAmountY);

        GameObject gunHolder = GetComponentInChildren<GunHolder>().gameObject;
        Instantiate(GunUsing.ModelForBot, gunHolder.transform);
    }
    public void SetCharacterSkin(GameObject skinPrefab)
    {
        GameObject skin = Instantiate(skinPrefab, SkinModelLocate);
        EquipGun(GunUsing);
        firePoint = GetComponentInChildren<VSMuzzle>().transform;
        skin.GetComponent<Animator>().runtimeAnimatorController = GunUsing.AnimatorControllerForBot;
        ControlAnimator.Controller = skin.GetComponent<Animator>();
    }
}
