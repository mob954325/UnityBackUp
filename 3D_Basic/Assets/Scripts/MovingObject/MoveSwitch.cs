using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// 수동문을 열고 닫는 수위치
/// </summary>
public class MoveSwitch : MonoBehaviour, IInteracable
{
    /// <summary>
    /// 스위치의 상태
    /// </summary>
    enum State
    {
        Off = 0,  // 꺼진상태
        On // 켜진상태
    }

    /// <summary>
    /// 스위치 현재 상태
    /// </summary>
    State state = State.Off; 

    /// <summary>
    /// target이 될 IInteracable
    /// </summary>
    IInteracable target;

    Animator animator;

    readonly int IsUseOnHash = Animator.StringToHash("IsUse");

    /// <summary>
    /// 재사용 쿨타임
    /// </summary>
    public float coolTime = 0.5f;

    /// <summary>
    /// 현재 남아있는 쿨타임
    /// </summary>
    float currentCoolTime = 0;

    /// <summary>
    /// 사용 가능 여부. 쿨타임이 0 미만일 때 사용가능
    /// </summary>
    public bool CanUse => currentCoolTime < 0.0f;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        target = transform.parent.GetComponent<IInteracable>();
        if(target == null)
        {
            Debug.LogWarning($"{gameObject.name}에게 사용할 오브젝트가 없습니다."); // 문이 없으면 경고
        }
    }

    void Update()
    {
        currentCoolTime -= Time.deltaTime;
    }


    /// <summary>
    /// 스위치 사용
    /// </summary>
    public void Use()
    {
        if(target != null && CanUse) // 조작할 오브젝트가 있는가
        {
            // 애니메이션만 처리
            switch (state)
            {
                case State.Off:
                    // 스위치를 누른 상황
                    animator.SetBool(IsUseOnHash, true); // 스위치 애니메이션 재생
                    state = State.On; // 스위치 상태 변경
                    break;
                case State.On:
                    // 스위치를 끄려는 상황
                    animator.SetBool(IsUseOnHash, false);// 스위치 애니메이션 재생
                    state = State.Off; // 상태 변경
                    break;
            }


            target.Use(); // 대상 사용하기
        }
    }
}
