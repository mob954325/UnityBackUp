using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEffect : RecycleObject
{
    Animator anim;
    float animLength = 0.0f;

    void Awake()
    {
        anim = GetComponent<Animator>();
        // .GetCurrentAnimatorClipInfo(0) : �ִϸ������� ù��° ���̾��� Ŭ�� ���� �޾ƿ���
        // GetCurrentAnimatorClipInfo(0)[0] : �ִϸ������� ù��° ���̾ �ִ� ���ϸ��̼� Ŭ�� �� ù��° Ŭ������ �޾ƿ���
        animLength = anim.GetCurrentAnimatorClipInfo(0)[0].clip.length; // �ִϸ��̼� ���� ����
    }

    void Start()
    {
        //StartCoroutine(Co_DestoryObj());

        //Destroy(this.gameObject, animLength);
    }

    //IEnumerator Co_DestoryObj()
    //{
    //    yield return new WaitForSeconds(0.2f);
    //    Destroy(gameObject);
    //}

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(LifeOver(animLength));
    }
}
