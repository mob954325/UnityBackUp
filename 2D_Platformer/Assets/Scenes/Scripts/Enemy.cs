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

    [Header("#Enemy Stats")]
    public int hp = 1;
    public float _scanRange; // CircleCast's distance

    protected RaycastHit2D _rayTarget;
    public LayerMask _targetMast;
    public Transform _target;

    void Update()
    {
        // 원점, 지름, 방향, 길이, 감지할 Layer
        _rayTarget = Physics2D.CircleCast(transform.position, _scanRange, Vector2.zero, 0, _targetMast);

        Debug.DrawRay(transform.position, Vector2.right * _scanRange, Color.red);

        if(_rayTarget.collider != null && _rayTarget.collider.gameObject.CompareTag("Player"))
        {
            _target = _rayTarget.collider.gameObject.transform;
            // Debug.Log($"hit Raycast : {_rayTarget.collider.gameObject.name}");

            // attack to player
            //GameObject _bulletObj = Instantiate(_bullet, _bulletPos.transform.position, Quaternion.identity);

        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Bullet"))
        {
            hp--;

            if (hp < 1)
            {
                Instantiate(_deadEffect, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _scanRange);
        //Gizmos.color = Color.blue;
        //Gizmos.DrawLine(shootVector, shootVector + (Vector3.right * transform.localScale.x * rangeAttack));
    }
}
