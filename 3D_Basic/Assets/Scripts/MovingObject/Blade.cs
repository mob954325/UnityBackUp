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
            transform.LookAt(Target); // �ٶ󺸴� ���� ����
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

    // 1.Blade : ��������Ʈ�� ��� ���� �� ������ ����
    // 2.WayPoints : GetNextWayPoint �Լ� ����
    // 3.WaypointUser : Target ������Ƽ ����, IsArrived ������Ƽ ����, OnMove �Լ� ����
    // 4.PlatformBase ����� : Ư�� �� ������ ��� �պ��ϴ� �ٴ�(�÷��̾ ž������ �� �̵��� �ڿ����������Ѵ�.)
}
