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

    //void Start()
    //{
    //    // Destroy(gameObject, lifeTime); // lifeTime이후 스스로 삭제
    //}

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(LifeOver(lifeTime));
    }

    void Update()
    {
        waveTime += Time.deltaTime;

        //transform.position += Vector3.right * Time.deltaTime * speed;

        if (waveTime > 0.5f) 
        {
            waveTime = 0;

            waveStength *= -1f;
        }

        transform.position += new Vector3(speed * Time.deltaTime, waveStength * Time.deltaTime);

        //transform.Translate(Time.deltaTime * speed * Vector2.right); 스칼라 * 벡터 -> 계산 횟수 3
        //transform.Translate(Vector2.right * Time.deltaTime * speed); 벡터 * 스칼라 -> 계산 횟수 4
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

/*        if (collision.gameObject.CompareTag("Player") ||
            collision.gameObject.CompareTag("Bullet"))
            return;*/

        Instantiate(effectPrefab, transform.position, Quaternion.identity); // hit 이펙트 생성

        //Destroy(gameObject);
        gameObject.SetActive(false);
    }


    //1. Bullet 프리펩에 필요한 컴포넌트 추가하고 설정하기
    //2. 총알은 "Enemy" 태그를 가진 오브젝트와 부딪치면 부딪친 대상을 삭제한다.
    //3. 총알은 다른 오브젝트와 부딪치면 자기 자신을 삭제한다.

    //4. Hit 스프라이트를 이용해 HItEffect라는 프리펩을 만들기
    //5. 총알이 부딪친 위치에 HitEffect를 생성한다.
    //6. HitEffect는 한번만 재생되고 사라진다. -> 게임 오브젝트가 제거되야함
}
