using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTrigger : PlatformBase
{
    /// <summary>
    /// �÷����� ������ �� ������ �����ϴ� �Լ�
    /// </summary>
    bool isMoving = false;

    void Start()
    {
        Target = targetWaypoints.GetNextWayPoint(); // �������� �� �÷����� �ȿ����̴� ���� �ذ�� (�ö��ڸ��� �����ϴ� ����)
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            isMoving = true; 
        }
    }

    protected override void OnMove()
    {
        if(isMoving)
        {
            base.OnMove(); // �÷��̾ �ö���� �̵��ϱ�
        }
    }

    protected override void OnArrived()
    {
        isMoving = false; // �����ϸ� ���߱�
        base.OnArrived();
    }
}
