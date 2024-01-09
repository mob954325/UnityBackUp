using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    /// <summary>
    /// 적이 죽을 때 실행될 델리게이트
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
    /// 플레이어가 이 적을 죽였을 때 얻는 점수
    /// </summary>
    public int score = 10;


    public float speed; // 이동속도
    public float amplitude = 3.0f; // 위 아래로 움직이는 속도
    public float frequency = 2.0f; // 사인그래프가 한번 왕복하는데 걸리는 시간

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

        //Action aa = () => Debug.Log("람다함수");
        //Action<int> bb = (x) => Debug.Log($"람다함수 {x}"); // 파라미터 받기
        //Func<int> cc = () => 10; // 파라미터 x, 항상 10 리턴

        Player player = FindAnyObjectByType<Player>();
        onDie += () => player.AddScore(score); // 죽을때 addScore 함수 실행 등록 
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
   

    // 적이 플레이어에게 점수를 주는 방식을 델리게이트로 처리하도록 변경학
    // 사건이 일어나는 곳ondie(델리게이트 실행),실제로 작용player이 일어나는 곳(델리게이트에 함수를 등록)
    // 점수 숫자가 차례로 올라가도록 설정
}