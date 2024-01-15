using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidMini : EnemyBase
{
    //이동속도가 기본속도에서 +-1;
    // 회전 속도 랜덤
    // 방향 설정이 되어야한다.

    /// <summary>
    /// 기준 속도
    /// </summary>
    float baseSpeed;
    /// <summary>
    /// 회전 속도(0!360)
    /// </summary>
    float rotateSpeed;
    /// <summary>
    /// 이동 방향
    /// </summary>
    private Vector3? direction = null;

    /// <summary>
    /// 이동 방향 을 읽고 쓰기 위한 프로퍼티
    /// </summary>
    public Vector3 Direction
    {
        private get => direction.GetValueOrDefault(); // derection이 null이면 vector3.zero 리턴 아니면 값 리턴
        set
        {
            if (direction == null) // 쓰기는 재활용 될때마다 한번만 가능하도록
                direction = value.normalized;
        }
    }

    void Awake()
    {
        baseSpeed = moveSpeed; // 첫 속도를 기준 속도로
    }

    /// <summary>
    /// 활성화 될때마다 실행
    /// </summary>
    protected override void onInitialze()
    {
        base.onInitialze();

        moveSpeed = baseSpeed + Random.Range(-1.0f, 1.0f);
        rotateSpeed = Random.Range(0, 360.0f);
        direction = null;
    }

    protected override void OnMoveUpdate(float deltaTime)
    {
        transform.Translate((Vector3)(deltaTime * moveSpeed * direction), Space.World); // Direction 방향으로 이동
        transform.Rotate(deltaTime * rotateSpeed * Vector3.forward); // rotatespeed 만큼 회전
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, (Vector3)(transform.position + direction));
    }
}
