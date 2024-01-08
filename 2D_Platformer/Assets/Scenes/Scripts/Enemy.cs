using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum Type { Drone, Turret }
public class Enemy : MonoBehaviour
{
    public Type type;

    public GameObject _deadEffect;
    public GameObject _bullet;
    public GameObject _bulletHitEffect;

    [Header("#Enemy Stats")]
    public int _hp = 1;
    public float _bulletSpeed;
    public float _scanRange; // CircleCast's distance

    public float _shotIntervalTime;
    WaitForSeconds _shotInterval;
    [SerializeField]bool _isShot = false;

    [Header("#Lay Target")]
    protected RaycastHit2D _rayTarget;
    public LayerMask _targetMast;
    public Transform _target;
    [SerializeField]private Vector3 _bulletDirection;

    void Awake()
    {
    }

    void Update()
    {
        _shotInterval = new WaitForSeconds(_shotIntervalTime);
        // 원점, 지름, 방향, 길이, 감지할 Layer
        _rayTarget = Physics2D.CircleCast(transform.position, _scanRange, Vector2.zero, 0, _targetMast);

        Debug.DrawRay(transform.position, Vector2.right * _scanRange, Color.red);

        if(_rayTarget.collider != null && _rayTarget.collider.gameObject.CompareTag("Player"))
        {
            _target = _rayTarget.collider.gameObject.transform; // player
            _bulletDirection = _target.transform.position - transform.position;
            //Debug.Log($"_bulletDirection : {_bulletDirection}");

            // create enemy bullet

            if(!_isShot) StartCoroutine(Shot());
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("PlayerBullet"))
        {
            _hp--;

            if (_hp < 1)
            {
                Instantiate(_deadEffect, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// Init BulletParameter
    /// </summary>
    /// <param name="bullet">add bullet override component</param>
    void InitBullet(Bullet_Enemy bullet)
    {
        bullet._speed = _bulletSpeed;
        bullet._direction = _bulletDirection;
        bullet._hitEffect = _bulletHitEffect;
        bullet.tag = "EnemyBullet";
        bullet._enemy = this;
    }

    IEnumerator Shot()
    {
        _isShot = true;

        GameObject _bulletObj = Instantiate(_bullet, transform.position, Quaternion.identity);
        Bullet_Enemy _bulletEnemy = _bulletObj.AddComponent<Bullet_Enemy>();
        InitBullet(_bulletEnemy);

        yield return _shotInterval;

        _isShot = false;

        Shot();
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _scanRange);
        //Gizmos.color = Color.blue;
        //Gizmos.DrawLine(shootVector, shootVector + (Vector3.right * transform.localScale.x * rangeAttack));
    }
}
