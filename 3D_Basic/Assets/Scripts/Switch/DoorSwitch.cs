using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// �������� ���� �ݴ� ����ġ
/// </summary>
public class DoorSwitch : MonoBehaviour, IInteracable
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
    /// target�� ������ �մ� DoorManual
    /// </summary>
    public DoorManual targetDoor;

    Animator animator;

    readonly int switchOnHash = Animator.StringToHash("SwitchOn");

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
        if(targetDoor == null)
        {
            Debug.LogWarning($"{gameObject.name}���� ����� ���� �����ϴ�."); // ���� ������ ���
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
        if(targetDoor != null && CanUse) // ������ ���� �ִ°�
        {
            switch (state)
            {
                case State.Off:
                    // ����ġ�� ���� ��Ȳ
                    targetDoor.Open(); // �� ����
                    animator.SetBool(switchOnHash, true); // ����ġ �ִϸ��̼� ���
                    state = State.On; // ����ġ ���� ����
                    break;
                case State.On:
                    // ����ġ�� ������ ��Ȳ
                    targetDoor.Close(); // �� �ݱ�
                    animator.SetBool(switchOnHash, false);// ����ġ �ִϸ��̼� ���
                    state = State.Off; // ���� ����
                    break;
            }
        }
    }
}
