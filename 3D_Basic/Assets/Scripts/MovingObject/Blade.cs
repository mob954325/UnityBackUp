using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : Waypoint_User
{
    public float spinSpeed = 720.0f;
    Transform bladeMesh;

    protected override Transform Target 
    {
        set
        {
            base.Target = value;
            transform.LookAt(Target); // 바라보는 방향 설정
        }
    }

    Rigidbody rigid;

    void Awake()
    {
        bladeMesh = transform.GetChild(0);
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        bladeMesh.Rotate(Time.deltaTime * spinSpeed * Vector3.right);
    }

    void OnCollisionEnter(Collision collision)
    {
        IAlive live = collision.gameObject.GetComponent<IAlive>();
        if (live != null)
        {
            live.Die();
        }
    }

    protected override void OnMove()
    {
        rigid.MovePosition(transform.position + moveDelta);
        base.OnMove();
    }

    // 1.Blade : 웨이포인트를 사용 했을 때 문제점 수정
    // 2.WayPoints : GetNextWayPoint 함수 구현
    // 3.WaypointUser : Target 프로퍼티 구현, IsArrived 프로퍼티 구현, OnMove 함수 구현
    // 4.PlatformBase 만들기 : 특정 두 지점을 계속 왕복하는 바닥(플레이어가 탑승했을 때 이동이 자연스러워야한다.)
}
