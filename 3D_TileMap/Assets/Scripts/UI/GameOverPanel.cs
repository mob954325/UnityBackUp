using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
{
    CanvasGroup canvasGroup;
    public float alphaChangeSpeed = 1.0f;

    TextMeshProUGUI playTime;
    TextMeshProUGUI killCount;

    Button restart;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        Transform child = transform.GetChild(1);
        playTime = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(2);
        killCount = child.gameObject.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(3);
        restart = child.GetComponent<Button>();
    }

    private void Start()
    {
        restart.onClick.AddListener( () =>
        {
            Debug.Log("��ư ����");
            StopCoroutine(WaitUnloadAll());
            StartCoroutine(WaitUnloadAll());
        });          // restart��ư�� �������� AddListener�� ����� �Լ��� ����ȴ�.

        canvasGroup.alpha = 0.0f;
        canvasGroup.interactable = false;   
        canvasGroup.blocksRaycasts = false;

        Player player = GameManager.Instance.Player;
        player.onDie += OnPlayerDie;
    }

    private void OnPlayerDie(float totalPlayTime, int totalKillCount)
    {
        playTime.text = $"Total Play Time \n\r < {totalPlayTime:f1} Sec >";
        killCount.text = $"Total Kill Count \n\r < {totalKillCount} >";
        StartCoroutine(StartAlphaChange());
    }

    IEnumerator StartAlphaChange()
    {
        while(canvasGroup.alpha < 1.0f)
        {
            canvasGroup.alpha += Time.deltaTime * alphaChangeSpeed;
            yield return null;
        }

        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    IEnumerator WaitUnloadAll()
    {
        WorldManager world = GameManager.Instance.World;
        while(!world.IsUnloadAll)                           // �÷��̾ �׾��� �� ��� ���� ��¡���� ��û�� �ϴϱ� �� �ɶ����� ���
        {
            yield return null;
        }
        SceneManager.LoadScene("ASyncLoadScene");
    }
}