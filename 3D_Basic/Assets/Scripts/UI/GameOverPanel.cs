using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanel : MonoBehaviour
{
    // 게임 패배 되면 보이기 

    RectTransform rect;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    void Start()
    {
        GameManager.Instance.onGameOver += ShowGameOverPanel;
        InitPosition();
    }

    public void ShowGameOverPanel()
    {
        rect.transform.position = new Vector3(0, 0, 0);
    }

    void InitPosition()
    {

        rect.transform.position = new Vector3(0, 1200f, -800f);
    }
}
