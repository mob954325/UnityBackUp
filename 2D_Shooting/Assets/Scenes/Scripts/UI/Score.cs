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
    /// 목표로 하는 최종 점수
    /// </summary>
    int goalScore = 0;

    /// <summary>
    /// 현재 화면에 보여주는 점수
    /// </summary>
    float currentScore = 0.0f;

    /// <summary>
    /// 점수가 올라가는 속도
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
        if(currentScore < goalScore) // 점수가 올라가는 도중
        {

            float speed = Mathf.Max((goalScore - currentScore) * 5.0f, scoreUpSpeed); // 최소 scoreUpSpeed만큼 속도 늘리기

            currentScore += Time.deltaTime * speed;
            currentScore = Mathf.Min(currentScore, goalScore); // float 넘치는거 방지

            int temp = (int)currentScore;
            score.text = $"Score : {temp:D5}";
            //score.text = $"Score : {currentScore:F0}";
        }
    }

    private void RefreshScore(int newScore)
    {
        //score.text = $"Score : {newScore:D5}";// 무조건 점수 5자리 빈자리는 0으로 채움
        //score.text = $"score : {newScore:5}"; // 무조건 점수 5자리 빈자리는 스페이스로 채움

        goalScore = newScore;
    }

}