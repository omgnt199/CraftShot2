using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VSPlayerMovement : MonoBehaviour
{
    public CharacterController characterController;
    public VSPlayerControlAnimator controlAnimator;
    public GameObject SkinModel;
    public Camera MainCamera;
    public Transform Weapon;
    public VSPlayerSound PlayerSound;
    [Header("Movement")]
    public FixedJoystick joystick;
    public float MoveSpeed;
    public float AccelerationSpeed;
    public float Gravity = -9.81f;
    private float startMoveSpeed;
    public float WalkSpeed;
    public float CrouchSpeed;
    public float AimingSpeed;
    private float _currentSpeed;
    private float _cameraYDefault;
    private float _cameraVibrateDelta = 0.2f;
    private bool _isCapAboveVibrateLimit = false;
    private float _xAxis, _zAxis;
    private Vector3 _moveInput;
    [Header("Ground")]
    public Transform GroundCheck;
    public float GroundDistance = 0.1f;
    public LayerMask GroundMask;
    private bool _isGrounded = true;
    [Header("Jump")]
    public float jumpHeight = 5f;
    Vector3 velocity;
    private bool _isJumping = false;
    private bool _isSpacing = false;
    [Header("Crouch")]
    public float CrouchingSpeed;
    private bool _isCrouching = false;
    Vector3 skinModelScaleStart;
    [Header("Dash")]
    public VisualEffect LineSpeedVfx;
    public float DashTime;
    private Vector3 _dashMotion;
    private float _dashTimer = 0;
    private bool _isDashing = false;
    [Header("Fly")]
    public float FlySpeed;
    private Vector3 _flyMotion;
    private bool _isFly = false;
    private const float DEFAULT_CHARACTER_HEIGHT = 2.6f;
    private const float DEFAULT_FOV = 60f;
    private const float RUNNING_FOV = 65f;
    private const float DASH_FOV = 75f;

    // Start is called before the first frame update
    void Start()
    {
        _cameraYDefault = MainCamera.transform.localPosition.y;
        startMoveSpeed = MoveSpeed;
        _currentSpeed = MoveSpeed;
        skinModelScaleStart = SkinModel.transform.localScale;

        LineSpeedVfx.SendEvent("OnStop");
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();
        Move();
    }
    void Move()
    {
        _isSpacing = _isGrounded;
        //Check ground
        _isGrounded = Physics.CheckSphere(GroundCheck.position, GroundDistance, GroundMask);

        if (!_isGrounded) PlayerSound.DisableFootStep();
        if (_isGrounded && !_isSpacing) PlayerSound.EnableJumpDown();

        if (_isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            _isJumping = false;
        }
        //Dash?
        if(_isDashing) HandleDash();
        //Fly?
        if (_isFly) HandleFLy();
        else
        {
            Gravity = Mathf.Lerp(Gravity, -19.87f, Time.deltaTime * 2f);
            MoveSpeed = Mathf.Lerp(MoveSpeed, WalkSpeed, Time.deltaTime * 2f);
        }
        //
        characterController.Move(_moveInput * MoveSpeed * Time.deltaTime);
        // Add Gravity
        velocity.y += Gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
        //Anim/FootStep
        if (_moveInput.magnitude == 0)
        {
            
            MainCamera.transform.localPosition = Vector3.Lerp(MainCamera.transform.localPosition, new Vector3(MainCamera.transform.localPosition.x, _cameraYDefault, MainCamera.transform.localPosition.z), Time.deltaTime * 12f);
            controlAnimator.Idle();
            PlayerSound.DisableFootStep();
        }
        else
        {

            if (_isGrounded)
            {
                PlayerSound.EnableFootStep();
                if (!_isCapAboveVibrateLimit)
                {
                    MainCamera.transform.localPosition = Vector3.Lerp(MainCamera.transform.localPosition, new Vector3(MainCamera.transform.localPosition.x, _cameraYDefault + _cameraVibrateDelta, MainCamera.transform.localPosition.z), Time.deltaTime * 12f);
                    if (MainCamera.transform.localPosition.y + 0.01f >= _cameraYDefault + _cameraVibrateDelta) _isCapAboveVibrateLimit = true;

                }
                else
                {
                    MainCamera.transform.localPosition = Vector3.Lerp(MainCamera.transform.localPosition, new Vector3(MainCamera.transform.localPosition.x, _cameraYDefault, MainCamera.transform.localPosition.z), Time.deltaTime * 12f);
                    if (MainCamera.transform.localPosition.y - 0.01f <= _cameraYDefault) _isCapAboveVibrateLimit = false;
                }
                controlAnimator.Run();
            }
            //

        }
    }
    public void Jump()
    {
        if(!_isJumping)
        {
            _isJumping = true;
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * Gravity);
        }
    }

    void HandleFLy()
    {
        _flyMotion = MainCamera.transform.forward * 2.5f;
        _moveInput = _flyMotion;
    }
    void EnterDashMode()
    {
        _isDashing = true;
        LineSpeedVfx.SendEvent("OnPlay");
        DOTween.To(() => characterController.height, height => characterController.height = height, DEFAULT_CHARACTER_HEIGHT / 2f, DashTime);
        DOTween.To(() => MainCamera.fieldOfView, fov => MainCamera.fieldOfView = fov, DASH_FOV, 0.2f);
        OnCrouch();
        //_dashMotion = moveInput == Vector3.zero ? transform.forward * 2.5f : moveInput * 2.5f;
        _dashMotion = MainCamera.transform.forward * 2.5f;
    }
    void HandleDash()
    {
        _dashTimer += Time.deltaTime;
        _moveInput = _dashMotion;
        if (_dashTimer >= DashTime)
        {
            LineSpeedVfx.SendEvent("OnStop");
            _dashTimer = 0;
            _isDashing = false;
            SkinModel.transform.localScale = skinModelScaleStart;
            DOTween.To(() => characterController.height, height => characterController.height = height, DEFAULT_CHARACTER_HEIGHT, DashTime / 2f);
            DOTween.To(() => MainCamera.fieldOfView, fov => MainCamera.fieldOfView = fov, DEFAULT_FOV, 0.2f);
        }
    }
    void MyInput()
    {
        //Move Input
#if UNITY_ANDROID || UNITY_IOS
        _xAxis = joystick.Horizontal;
        _zAxis = joystick.Vertical;
#endif
#if UNITY_EDITOR || UNITY_STANDALONE
        _xAxis = Input.GetAxis("Horizontal");
        _zAxis = Input.GetAxis("Vertical");
#endif
        _moveInput = transform.right * _xAxis + transform.forward * _zAxis;
        //Crouching input
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            OnCrouch();
            SpeedOnCrouching();
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            OffCrouch();
            SpeedOnWalking();
        }
        //Jump input
        if (Input.GetKeyDown(KeyCode.Space)) Jump();
        //Dash input
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (!_isDashing) EnterDashMode();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            _isFly = !_isFly;
            if (_isFly)
            {
                Gravity = 0f;
                MoveSpeed = FlySpeed;
            }
        }
    }
    void OnCrouch()
    {
        SkinModel.transform.localScale = new Vector3(skinModelScaleStart.x, skinModelScaleStart.y / 2f, skinModelScaleStart.z);
        characterController.height = DEFAULT_CHARACTER_HEIGHT / 2f;
    }
    void OffCrouch()
    {
        SkinModel.transform.localScale = skinModelScaleStart;
        characterController.height =  DEFAULT_CHARACTER_HEIGHT;
    }
    public void SpeedOnWalking()
    {
        MoveSpeed = WalkSpeed;
    }
    public void SpeedOnCrouching()
    {
        MoveSpeed = CrouchSpeed;
    }
    public void SpeedOnAiming()
    {
        MoveSpeed = AimingSpeed;
    }
}
