using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSwitch : PlatformBase, IInteracable
{
    public bool CanUse => false; // ���ó ����

    /// <summary>
    /// �÷����� ������ �� ������ �����ϴ� �Լ�
    /// </summary>
    bool isMoving = false;

    void Start()
    {
        Target = targetWaypoints.GetNextWayPoint(); // �������� �� �÷����� �ȿ����̴� ���� �ذ�� (�ö��ڸ��� �����ϴ� ����)
    }

    protected override void OnMove()
    {
        if (isMoving)
        {
            base.OnMove(); // �÷��̾ �ö���� �̵��ϱ�
        }
    }

    protected override void OnArrived()
    {
        isMoving = false; // �����ϸ� ���߱�
        base.OnArrived();
    }

    public void Use()
    {
        isMoving = true; // ������ ����ϸ� �����̱�
    }
}
