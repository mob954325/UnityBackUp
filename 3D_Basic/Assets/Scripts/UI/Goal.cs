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

    // �÷��̾ Ʈ���� �ȿ� ������ Ŭ����
    void OnTriggerEnter(Collider other)
    {
        // Ŭ��� �Ǹ� ���� ��� ��Ʈ����
        if(other.CompareTag("Player"))
        {
            for(int i = 0; i < ps.Length; i++)
            {
                ps[i].Play();
            }

            // - Ŭ���� ȭ�� ����
            GameManager.Instance.GameClear();
        }
    }

    // Ŭ����� 
    // Ŭ��� �Ǹ� �÷��̾�� ����, �Է� ����

    // ���� ���� �� ���� ���� ȭ�� ����
}
