using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VSPlayerControlAnimator : MonoBehaviour
{
    public Camera Cam;
    public Animator ModelAnimator;
    public Animator WeaponAnimator;

    private int _isModelRunning;
    private int _isRunningHash;
    private int _isIdleHash;
    private int _isShootHash;
    private int _sprayHash;
    private int _isAimHash;
    private void Awake()
    {

        _isModelRunning = Animator.StringToHash("IsModelRunning");
        _isRunningHash = Animator.StringToHash("IsRunning");
        _isIdleHash = Animator.StringToHash("IsIdle");
        _isShootHash = Animator.StringToHash("IsShoot");
        _sprayHash = Animator.StringToHash("Spray");
        _isAimHash = Animator.StringToHash("IsAim");

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Knife() => WeaponAnimator.Play("KnifeAttack");
    public void Shoot()
    {
        WeaponAnimator.SetBool("IsIdle", false);
        WeaponAnimator.SetBool("IsShoot", true);
        float spray = WeaponAnimator.GetFloat("Spray");
        spray += Time.deltaTime * 10f;
        WeaponAnimator.SetFloat("Spray", spray);
    }
    public void Run()
    {
        ModelAnimator.SetBool("IsModelRunning", true);
        WeaponAnimator.SetBool("IsRunning", true);
    }
    public void Idle()
    {
        ModelAnimator.SetBool("IsModelRunning", false);
        WeaponAnimator.SetBool("IsRunning", false);
    }
    public void OnAim()
    {
        WeaponAnimator.Play("Aim");
        WeaponAnimator.SetBool("IsAim", true);
    }
    public void OffAim()
    {

        WeaponAnimator.SetBool("IsAim", false);
        WeaponAnimator.SetBool("IsIdle", true);
    }
    public void RebindAnim()
    {
        ModelAnimator.Rebind();
        WeaponAnimator.Rebind();
    }
}
