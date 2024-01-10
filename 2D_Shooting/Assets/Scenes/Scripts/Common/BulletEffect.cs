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
        // .GetCurrentAnimatorClipInfo(0) : 애니메이터의 첫번째 레이어의 클립 정보 받아오기
        // GetCurrentAnimatorClipInfo(0)[0] : 애니메이터의 첫번째 레이어에 있는 에니메이션 클립 중 첫번째 클립정보 받아오기
        animLength = anim.GetCurrentAnimatorClipInfo(0)[0].clip.length; // 애니메이션 길이 추출
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
