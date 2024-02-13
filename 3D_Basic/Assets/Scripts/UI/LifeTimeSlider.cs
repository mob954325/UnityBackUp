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
    /// ���� ���� ���ϱ� ���� ���� �ִ� ��
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
        maxValue = player.startLifeTime;
    }

    void Refresh(float ratio)
    {
        slider.value = ratio;
        tmp.text = $"{(ratio*maxValue):f1} Sec"; // ������ �ִ� ���� ���ؼ� ���� ������ ����
    }
    // �÷��̾��� ������ �����̴��� ǥ���ϱ�
    // �÷��̾��� ���� ������ text�� �Ҽ��� ���ڸ����� ǥ���ϱ�
}
