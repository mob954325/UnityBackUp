using System.Collections;
using TMPro;
using UnityEngine;

public class DoorManualAutoClose : DoorManual
{
    public float CloseToTime = 3.0f;

    protected override void Awake()
    {
        base.Awake();
        showKey = GetComponentInChildren<TextMeshPro>(true);
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            showKey.gameObject.SetActive(false);
        }
    }

    public new void Use()
    {
        if(!isOpen)
        {
            isOpen = true;
            Open();
            StopAllCoroutines();
            StartCoroutine(DoorClose()); // 자동으로 문 닫음
        }
        else if(isOpen)
        {
            StopAllCoroutines();
            Close(); // 문을 바로 닫음
            isOpen = false;
        }
    }

    /// <summary>
    /// CloseToTime 시간이 지나고 문 닫는 애니메이션 실행
    /// </summary>
    /// <returns></returns>
    IEnumerator DoorClose()
    {
        yield return new WaitForSeconds(CloseToTime);
        Close();
        isOpen = false;
    }
}


// 실습
// 1. 일정 시간 이후에 자동으로 문이 닫힘
// 2. 플레이어가 자신의 트리거 안에 들어오면 글자로 단축키 보여주기