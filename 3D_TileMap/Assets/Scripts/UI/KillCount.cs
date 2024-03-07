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
        current += Time.deltaTime * countingSpeed;      // current는 target까지 증가
        if(current > target)
        {
            current = target;                           // 넘치는 거 방지
        }
        imageNumber.Number = Mathf.FloorToInt(current);
    }

    private void OnKillcountChange(int count)
    {
        target = count; // 새 킬 카운트를 target으로 지정
    }
}
