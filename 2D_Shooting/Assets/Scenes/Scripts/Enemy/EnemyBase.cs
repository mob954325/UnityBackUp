using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : RecycleObject
{

    [Header("# Stats")]
    int hp = 1;
    private int Hp
    {
        get => hp;
        set
        {
            hp = value;
            if(hp <= 0)
            {
                hp = 0;
                OnDie();
            }
        }
    }

    /// <summary>
    /// �ִ� hp
    /// </summary>
    public int MaxHp = 1;

    /// <summary>
    /// �÷��̾ �� ���� �׿��� �� ��� ����
    /// </summary>
    public int score = 10;

    public float moveSpeed; // �̵��ӵ�

    [Header("# obj")]
    public GameObject explosionEffect; // explosionEffect

    /// <summary>
    /// ������ �� �÷��̾�
    /// </summary>
    public Player player;

    /// <summary>
    /// ���� ���� �� ����� ��������Ʈ
    /// </summary>
    protected Action onDie;

    protected override void OnEnable()
    {
        base.OnEnable();
        onInitialze();
    }

    protected override void OnDisable()
    {
        if(player != null)
        {
            onDie -= PlayerAddScore; // ���������� ����
            onDie = null; // Ȯ���ϰ� �����Ѵٰ� ǥ�� , null�� �ᵵ ����
            player = null;
        }

        base.OnDisable();
    }
    
    /// <summary>
    /// �÷��̾� ���� �߰��� �Լ�(��������Ʈ ��Ͽ�)
    /// </summary>
    void PlayerAddScore()
    {
        player.AddScore(score);
    }

    void Update()
    {
        OnMoveUpdate(Time.deltaTime);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        hitDamage(collision);
    }

    void hitDamage(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet") // �Ѿ� �Ǵ� �÷��̾ �ε�ġ�� 1 ����
         || collision.gameObject.CompareTag("Player"))
        {
            Hp--;
        }
    }

    /// <summary>
    /// Enemy �迭�� �ʱ�ȭ �Լ�
    /// </summary>
    protected virtual void onInitialze()
    {
        if(player = null)
        {
            player = GameManager.Instance.Player; // �÷��̾� ã��
        }
        if (player != null)
        {
            onDie += PlayerAddScore; // �÷��̾� ���� ���� �Լ� ���
        }

        Hp = MaxHp; // Hp �ִ�� ���
    }

    /// <summary>
    /// ������Ʈ �߿� �̵�ó���ϴ� �Լ�
    /// </summary>
    /// <param name="deltaTime">�����Ӱ��� ����</param>
    protected virtual void OnMoveUpdate(float deltaTime)
    {
        transform.Translate(deltaTime * moveSpeed * -transform.right, Space.World);// �⺻���� : �������� �̵�
    }

    /// <summary>
    /// ���ó���� �Լ�
    /// </summary>
    protected virtual void OnDie()
    {
        // ������ ����Ʈ ����
        Factory.Instance.GetExplosion(transform.position);

        onDie?.Invoke(); // �׾��ٴ� ��ȣ ������

        gameObject.SetActive(false);
    }
   
}