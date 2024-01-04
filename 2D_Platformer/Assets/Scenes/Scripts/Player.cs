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
    const string isShot_string = "isShot";
    const string isRunningShot_string = "isRunningShot";

    [Header("#Player Stats")]
    public float _moveSpeed;
    public float _moveX;
    public float _shotDelay = 0.2f;
    public float _jumpPower;
    public bool _isShot = false;
    public bool _isJump = false;
    public bool _isShotting_Anim = false;

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
    }



    void OnDisable()
    {
        _inputActions.Player.Fire.canceled -= OnFire;
        _inputActions.Player.Fire.performed -= OnFire;
        _inputActions.Player.Jump.canceled -= OnJump;
        _inputActions.Player.Jump.performed -= OnJump;
        _inputActions.Player.Move.canceled -= OnMove;
        _inputActions.Player.Move.performed -= OnMove;
        _inputActions.Player.Disable();
    }

    void FixedUpdate()
    {
        transform.Translate(Time.fixedDeltaTime * _inputMove * _moveSpeed);
        _anim.SetBool(isJump_String, _isJump);

        if (_inputMove.x != 0)
        {
            _sprite.flipX = _inputMove.x < 0;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            _isJump = false;
        }
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (_isShotting_Anim)
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
    }

    private void OnFire(InputAction.CallbackContext context)
    {
        if (_isJump || _isShot)
            return;

        if (context.performed)
        {
            StartCoroutine(Shot_Corutine());

            if (_inputMove.x != 0)
            {
                _anim.SetTrigger(isRunningShot_string);
            }
            else
            {
                _anim.SetTrigger(isShot_string);
            }
            _isShotting_Anim = true;
        }

        if(context.canceled)
        {
            if (_inputMove.x != 0)
            {
                _anim.SetTrigger(isRunningShot_string);
            }
            else
            {
                _anim.SetTrigger(isShot_string);
            }
            _isShotting_Anim = false;
        }
    }

    IEnumerator Shot_Corutine()
    {
        Instantiate(_bullet, _bulletPos.transform.position, Quaternion.identity);
        _isShot = true;
        yield return new WaitForSeconds(_shotDelay);
        _isShot = false;
    }
}
