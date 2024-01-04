using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // 3. ���� ���Ʒ��� �ĵ�ġ���� �����δ�.
    // 4. ���� -x ��ǥ�� �����δ�.

    /// 
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
    public float speed; // �̵��ӵ�
    public float amplitude = 3.0f; // �� �Ʒ��� �����̴� �ӵ�
    public float frequency = 2.0f; // ���α׷����� �ѹ� �պ��ϴµ� �ɸ��� �ð�

    [Header("# obj")]
    public GameObject explosionEffect;

    void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    void Start()
    {
        spawnY = transform.position.y;
        elapsedTime = 0.0f;
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Bullet"))
        {
            hp--;
            if (hp < 1)
            {
                Instantiate(explosionEffect, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }

        }
    }

    // 1. ������ HP �߰� (3�� ������ ��)
    // 2. ���� ������ �� explosionEffect ����
}
