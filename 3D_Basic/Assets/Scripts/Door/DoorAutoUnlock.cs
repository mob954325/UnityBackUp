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
                    // 잠그기
                    doorMaterial.color = lockColor;
                    sensor.enabled = false;
                }
                else
                {
                    // 잠금 해제하기
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
        door = door.GetChild(0); // 문 찾기

        MeshRenderer meshRenderer = door.GetComponent<MeshRenderer>(); // 문의 랜더러 찾기
        doorMaterial = meshRenderer.material; // 랜더러에서 머터리얼 갖오기
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
