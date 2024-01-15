using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Background : MonoBehaviour
{
    /// <summary>
    /// 이동속도
    /// </summary>
    public float scrollingSpeed = 2.5f;
    /// <summary>
    /// 가로 그림 길이
    /// </summary>
    const float Backgroundwidth = 13.0f;

    /// <summary>
    /// 대상
    /// </summary>
    public Transform[] bgSlots;

    /// <summary>
    /// 끝으로 보내는 기준
    /// </summary>
    float baseLineX;

    protected virtual void Awake()
    {
        bgSlots = new Transform[transform.childCount]; // 배열만들고

        for(int i = 0; i < bgSlots.Length; i++) 
        {
            bgSlots[i] = transform.GetChild(i); // 자식 넣기
        }

        baseLineX = transform.position.x - Backgroundwidth; // 기준 생성
    }

    void Update()
    {
        for(int i = 0; i < bgSlots.Length; i++)
        {
            bgSlots[i].Translate(Time.deltaTime * scrollingSpeed * -transform.right); // 스크롤링

            if (bgSlots[i].position.x < baseLineX)
            {
                MoveRight(i); // 기준에 도달하면 오른쪽으로 보내기
            }
        }




        //transform.position += Time.deltaTime * scrollingSpeed * Vector3.left;
        //if (transform.position.x < -23f) // -23 -10 3
        //    transform.position = new Vector3(transform.position.x + (Backgroundwidth * 3),
        //                                     transform.position.y);
    }

    protected virtual void MoveRight(int index)
    {
        bgSlots[index].Translate(Backgroundwidth * bgSlots.Length * transform.right);
    }
}
