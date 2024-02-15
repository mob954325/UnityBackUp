using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClearPanel : MonoBehaviour
{
    // ���� Ŭ���� �Ǹ� ���̱� 

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
       //    canvasGroup.alpha = 1; // ���İ� �÷��� ���̰� �����
       //    canvasGroup.blocksRaycasts = true; // �����ɽ�Ʈ�� �ڱⰡ �ǰ� �ϱ�
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
