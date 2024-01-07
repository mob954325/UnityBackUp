using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
[RequireComponent(typeof(Animator))]

public class Player : MonoBehaviour
{
    // Component
    PlayerInput _inputActions;
    Rigidbody2D _rigid;
    Animator _anim;
    SpriteRenderer _sprite;

    [Header("#Player Obj info")]
    // valueable
    public Vector2 _inputMove = Vector2.zero;
    public GameObject _bulletPos;
    public GameObject _bullet;
    // Animator Parameter
    readonly int speed_String = Animator.StringToHash("speed");
    readonly int isJump_String = Animator.StringToHash("isJump");
    readonly int isCrouch_String = Animator.StringToHash("isCrouch");
    const string isShot_string = "isShot";
    const string isRunningShot_string = "isRunningShot";

    [Header("#Player Stats")]
    public float _moveSpeed;
    public float _moveX;
    public float _shotDelay = 0.2f;
    public float _jumpPower;
    public bool _isFlipX = false;
    public bool _isShot = false;
    public bool _isJump = false;
    public bool _isCrouch = false;

    //public bool _isShotting_Anim = false;

    void Awake()
    {
        _inputActions = new PlayerInput();
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        _inputActions.Player.Enable();
        _inputActions.Player.Move.performed += OnMove;
        _inputActions.Player.Move.canceled += OnMove;
        _inputActions.Player.Jump.performed += OnJump;
        _inputActions.Player.Jump.canceled += OnJump;
        _inputActions.Player.Fire.performed += OnFire;
        _inputActions.Player.Fire.canceled += OnFire;
        _inputActions.Player.Crouch.performed += OnCrouch;
        _inputActions.Player.Crouch.canceled += OnCrouch;
    }

    void OnDisable()
    {
        _inputActions.Player.Crouch.canceled -= OnCrouch;
        _inputActions.Player.Crouch.performed -= OnCrouch;
        _inputActions.Player.Fire.canceled -= OnFire;
        _inputActions.Player.Fire.performed -= OnFire;
        _inputActions.Player.Jump.canceled -= OnJump;
        _inputActions.Player.Jump.performed -= OnJump;
        _inputActions.Player.Move.canceled -= OnMove;
        _inputActions.Player.Move.performed -= OnMove;
        _inputActions.Player.Disable();
    }

    void Update()
    {
        transform.Translate(Time.deltaTime * _inputMove * _moveSpeed);

        _anim.SetBool(isJump_String, _isJump);

        if (_inputMove.x != 0)
        {
            _isFlipX = _inputMove.x < 0;
            _sprite.flipX = _isFlipX;
        }

        if(_isCrouch)
        {
            _inputMove = Vector2.zero;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            _isJump = false;
        }
    }
    private void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _isCrouch = true;

            _inputMove.x = 0;

            if (_isFlipX)
            {
                _bulletPos.transform.localPosition = new Vector3(-1.9f, -0.7f, 0);
            }
            else
            {
                _bulletPos.transform.localPosition = new Vector3(1.9f, -0.7f, 0);
            }
        }

        if(context.canceled)
        {
            _isCrouch = false;

            if (_isFlipX)
            {
                _bulletPos.transform.localPosition = new Vector3(-1.9f, 0.3f, 0);
                _inputMove.x = (-1)*(float)_anim.GetFloat(speed_String);
            }
            else
            {
                _bulletPos.transform.localPosition = new Vector3(1.9f, 0.3f, 0);
                _inputMove.x = (float)_anim.GetFloat(speed_String);
            }
        }

        _anim.SetBool(isCrouch_String, _isCrouch);
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (_isShot || _isCrouch)
            return;

        if(context.performed && !_isJump)
        {
            _rigid.AddForce(Vector3.up * _jumpPower, ForceMode2D.Impulse);
            _isJump = true;
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _inputMove = context.ReadValue<Vector2>();

        _anim.SetFloat(speed_String, Mathf.Abs(_inputMove.x));

        if (context.performed)
        {
            if(_inputMove.x < 0)
            {
                if (_isCrouch)
                {
                    _bulletPos.transform.localPosition = new Vector3(-1.9f, -0.7f, 0);
                }
                else
                {
                    _bulletPos.transform.localPosition = new Vector3(-1.9f, 0.3f, 0);
                }
            }
            else
            {
                if (_isCrouch)
                {
                    _bulletPos.transform.localPosition = new Vector3(1.9f, -0.7f, 0);
                }
                else
                {
                    _bulletPos.transform.localPosition = new Vector3(1.9f, 0.3f, 0);
                }
            }
        }
    }

    private void OnFire(InputAction.CallbackContext context)
    {
        if (_isJump || _isShot)
            return;

        if (context.performed)
        {
            StartCoroutine(Shot_Corutine());

            if (_inputMove.x != 0 || _isCrouch)
            {
                _anim.SetTrigger(isRunningShot_string);
            }
            else
            {
                _anim.SetTrigger(isShot_string);
            }
        }

        if(context.canceled)
        {
            if (_inputMove.x != 0 || _isCrouch)
            {
                _anim.SetTrigger(isRunningShot_string);
            }
            else
            {
                _anim.SetTrigger(isShot_string);
            }
            
        }
    }

    IEnumerator Shot_Corutine()
    {
        GameObject _bulletObj = Instantiate(_bullet, _bulletPos.transform.position, Quaternion.identity);
        _bulletObj.GetComponent<Bullet>()._player = this;

        _isShot = true;
        yield return new WaitForSeconds(_shotDelay);
        _isShot = false;
    }
}
