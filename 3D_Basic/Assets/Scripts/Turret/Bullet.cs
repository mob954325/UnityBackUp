using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : RecycleObject
{
    /// <summary>
    /// �Ѿ��� �ʱ� �ӵ�
    /// </summary>
    public float initialSpeed = 20.0f;

    /// <summary>
    /// �Ѿ��� ����
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
        StartCoroutine(LifeOver(lifeTime)); // ���� ����
        rigid.angularVelocity = Vector3.zero; // ������ ȸ���� ����
        rigid.velocity = initialSpeed * transform.forward; // �߻� ����� �ӵ� ����
    }

    void OnCollisionEnter(Collision collision)
    {
        StopAllCoroutines();
        StartCoroutine(LifeOver(lifeTime)); // �浹�ϰ� 2�� �ڿ� �����
    }

    void FixedUpdate()
    {
        if(rigid.velocity.sqrMagnitude > 0.1f)
        {
            transform.forward = rigid.velocity;
        }
    }
}
