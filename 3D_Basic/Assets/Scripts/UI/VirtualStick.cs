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
        // eventData.position ���콺 �������� ��ũ�� ��ǥ

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            containerRect,             // �� ���� �ȿ���
            eventData.position,        // �� ��ũ�� ��ǥ��
            eventData.pressEventCamera, // ������ �Ǵ� ī�޶�
            out Vector2 position // ���÷� �󸶳� �����̴��� position���� ����
            );

        position = Vector2.ClampMagnitude(position, stickRange);

        InputUpdate(position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // IBeginHandler, IEndDragHandler�� ����Ϸ��� IDragHandler �ʼ�
        InputUpdate(Vector2.zero);
    }

    private void InputUpdate(Vector2 inputDelta)
    {
        handleRect.anchoredPosition = inputDelta;
        OnMoveInput.Invoke(inputDelta/stickRange); // -1,-1 - 1,1�� ��ȯ�ؼ� ������
        //Debug.Log(inputDelta / stickRange);
    }

    public void Stop()
    {
        OnMoveInput = null;
    }
}
