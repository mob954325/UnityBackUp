using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundPlanet : MonoBehaviour
{
    public float moveSpeed = 5.0f;

    public float minRight = 30.0f;
    public float maxRightEnd = 60.0f;
    public float minY = -8.0f;
    public float maxY = -5.0f;

    float baseLineX;

    void Start()
    {
        baseLineX = transform.position.x;// 기준선 = 초기위치 x
    }

    void Update()
    {
        // 왼쪽으로 이동
        transform.Translate(Time.deltaTime * moveSpeed * -transform.right);

        if(transform.position.x < baseLineX) // 기준선을 넘으면
        {
            transform.position = new Vector3
                (Random.Range(minRight, maxRightEnd), // 랜덤한 오른쪽
                 Random.Range(minY, maxY), // 랜덤한 y값으로 위치조절
                 0.0f);
        }
    }
}
