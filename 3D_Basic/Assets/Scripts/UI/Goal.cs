using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public ParticleSystem[] ps;

    void Awake()
    {
        ps = new ParticleSystem[transform.childCount - 2];
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

            OnGoalIn();
            // - 클리어 화면 띄우기
            StartCoroutine(FireWorkEffect());
            StartCoroutine(GameClear());
        }
    }

    private void OnGoalIn()
    {
        for (int i = 0; i < ps.Length; i++)
        {
            ps[i].Play();
        }
    }

    IEnumerator GameClear()
    {
        yield return new WaitForSeconds(1.0f);
        GameManager.Instance.GameClear();
    }

    // 셔플
    IEnumerator FireWorkEffect()
    {
        while (true)
        {
            // ps 셔플하기 (피셔 예이츠 알고리즘)
            for(int i = ps.Length -1; i > -1; i--)
            {//5 4 .. 1
                int index = Random.Range(0, i);

                (ps[index], ps[i]) = (ps[i], ps[index]); // 두 값을 스왑하기 (튜플 방식 C#)
            }

            for(int i = 0; i < ps.Length; i++)
            {
                yield return new WaitForSeconds(0.5f);
                ps[i].Play();
            }
        }
    }

    // 골 안에 들어가고 1초가 지나야 게임 클리어 판정
        // GameClear 패널은 골에 들어가고 1초후에 뜬다.
    // 골에 들어가면 폭죽 6개가 한번에 터진 후 0,2초 간격으로 폭죽이 터진다.(연속으로 중복해서 터지면 안됨)
}
