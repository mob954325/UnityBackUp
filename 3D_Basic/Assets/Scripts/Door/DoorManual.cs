using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DoorManual : DoorBase, IInteracable
{
    public TextMeshPro showKey; // 3d ����

    /// <summary>
    /// ���� �����ִ� ����(true�� ���� �����ִ�, false�� ���� �����ִ�.)
    /// </summary>
    protected bool isOpen = false;

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

    void Update()
    {
        currentCoolTime -= Time.deltaTime;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Vector3 cameraToDoor = transform.position - Camera.main.transform.position; // �÷��̾�� ������ ���ϴ� ���⺤��

            float angle = Vector3.Angle(transform.forward, cameraToDoor); // ����

            if (angle > 90.0f) // �þ߰��� 90�� ���� ũ�� ī�޶� �� �տ� �ִ�.
            {
                showKey.transform.rotation = transform.rotation * Quaternion.Euler(0, 180f, 0);
            }
            else
            {
                showKey.transform.rotation = transform.rotation;
            }


            showKey.gameObject.SetActive(true);
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            showKey.gameObject.SetActive(false);
        }
    }

    public void Use()
    {
        if(CanUse) // ��� ������ ���� ���
        {
            if (!isOpen)
            {
                Open();
                isOpen = true;
            }
            else
            {
                Close();
                isOpen = false;
            }
            currentCoolTime = coolTime; // ��Ÿ�� �ʱ�ȭ
        }
    }
}
