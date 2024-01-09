using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    /// <summary>
    /// ���� ���� �� ����� ��������Ʈ
    /// </summary>
    protected Action onDie;

    //lambda


    /*    float timer = 0;
        public float speed = 1f;
        public float waveTime = 2f;
        public float waveStength = 1.2f;
        float wave;*/

    Collider2D coll;

    float spawnY = 0.0f;
    float elapsedTime = 0.0f;

    [Header("# Stats")]
    public int hp = 3;
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
    /// �÷��̾ �� ���� �׿��� �� ��� ����
    /// </summary>
    public int score = 10;


    public float speed; // �̵��ӵ�
    public float amplitude = 3.0f; // �� �Ʒ��� �����̴� �ӵ�
    public float frequency = 2.0f; // ���α׷����� �ѹ� �պ��ϴµ� �ɸ��� �ð�

    [Header("# obj")]
    public GameObject explosionEffect; // explosionEffect


    void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    void Start()
    {
        spawnY = transform.position.y;
        elapsedTime = 0.0f;

        //Action aa = () => Debug.Log("�����Լ�");
        //Action<int> bb = (x) => Debug.Log($"�����Լ� {x}"); // �Ķ���� �ޱ�
        //Func<int> cc = () => 10; // �Ķ���� x, �׻� 10 ����

        Player player = FindAnyObjectByType<Player>();
        onDie += () => player.AddScore(score); // ������ addScore �Լ� ���� ��� 
    }

    void Update()
    {
        //elapsedTime += Time.deltaTime;
        elapsedTime += Time.deltaTime * frequency; // sin �׷��� ���� ������ �ϱ�

        //waveSelf();
        transform.position = new Vector3(transform.position.x - Time.deltaTime * speed,
                                  spawnY + Mathf.Sin(elapsedTime) * amplitude,
                                  0.0f);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        hitDamage(collision);
    }

    void hitDamage(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Hp--;
        }
    }

    void WaveSelf()
    {
        /*timer += Time.deltaTime;


        if (timer > waveTime)
        {
            timer = 0;

            waveStength *= -1f;
        }

        transform.position += new Vector3(-speed * Time.deltaTime, waveStength * Time.deltaTime);*/
    }


    public void OnDie()
    {
        Instantiate(explosionEffect, transform.position, Quaternion.identity);

        //Player player = FindAnyObjectByType<Player>();

        onDie?.Invoke();

        Destroy(gameObject);
    }
   

    // ���� �÷��̾�� ������ �ִ� ����� ��������Ʈ�� ó���ϵ��� ������
    // ����� �Ͼ�� ��ondie(��������Ʈ ����),������ �ۿ�player�� �Ͼ�� ��(��������Ʈ�� �Լ��� ���)
    // ���� ���ڰ� ���ʷ� �ö󰡵��� ����
}