using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillCount : MonoBehaviour
{
    public float countingSpeed = 1.0f;

    float target = 0.0f;
    float current = 0.0f;

    ImageNumber imageNumber;

    void Awake()
    {
        imageNumber = GetComponent<ImageNumber>();
    }

    void Start()
    {
        Player player = GameManager.Instance.Player;
        player.onKillCountChange += OnKillcountChange;
    }

    void Update()
    {
        current += Time.deltaTime * countingSpeed;      // current�� target���� ����
        if(current > target)
        {
            current = target;                           // ��ġ�� �� ����
        }
        imageNumber.Number = Mathf.FloorToInt(current);
    }

    private void OnKillcountChange(int count)
    {
        target = count; // �� ų ī��Ʈ�� target���� ����
    }
}
