using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Boss : Enemy
{
    [Header("#Attack Info")]
    public float _scanRange; // CircleCast's distance
    [Header("#Lay Target")]
    [SerializeField]private RaycastHit2D _rayTarget;
    [SerializeField]public LayerMask _targetMast;
    [SerializeField]private Transform _target;

    void Update()
    {
        CheckPlayer();
    }

    void CheckPlayer()
    {
        // 원점, 지름, 방향, 길이, 감지할 Layer
        _rayTarget = Physics2D.CircleCast(transform.position, _scanRange, Vector2.zero, 0, _targetMast);

        Debug.DrawRay(transform.position, Vector2.right * _scanRange, Color.red);

        if(_rayTarget.collider == null)
        {
            _target = null;
        }
        if (_rayTarget.collider != null && _rayTarget.collider.gameObject.CompareTag("Player"))
        {
            _target = _rayTarget.collider.gameObject.transform; // player
            Debug.Log($"{_rayTarget.collider.gameObject.transform}");
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _scanRange);
    }
}
