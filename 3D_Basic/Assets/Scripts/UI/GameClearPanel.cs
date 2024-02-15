using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClearPanel : MonoBehaviour
{
    // 게임 클리어 되면 보이기 

    RectTransform rect;

    //CanvasGroup canvasGroup;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        //canvasGroup = GetComponent<CanvasGroup>();
    }

    void Start()
    {
        GameManager.Instance.onGameClear += ShowGameClearPanel;
       //GameManager.Instance.onGameClear += () =>
       //{
       //    canvasGroup.alpha = 1; // 알파값 올려서 보이게 만들기
       //    canvasGroup.blocksRaycasts = true; // 레이케스트를 자기가 되게 하기
       //};
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
