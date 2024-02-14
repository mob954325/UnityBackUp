using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClearPanel : MonoBehaviour
{
    // 게임 클리어 되면 보이기 

    RectTransform rect;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    void Start()
    {
        GameManager.Instance.onGameClear += ShowGameClearPanel;
        InitPosition();
    }

    public void ShowGameClearPanel()
    {
        rect.transform.position = new Vector3(0, 0, 0);
    }

    void InitPosition()
    {

        rect.transform.position = new Vector3(0, 1200f, 0);
    }
}
