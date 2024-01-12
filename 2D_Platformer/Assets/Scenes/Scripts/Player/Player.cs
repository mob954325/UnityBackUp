using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using Unity.Mathematics;
[RequireComponent(typeof(Animator))]

public class Player : MonoBehaviour
{
    // Component
    PlayerInput _inputActions;
    Rigidbody2D _rigid;
    Animator _animator;
    SpriteRenderer _sprite;

    // delegate
    Action<int> _changeScore;

    [Header("Score")]
    [SerializeField] private int _maxScore = 9999;
    [SerializeField] private int _score;
    public int _Score
    {
        get => _score;
        set
        {
            if (_score != value)
            {
                _score = Math.Max(value, _maxScore);
                //_changeScore?.Invoke(_score); delegate to score UI
            }
        }

    }

    [Header("#Player Obj info")]
    // valueable
    public Vector2 _inputMove = Vector2.zero;
    public GameObject _bulletPos;
    public GameObject _bullet;
    public GameObject _bulletHitEffect;
    // Animator Parameter
    readonly int speed_String = Animator.StringToHash("speed");
    readonly int isJump_String = Animator.StringToHash("isJump");
    readonly int isCrouch_String = Animator.StringToHash("isCrouch");
    readonly int isClimb_String = Animator.StringToHash("isClimb");
    readonly int doneClimb_String = Animator.StringToHash("doneClimb");

    const string isShot_string = "isShot";
    const string isRunningShot_string = "isRunningShot";

    [Header("#Player Stats")]
    float _gravityScale = 3.5f;

    public float _bulletSpeed;
    public float _shotDelay = 0.2f;

    public float _moveSpeed;
    public float _moveX;

    public float _jumpPower;
    public float _hitPower = 1.2f; // 피격 밀림 크기

    public bool _isFlipX = false; // 스프라이트 뒤집혀짐 여부
    public bool _isShot = false; // 사격 여부
    public bool _isJump = false; // 점프 여부
    public bool _isCrouch = false; // 앉기키 누름 여부
    public bool _isHit = false; // 피격 여부
    public bool _canClimb = false;

    //public bool _isShotting_Anim = false;

    public void AddScore(int getScore)
    {
        _score += getScore;
    }

    void Awake()
    {
        _gravityScale = 3.5f;

        _inputActions = new PlayerInput();
        _rigid = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
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

        _animator.SetBool(isJump_String, _isJump);

        if (_inputMove.x != 0)
        {
            _isFlipX = _inputMove.x < 0;
            _sprite.flipX = _isFlipX;
        }

        if(_isCrouch)
        {
            _inputMove = Vector2.zero;
        }

        if(!_canClimb)
        {
            _inputMove.y = 0f;
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        // check Player jump
        if(collision.gameObject.CompareTag("Block"))
        {
            _isJump = false;
        }

        // check enemy attack
        if((collision.gameObject.CompareTag("EnemyBullet") || collision.gameObject.CompareTag("Enemy")) && !_isHit)
        {
            Vector3 _hitDir = transform.position - collision.transform.position;
            _rigid.AddForce(_hitDir * _hitPower, ForceMode2D.Impulse);
            //_rigid.velocity = Vector3.zero;

            StartCoroutine(Hit_Corutine());
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            _canClimb = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            _canClimb = false;
            _animator.SetTrigger(doneClimb_String);
            _rigid.gravityScale = _gravityScale;
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
                _inputMove.x = (-1)*(float)_animator.GetFloat(speed_String);
            }
            else
            {
                _bulletPos.transform.localPosition = new Vector3(1.9f, 0.3f, 0);
                _inputMove.x = (float)_animator.GetFloat(speed_String);
            }
        }

        _animator.SetBool(isCrouch_String, _isCrouch);
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (_isShot || _isCrouch || _canClimb)
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
        _inputMove.y = 0f;

        _animator.SetFloat(speed_String, Mathf.Abs(_inputMove.x));

        if (context.performed)
        {
            /// flip Sprite
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

            if(_canClimb)
            {
                _isJump = false;
                _animator.SetTrigger(isClimb_String);
                _inputMove.y = context.ReadValue<Vector2>().y;
                _rigid.gravityScale = 0f;
            }
        } // performed
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
                _animator.SetTrigger(isRunningShot_string);
            }
            else
            {
                _animator.SetTrigger(isShot_string);
            }
        }

        if(context.canceled)
        {
            if (_inputMove.x != 0 || _isCrouch)
            {
                _animator.SetTrigger(isRunningShot_string);
            }
            else
            {
                _animator.SetTrigger(isShot_string);
            }
            
        }
    }


    /// <summary>
    /// 공격 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator Shot_Corutine()
    {
        GameObject _bulletObj = Instantiate(_bullet, _bulletPos.transform.position, Quaternion.identity);
        Bullet_Player _bulletPlayer = _bulletObj.AddComponent<Bullet_Player>();
        _bulletPlayer._speed = _bulletSpeed;
        _bulletPlayer._hitEffect = _bulletHitEffect;
        _bulletPlayer._player = this;
        _bulletPlayer.tag = "PlayerBullet";

        _isShot = true;
        yield return new WaitForSeconds(_shotDelay);
        _isShot = false;
    }

    /// <summary>
    /// 피격 코루틴 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator Hit_Corutine()//
    {
        _sprite.color = new Color(0.5f, 0.5f, 0.5f, 1);
        _isHit = true;
        yield return new WaitForSeconds(1f);
        _sprite.color = Color.white;
        _isHit = false;
    }
}
