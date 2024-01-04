using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // 3. 적은 위아래로 파도치듯이 움직인다.
    // 4. 적은 -x 좌표로 움직인다.

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
    public float speed; // 이동속도
    public float amplitude = 3.0f; // 위 아래로 움직이는 속도
    public float frequency = 2.0f; // 사인그래프가 한번 왕복하는데 걸리는 시간

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
        elapsedTime += Time.deltaTime * frequency; // sin 그래프 진행 빠르게 하기

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

    // 1. 적에게 HP 추가 (3대 맞으면 펑)
    // 2. 적이 폭발할 때 explosionEffect 생성
}
