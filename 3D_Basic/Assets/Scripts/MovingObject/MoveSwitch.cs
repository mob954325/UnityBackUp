using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// �������� ���� �ݴ� ����ġ
/// </summary>
public class MoveSwitch : MonoBehaviour, IInteracable
{
    /// <summary>
    /// ����ġ�� ����
    /// </summary>
    enum State
    {
        Off = 0,  // ��������
        On // ��������
    }

    /// <summary>
    /// ����ġ ���� ����
    /// </summary>
    State state = State.Off; 

    /// <summary>
    /// target�� �� IInteracable
    /// </summary>
    IInteracable target;

    Animator animator;

    readonly int IsUseOnHash = Animator.StringToHash("IsUse");

    /// <summary>
    /// ���� ��Ÿ��
    /// </summary>
    public float coolTime = 0.5f;

    /// <summary>
    /// ���� �����ִ� ��Ÿ��
    /// </summary>
    float currentCoolTime = 0;

    /// <summary>
    /// ��� ���� ����. ��Ÿ���� 0 �̸��� �� ��밡��
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
            Debug.LogWarning($"{gameObject.name}���� ����� ������Ʈ�� �����ϴ�."); // ���� ������ ���
        }
    }

    void Update()
    {
        currentCoolTime -= Time.deltaTime;
    }


    /// <summary>
    /// ����ġ ���
    /// </summary>
    public void Use()
    {
        if(target != null && CanUse) // ������ ������Ʈ�� �ִ°�
        {
            // �ִϸ��̼Ǹ� ó��
            switch (state)
            {
                case State.Off:
                    // ����ġ�� ���� ��Ȳ
                    animator.SetBool(IsUseOnHash, true); // ����ġ �ִϸ��̼� ���
                    state = State.On; // ����ġ ���� ����
                    break;
                case State.On:
                    // ����ġ�� ������ ��Ȳ
                    animator.SetBool(IsUseOnHash, false);// ����ġ �ִϸ��̼� ���
                    state = State.Off; // ���� ����
                    break;
            }


            target.Use(); // ��� ����ϱ�
        }
    }
}
