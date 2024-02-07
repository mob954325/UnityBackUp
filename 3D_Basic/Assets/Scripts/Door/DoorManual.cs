using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DoorManual : DoorBase, IInteracable
{
    public TextMeshPro showKey; // 3d 글자

    /// <summary>
    /// 문이 열려있는 상태(true면 문이 열려있다, false면 문이 닫혀있다.)
    /// </summary>
    protected bool isOpen = false;

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

    void Update()
    {
        currentCoolTime -= Time.deltaTime;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Vector3 cameraToDoor = transform.position - Camera.main.transform.position; // 플레이어에서 문으로 향하는 방향벡터

            float angle = Vector3.Angle(transform.forward, cameraToDoor); // 각도

            if (angle > 90.0f) // 시야각이 90도 보다 크면 카메라가 문 앞에 있다.
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
        if(CanUse) // 사용 가능할 때만 사용
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
            currentCoolTime = coolTime; // 쿨타임 초기화
        }
    }
}
