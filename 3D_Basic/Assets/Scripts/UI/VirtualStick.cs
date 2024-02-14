using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualStick : MonoBehaviour, IDragHandler, IEndDragHandler
{
    RectTransform handleRect;
    RectTransform containerRect;

    float stickRange;

    public Action<Vector2> OnMoveInput;
    void Awake()
    {
        containerRect = GetComponent<RectTransform>();

        Transform child = transform.GetChild(0);
        handleRect = child.GetComponent<RectTransform>();
        // handleRect = child as RectTransform

        stickRange = (containerRect.rect.width - handleRect.rect.width) * 0.5f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // eventData.position 마우스 포인터의 스크린 좌표

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            containerRect,             // 이 영역 안에서
            eventData.position,        // 이 스크린 좌표가
            eventData.pressEventCamera, // 기준이 되는 카메라
            out Vector2 position // 로컬로 얼마나 움직이는지 position으로 리턴
            );

        position = Vector2.ClampMagnitude(position, stickRange);

        InputUpdate(position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // IBeginHandler, IEndDragHandler를 사용하려면 IDragHandler 필수
        InputUpdate(Vector2.zero);
    }

    private void InputUpdate(Vector2 inputDelta)
    {
        handleRect.anchoredPosition = inputDelta;
        OnMoveInput.Invoke(inputDelta/stickRange); // -1,-1 - 1,1로 변환해서 보낸다
        //Debug.Log(inputDelta / stickRange);
    }

    public void Stop()
    {
        OnMoveInput = null;
    }
}
