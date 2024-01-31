using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��� ���� �θ� Ŭ����
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
    /// ���� ���� �� ���� �������� ���� ó���ؾ��� ���� ����� �����Լ�
    /// </summary>
    protected virtual void OnOpen()
    {

    }

    /// <summary>
    /// ���� ���� �� ���� ���� ���� ���� ó���ؾ��� ���� ����� �����Լ�
    /// </summary>
    protected virtual void OnClose()
    {

    }

    /// <summary>
    /// ���� ���� �Լ�
    /// </summary>
    public void Open()
    {
        animator.SetBool(IsOpenHash, true);
    }

    /// <summary>
    /// ���� �ݴ� �Լ�
    /// </summary>
    public void Close()
    {
        animator.SetBool(IsOpenHash, false);
    }
}
