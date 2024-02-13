using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Waypoints 클래스에 지정된 웨이포인트들을 따라 움직이는 역할을 하는 클래스
/// </summary>
public class Waypoint_User : MonoBehaviour
{
    /// <summary>
    /// 이 오브젝트가 따라 움직일 경로를 가진 웨이포인트들
    /// </summary>
    public Waypoints targetWaypoints;

    /// <summary>
    /// 이동 속도
    /// </summary>
    public float moveSpeed = 5.0f;

    /// <summary>
    /// 이번 물리 프레임에서 이동한 정도
    /// </summary>
    protected Vector3 moveDelta = Vector3.zero;

    /// <summary>
    /// 이동방향
    /// </summary>
    protected Vector3 moveDirection;

    /// <summary>
    /// 현재 목표로 하고 있는 웨이포인트 지점의 트랜스폼
    /// </summary>
    Transform target;

    /// <summary>
    /// 목표로 할 웨이포인트를 지정하고 확인하는 프로퍼티
    /// </summary>
    /// 

    protected virtual Transform Target
    {
        get => target;
        set
        {
            target = value;
            moveDirection = (target.position - transform.position).normalized;
        }
    }

    /// <summary>
    /// 현재 위치가 도착지점에 근접했는지 확인하는 프로퍼티(true면 목표 웨이포인트에 도착, false면 도착 안함)
    /// </summary>
    bool IsArrived
    {
        get
        {
            return (target.position - transform.position).sqrMagnitude < 0.01f;
        }
    }

    private void Start()
    {
        //transform.position = target.position; // 위치 초기화

        Target = targetWaypoints.CurrentWayPoint;
    }

    void FixedUpdate()
    {
        moveDelta = moveDirection * moveSpeed * Time.fixedDeltaTime;
        OnMove();
    }

    /// <summary>
    /// 이동 처리용 함수
    /// </summary>
    protected virtual void OnMove()
    {
        // 이동처리
        moveDelta = Time.fixedDeltaTime * moveSpeed * moveDirection;
        transform.Translate(moveDelta, Space.World);

        if(IsArrived) // true면 웨이포인트 지점에 도착
        {
            OnArrived();
        }
    }

    /// <summary>
    /// 웨이 포인트 지점에 도착 했을 때 함수
    /// </summary>
    protected virtual void OnArrived()
    {
        Target = targetWaypoints.GetNextWayPoint();
    }
}
