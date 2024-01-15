using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveEnemy : EnemyBase
{
    //lambda


    /*    float timer = 0;
        public float speed = 1f;
        public float waveTime = 2f;
        public float waveStength = 1.2f;
        float wave;*/
    [Header("Wave info")]
    float spawnY = 0.0f;
    float elapsedTime = 0.0f;

    public float amplitude = 3.0f; // 위 아래로 움직이는 속도
    public float frequency = 2.0f; // 사인그래프가 한번 왕복하는데 걸리는 시간

    public void SetStartPosition(Vector3 position)
    {
        spawnY = position.y;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        spawnY = transform.position.y;
        elapsedTime = 0.0f;

        //Action aa = () => Debug.Log("람다함수");
        //Action<int> bb = (x) => Debug.Log($"람다함수 {x}"); // 파라미터 받기
        //Func<int> cc = () => 10; // 파라미터 x, 항상 10 리턴 

    }

    protected override void OnMoveUpdate(float deltaTime)
    {
        //elapsedTime += Time.deltaTime;
        elapsedTime += deltaTime * frequency; // sin 그래프 진행 빠르게 하기

        //waveSelf();
        transform.position = new Vector3(transform.position.x - deltaTime * moveSpeed,
                                  spawnY + Mathf.Sin(elapsedTime) * amplitude,
                                  0.0f);
    }


    // 적이 플레이어에게 점수를 주는 방식을 델리게이트로 처리하도록 변경학
    // 사건이 일어나는 곳ondie(델리게이트 실행),실제로 작용player이 일어나는 곳(델리게이트에 함수를 등록)
    // 점수 숫자가 차례로 올라가도록 설정
}