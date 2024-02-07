using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAutoUnlock : DoorAuto
{
    bool locking = true;
    bool Locking
    {
        get => locking;
        set
        { 
            if(locking != value)
            {
                locking = value;
                if(locking)
                {
                    // ��ױ�
                    doorMaterial.color = lockColor;
                    sensor.enabled = false;
                }
                else
                {
                    // ��� �����ϱ�
                    doorMaterial.color = unLockColor;
                    sensor.enabled = true;
                }
            }
        }
    }

    BoxCollider sensor;

    public Color lockColor;
    public Color unLockColor;

    Material doorMaterial;

    protected override void Awake()
    {
        base.Awake();
        sensor = GetComponent<BoxCollider>(); // door Sensor

        Transform door = transform.GetChild(1);
        door = door.GetChild(0); // �� ã��

        MeshRenderer meshRenderer = door.GetComponent<MeshRenderer>(); // ���� ������ ã��
        doorMaterial = meshRenderer.material; // ���������� ���͸��� ������
    }

    protected override void Start()
    {
        Locking = true;
    }

    protected override void OnKeyUsed()
    {
        Locking = false;
    }
}
