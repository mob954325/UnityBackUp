using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 모든 문의 부모 클래스
/// </summary>
public class DoorBase : MonoBehaviour
{
    Animator animator;

    readonly int IsOpenHash = Animator.StringToHash("isOpen");

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// 문이 열릴 때 문이 종류별로 따로 처리해야할 일을 기록할 가상함수
    /// </summary>
    protected virtual void OnOpen()
    {

    }

    /// <summary>
    /// 문이 닫힐 때 문이 종류 별로 따로 처리해야할 일을 기록할 가상함수
    /// </summary>
    protected virtual void OnClose()
    {

    }

    /// <summary>
    /// 문은 여는 함수
    /// </summary>
    public void Open()
    {
        animator.SetBool(IsOpenHash, true);
    }

    /// <summary>
    /// 문을 닫는 함수
    /// </summary>
    public void Close()
    {
        animator.SetBool(IsOpenHash, false);
    }
}
