using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidMini : EnemyBase
{
    //�̵��ӵ��� �⺻�ӵ����� +-1;
    // ȸ�� �ӵ� ����
    // ���� ������ �Ǿ���Ѵ�.

    /// <summary>
    /// ���� �ӵ�
    /// </summary>
    float baseSpeed;
    /// <summary>
    /// ȸ�� �ӵ�(0!360)
    /// </summary>
    float rotateSpeed;
    /// <summary>
    /// �̵� ����
    /// </summary>
    private Vector3? direction = null;

    /// <summary>
    /// �̵� ���� �� �а� ���� ���� ������Ƽ
    /// </summary>
    public Vector3 Direction
    {
        private get => direction.GetValueOrDefault(); // derection�� null�̸� vector3.zero ���� �ƴϸ� �� ����
        set
        {
            if (direction == null) // ����� ��Ȱ�� �ɶ����� �ѹ��� �����ϵ���
                direction = value.normalized;
        }
    }

    void Awake()
    {
        baseSpeed = moveSpeed; // ù �ӵ��� ���� �ӵ���
    }

    /// <summary>
    /// Ȱ��ȭ �ɶ����� ����
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
        transform.Translate((Vector3)(deltaTime * moveSpeed * direction), Space.World); // Direction �������� �̵�
        transform.Rotate(deltaTime * rotateSpeed * Vector3.forward); // rotatespeed ��ŭ ȸ��
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, (Vector3)(transform.position + direction));
    }
}
