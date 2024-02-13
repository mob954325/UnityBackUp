using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapFire : TrapBase
{
    public float duration = 5.0f;
    ParticleSystem ps;

    void Awake()
    {
        Transform child = transform.GetChild(1);
        ps = child.GetComponent<ParticleSystem>();
    }

    protected override void OnTrapActivate(GameObject target)
    {
        ps.Play();

        IAlive live = target.GetComponent<IAlive>();
        if (live != null)
        {
            live.Die();// ���� �� �ִ� ��� ���̱�
        }

        StopAllCoroutines(); // ���� �ڷ�ƾ ����
        StartCoroutine(stopEffect()); // 5�ʵڿ� ����Ʈ�� ������Ű�� �ڷ�ƾ ����
    }

    IEnumerator stopEffect()
    {
        yield return new WaitForSeconds(duration);
        ps.Stop();
    }
}
