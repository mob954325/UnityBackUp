using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    TextMeshProUGUI score;
    Player player;

    /// <summary>
    /// ��ǥ�� �ϴ� ���� ����
    /// </summary>
    int goalScore = 0;

    /// <summary>
    /// ���� ȭ�鿡 �����ִ� ����
    /// </summary>
    float currentScore = 0.0f;

    /// <summary>
    /// ������ �ö󰡴� �ӵ�
    /// </summary>
    public float scoreUpSpeed = 50.0f;

    void Awake()
    {
        score = GetComponent<TextMeshProUGUI>();   
        player = FindAnyObjectByType<Player>();
    }
    void Start()
    {
        player.onScoreChange += RefreshScore;

        goalScore = 0;
        currentScore = 0.0f;
        score.text = "Score : 00000";
    }

    void LateUpdate()
    {
        if(currentScore < goalScore) // ������ �ö󰡴� ����
        {

            float speed = Mathf.Max((goalScore - currentScore) * 5.0f, scoreUpSpeed); // �ּ� scoreUpSpeed��ŭ �ӵ� �ø���

            currentScore += Time.deltaTime * speed;
            currentScore = Mathf.Min(currentScore, goalScore); // float ��ġ�°� ����

            int temp = (int)currentScore;
            score.text = $"Score : {temp:D5}";
            //score.text = $"Score : {currentScore:F0}";
        }
    }

    private void RefreshScore(int newScore)
    {
        //score.text = $"Score : {newScore:D5}";// ������ ���� 5�ڸ� ���ڸ��� 0���� ä��
        //score.text = $"score : {newScore:5}"; // ������ ���� 5�ڸ� ���ڸ��� �����̽��� ä��

        goalScore = newScore;
    }

}