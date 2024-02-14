using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LifeTimeSlider : MonoBehaviour
{
    Player player;

    Slider slider;
    TextMeshProUGUI tmp;

    /// <summary>
    /// 원래 값을 구하기 위해 사용될 최대 값
    /// </summary>
    float maxValue = 1.0f;

    void Awake()
    {
        slider = GetComponent<Slider>();
        tmp = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Start()
    {
        player = GameManager.Instance.Player;
        player.onLifeTimeChange += Refresh;
        player.onDie += Stop;
        maxValue = player.startLifeTime;
    }

    private void Stop()
    {
        player = GameManager.Instance.Player;
        player.onLifeTimeChange -= Refresh;
        player.onDie -= Stop;
    }

    void Refresh(float ratio)
    {
        slider.value = ratio;
        tmp.text = $"{(ratio*maxValue):f1} Sec"; // 비율에 최대 값을 곱해서 원래 값으로 변경
    }
    // 플레이어의 수명을 슬라이더로 표현하기
    // 플레이어의 남은 수명을 text로 소숫점 한자리까지 표현하기
}
