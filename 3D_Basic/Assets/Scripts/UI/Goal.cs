using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public ParticleSystem[] ps;

    void Awake()
    {
        ps = new ParticleSystem[3];
        for(int i = 0; i < ps.Length; i++)
        {
            ps[i] = transform.GetChild(i).GetComponent<ParticleSystem>();
        }
    }

    // 플레이어가 트리거 안에 들어오면 클리어
    void OnTriggerEnter(Collider other)
    {
        // 클리어가 되면 폭주 모두 터트리기
        if(other.CompareTag("Player"))
        {
            for(int i = 0; i < ps.Length; i++)
            {
                ps[i].Play();
            }

            // - 클리어 화면 띄우기
            GameManager.Instance.GameClear();
        }
    }

    // 클리어시 
    // 클리어가 되면 플레이어는 수명, 입력 정지

    // 게임 오버 시 게임 오버 화면 띄우기
}
