using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Waypoints Ŭ������ ������ ��������Ʈ���� ���� �����̴� ������ �ϴ� Ŭ����
/// </summary>
public class Waypoint_User : MonoBehaviour
{
    /// <summary>
    /// �� ������Ʈ�� ���� ������ ��θ� ���� ��������Ʈ��
    /// </summary>
    public Waypoints targetWaypoints;

    /// <summary>
    /// �̵� �ӵ�
    /// </summary>
    public float moveSpeed = 5.0f;

    /// <summary>
    /// �̹� ���� �����ӿ��� �̵��� ����
    /// </summary>
    protected Vector3 moveDelta = Vector3.zero;

    /// <summary>
    /// �̵�����
    /// </summary>
    protected Vector3 moveDirection;

    /// <summary>
    /// ���� ��ǥ�� �ϰ� �ִ� ��������Ʈ ������ Ʈ������
    /// </summary>
    Transform target;

    /// <summary>
    /// ��ǥ�� �� ��������Ʈ�� �����ϰ� Ȯ���ϴ� ������Ƽ
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
    /// ���� ��ġ�� ���������� �����ߴ��� Ȯ���ϴ� ������Ƽ(true�� ��ǥ ��������Ʈ�� ����, false�� ���� ����)
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
        //transform.position = target.position; // ��ġ �ʱ�ȭ

        Target = targetWaypoints.CurrentWayPoint;
    }

    void FixedUpdate()
    {
        moveDelta = moveDirection * moveSpeed * Time.fixedDeltaTime;
        OnMove();
    }

    /// <summary>
    /// �̵� ó���� �Լ�
    /// </summary>
    protected virtual void OnMove()
    {
        // �̵�ó��
        moveDelta = Time.fixedDeltaTime * moveSpeed * moveDirection;
        transform.Translate(moveDelta, Space.World);

        if(IsArrived) // true�� ��������Ʈ ������ ����
        {
            OnArrived();
        }
    }

    /// <summary>
    /// ���� ����Ʈ ������ ���� ���� �� �Լ�
    /// </summary>
    protected virtual void OnArrived()
    {
        Target = targetWaypoints.GetNextWayPoint();
    }
}
