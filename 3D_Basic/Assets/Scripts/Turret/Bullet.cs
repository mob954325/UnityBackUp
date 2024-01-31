using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : RecycleObject
{
    /// <summary>
    /// 총알의 초기 속도
    /// </summary>
    public float initialSpeed = 20.0f;

    /// <summary>
    /// 총알의 수명
    /// </summary>
    public float lifeTime = 2.0f;

    Rigidbody rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(LifeOver(lifeTime)); // 수명 설정
        rigid.angularVelocity = Vector3.zero; // 이전의 회전력 제거
        rigid.velocity = initialSpeed * transform.forward; // 발사 방향과 속도 설정
    }

    void OnCollisionEnter(Collision collision)
    {
        StopAllCoroutines();
        StartCoroutine(LifeOver(lifeTime)); // 충돌하고 2초 뒤에 사라짐
    }

    void FixedUpdate()
    {
        if(rigid.velocity.sqrMagnitude > 0.1f)
        {
            transform.forward = rigid.velocity;
        }
    }
}
