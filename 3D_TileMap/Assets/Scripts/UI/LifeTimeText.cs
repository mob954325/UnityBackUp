using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LifeTimeText : MonoBehaviour
{
    TextMeshProUGUI timeText;
    float maxLifeTime;

    void Awake()
    {
        timeText = GetComponent<TextMeshProUGUI>();
    }
    void Start()
    {
        Player player = GameManager.Instance.Player;
        maxLifeTime = player.maxLifeTime;

        player.onLifeTimeChange += LifeTimeChange;

        timeText.text = $"{maxLifeTime:f2} Sec";
    }

    void LifeTimeChange(float ratio)
    {
        timeText.text = $"{ratio * maxLifeTime:f2} Sec";
    }
}
