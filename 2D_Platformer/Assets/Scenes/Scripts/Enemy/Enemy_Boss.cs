using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Boss : Enemy
{
    [Header("#Parameters")]
    public bool _isRunning = false;
    public bool _isFlip = false;
    public bool _isPlayerInAttackArea = false;
    public bool _isAttaked = false;
    public bool _isDead = false;
    public bool _isDash = false;
    public float _attakDelay = 2.0f;
    float _originalSpeed;

    [Header("#Objects")]
    public GameObject _attackArea;

    [Header("#Lay Target")]
    public float _scanRange; // CircleCast's distance
    [SerializeField]private RaycastHit2D _rayTarget;
    [SerializeField]public LayerMask _targetMast;
    [SerializeField]private Transform _target;

    /// <summary>
    /// minimum distance between player and enemy
    /// </summary>
    public float _minDistanceToTarget = 4.6f;

    readonly int isAttack_String = Animator.StringToHash("isAttack");
    readonly int isRunning_String = Animator.StringToHash("isRunning");
    readonly int isDead_String = Animator.StringToHash("isDead");
    SpriteRenderer _spriteRenderer;
    Animator _animtor;

    void Awake()
    {
        _minDistanceToTarget = 4.4f;
        _originalSpeed = _speed;

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animtor = GetComponent<Animator>();
    }
    void Update()
    {
        if(!_isDead)
            CheckPlayer();
    }

    protected override void OnDie()
    {
        base.OnDie();
        _deadEffect.GetComponent<SpriteRenderer>().flipX = !_isFlip;
    }

    void CheckPlayer()
    {
        // ����, ����, ����, ����, ������ Layer
        _rayTarget = Physics2D.CircleCast(transform.position, _scanRange, Vector2.zero, 0, _targetMast);

        Debug.DrawRay(transform.position, Vector2.right * _scanRange, Color.red);

        if(_isAttaked)
        {
            _target = null;
        }
        if (_rayTarget.collider != null && _rayTarget.collider.gameObject.CompareTag("Player"))
        {
            _target = _rayTarget.collider.gameObject.transform; // player
            Vector3 _direction = _target.position - transform.position;
            float _distance = _target.position.magnitude - transform.position.magnitude;

            FlipX(_distance);

            _animtor.SetBool(isRunning_String, !_isAttaked);

            if (_isPlayerInAttackArea)
                return;

            if(Mathf.Abs(_distance) > _minDistanceToTarget) // Move position to player 
            {
                _isRunning = true;
                _isPlayerInAttackArea = false;
                transform.Translate(new Vector3(Time.deltaTime * _speed * _direction.x, 0));
            }
            else
            {
                _isRunning = false;
                _isPlayerInAttackArea = true;

                if(!_isAttaked)
                {
                    _speed = 0f;
                    StartCoroutine(Attack_Corutine());
                    _speed = _originalSpeed;
                }
            }
        }
    }

    void FlipX(float dir)
    {
        if (dir < 0)
        {
            _spriteRenderer.flipX = true;
            if (dir < 0 && !_isFlip)
            {
                _isFlip = true;
                _attackArea.transform.localPosition = new Vector3(_attackArea.transform.localPosition.x * (-1), _attackArea.transform.localPosition.y);
            }
        }
        else
        {
            _spriteRenderer.flipX = false;
            if (dir > 0.1f && _isFlip)
            {
                _isFlip = false;
                _attackArea.transform.localPosition = new Vector3(_attackArea.transform.localPosition.x * (-1), _attackArea.transform.localPosition.y);
            }
        }
    }

    IEnumerator Attack_Corutine()
    {
        _isAttaked = true;
        _animtor.SetBool(isAttack_String, true);
        yield return new WaitForSeconds(0.4f);
        _attackArea.SetActive(true);

        yield return new WaitForSeconds(0.4f);
        _animtor.SetBool(isAttack_String, false);

        _attackArea.SetActive(false);

        yield return new WaitForSeconds(_attakDelay); // attakDelay
        _isAttaked = false;
        _isPlayerInAttackArea = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _scanRange);
    }
}
