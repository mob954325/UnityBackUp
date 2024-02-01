using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    //�Ϸ�
    //1. player tag�� ���� ������Ʈ ã�� 
    //2. �ش� ������Ʈ���� �ٰ����� : ���� �����Ÿ� ������ 
    // �̿Ϸ�
    //3. �����ϱ� -> ��� -> ���� ������ �۵� 

    // components
    Player player;
    Rigidbody rigid;

    // enemy info
    public float speed = 5.0f;
    public float range = 5.0f;



    void Awake()
    {
        player = FindAnyObjectByType<Player>();
        rigid = GetComponent<Rigidbody>();
        Debug.Log($"������ �÷��̾� ������Ʈ �̸� : {player.gameObject.name}");
    }

    void FixedUpdate()
    {
        MoveToPlayer();
    }

    void MoveToPlayer()
    {
        Vector3 moveDirection = player.transform.position - transform.position; // player

        if(moveDirection.magnitude > range)
        {
            rigid.MovePosition(rigid.position + Time.fixedDeltaTime * moveDirection * speed);
        }

        Debug.Log($"�÷��̾���� ������ �Ÿ� : {moveDirection.magnitude}");
    }
}
