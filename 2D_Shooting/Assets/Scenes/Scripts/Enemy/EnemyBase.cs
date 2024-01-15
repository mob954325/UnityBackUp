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
    /// 최대 hp
    /// </summary>
    public int MaxHp = 1;

    /// <summary>
    /// 플레이어가 이 적을 죽였을 때 얻는 점수
    /// </summary>
    public int score = 10;

    public float moveSpeed; // 이동속도

    [Header("# obj")]
    public GameObject explosionEffect; // explosionEffect

    /// <summary>
    /// 점수를 줄 플레이어
    /// </summary>
    public Player player;

    /// <summary>
    /// 적이 죽을 때 실행될 델리게이트
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
            onDie -= PlayerAddScore; // 순차적으로 제거
            onDie = null; // 확실하게 정리한다고 표시 , null만 써도 무방
            player = null;
        }

        base.OnDisable();
    }
    
    /// <summary>
    /// 플레이어 점수 추가용 함수(델리게이트 등록용)
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
        if (collision.gameObject.CompareTag("Bullet") // 총알 또는 플레이어가 부딪치면 1 감소
         || collision.gameObject.CompareTag("Player"))
        {
            Hp--;
        }
    }

    /// <summary>
    /// Enemy 계열의 초기화 함수
    /// </summary>
    protected virtual void onInitialze()
    {
        if(player = null)
        {
            player = GameManager.Instance.Player; // 플레이어 찾기
        }
        if (player != null)
        {
            onDie += PlayerAddScore; // 플레이어 점수 증가 함수 등록
        }

        Hp = MaxHp; // Hp 최대로 등록
    }

    /// <summary>
    /// 업데이트 중에 이동처리하는 함수
    /// </summary>
    /// <param name="deltaTime">프레임간의 간격</param>
    protected virtual void OnMoveUpdate(float deltaTime)
    {
        transform.Translate(deltaTime * moveSpeed * -transform.right, Space.World);// 기본동장 : 왼쪽으로 이동
    }

    /// <summary>
    /// 사망처리용 함수
    /// </summary>
    protected virtual void OnDie()
    {
        // 터지는 이펙트 생성
        Factory.Instance.GetExplosion(transform.position);

        onDie?.Invoke(); // 죽었다는 신호 보내기

        gameObject.SetActive(false);
    }
   
}