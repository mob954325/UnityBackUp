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

    // �÷��̾ Ʈ���� �ȿ� ������ Ŭ����
    void OnTriggerEnter(Collider other)
    {
        // Ŭ��� �Ǹ� ���� ��� ��Ʈ����
        if(other.CompareTag("Player"))
        {

            OnGoalIn();
            // - Ŭ���� ȭ�� ����
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

    // ����
    IEnumerator FireWorkEffect()
    {
        while (true)
        {
            // ps �����ϱ� (�Ǽ� ������ �˰���)
            for(int i = ps.Length -1; i > -1; i--)
            {//5 4 .. 1
                int index = Random.Range(0, i);

                (ps[index], ps[i]) = (ps[i], ps[index]); // �� ���� �����ϱ� (Ʃ�� ��� C#)
            }

            for(int i = 0; i < ps.Length; i++)
            {
                yield return new WaitForSeconds(0.5f);
                ps[i].Play();
            }
        }
    }

    // �� �ȿ� ���� 1�ʰ� ������ ���� Ŭ���� ����
        // GameClear �г��� �� ���� 1���Ŀ� ���.
    // �� ���� ���� 6���� �ѹ��� ���� �� 0,2�� �������� ������ ������.(�������� �ߺ��ؼ� ������ �ȵ�)
}
