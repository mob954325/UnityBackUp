using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Bullet : RecycleObject
{
    // 시작하자마자 계속 오른쪽으로 초속 7로 움직이게 만들기

    public float speed = 7.0f;
    public float waveStength = 1.2f;
    float waveTime = 0;

    /// <summary>
    /// 총알 수명
    /// </summary>
    public float lifeTime = 10.0f;

    /// <summary>
    /// effect
    /// </summary>
    public GameObject effectPrefab;

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(LifeOver(lifeTime));
    }

    void Update()
    {
        waveTime += Time.deltaTime;

        if (waveTime > 0.5f) 
        {
            waveTime = 0;

            waveStength *= -1f;
        }

        transform.Translate(Time.deltaTime * speed * Vector2.right);

        //transform.Translate(Time.deltaTime * speed * Vector2.right); 스칼라 * 벡터 -> 계산 횟수 3
        //transform.Translate(Vector2.right * Time.deltaTime * speed); 벡터 * 스칼라 -> 계산 횟수 4
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Factory.Instance.GetHitEffect(transform.position);

        //Destroy(gameObject);
        gameObject.SetActive(false);
    }
}
