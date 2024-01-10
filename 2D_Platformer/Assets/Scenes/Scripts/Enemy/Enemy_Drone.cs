using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Drone : Enemy
{
    [Header("#Ray info")]
    public float _rayOffsetX;
    public float _rayOffsetY;
    public LayerMask _targetMast;
    private RaycastHit2D _rayTarget;

    void Update()
    {
        CheckGround();
    }

    /// <summary>
    /// check tag Ground using rayLine
    /// </summary>
    void CheckGround()
    {
        Vector2 _startRay = new Vector3(transform.position.x + _rayOffsetX,
                                transform.position.y);
        Vector2 _endRay = new Vector3(_startRay.x, _startRay.y + _rayOffsetY);

        _rayTarget = Physics2D.Linecast(_startRay, _endRay, _targetMast);
        Debug.DrawRay(_startRay, (_endRay - _startRay));

        if (_rayTarget.collider != null && _rayTarget.collider.gameObject.CompareTag("Ground"))
        {
            Debug.Log($"tag : {_rayTarget.collider.gameObject.tag}");
        }
    }
}
