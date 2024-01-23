using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class RankPanel : MonoBehaviour
{
    // 랭킹을 파일로 저장할 수 있어야한다.
    // 랭킹 파일 불러오기
    // 랭킹 업데이트
    // 랭킹 보드 초기화

    /// <summary>
    /// 패털에서 표시되는 랭크 한줄
    /// </summary>
    RankLine[] rankLines;

    /// <summary>
    /// 최고 득점 (1-5등)
    /// </summary>
    int[] highScores;

    /// <summary>
    /// 최고 득점자 이름(1등-5등)
    /// </summary>
    string[] rankerNames;

    /// <summary>
    /// 여기서 표시할 랭크 수
    /// </summary>
    const int rankCount = 5;

    void Awake()
    {
        rankLines = GetComponentsInChildren<RankLine>();
        highScores = new int[rankCount];
        rankerNames = new string[rankCount];
    }

    /// <summary>
    /// 랭킹 데이터를 초기갑승로 모두 설정하는 점수
    /// </summary>
    void SetDefaultData()
    {
        for(int i = 0; i < rankCount; i++)
        {
            char temp = 'A';
            temp = (char)((byte)temp + (byte)i);
            int DefaultName = 65 + i;

            rankerNames[i] = $"{temp}{temp}{temp}";

            int score = 10;

            for(int j = rankCount-i; j > 0; j--)
            {
                score *= 10;
            }
            highScores[i] = score;
        }
        RefreshRankLines();
    }

    /// <summary>
    /// 랭킹데이터를 파일에 저장하는 함수
    /// </summary>
    void SaveRankData()
    {
        //JsonUtility.ToJson();

        SaveData data = new SaveData(); // 저장용 클래스 인스턴스 만들기
        data.rankerName = rankerNames; // 저장용 객체에 데이터 넣기
        data.highScore = highScores; // 저장용 객체의 내용을 json형식의 문자열로 변경

        string jsonText = JsonUtility.ToJson(data); // 문자열반환 후 변수에 넣어줌

        string path = $"{Application.dataPath}/Save/";
        if(!Directory.Exists(path)) // Exists : true면 폴더 있음, false는 없음
        {
            // 폴더가 없다.
            Directory.CreateDirectory(path); // path에 지정된 폴더를 만든다.
        }
        string fullPath = $"{path}Save.json"; // 전체 경로
        System.IO.File.WriteAllText(fullPath, jsonText); // 파일로 저장
        
    }

    void TestSaveData()
    {
        Player player = GameManager.Instance.Player;

        SavePlayerInfo playerData = new SavePlayerInfo();
        playerData.PlayerPosition = player.transform.position;
        playerData.speed = player.moveSpeed;
        playerData.BonusScore = player.powerBonus;

        string jsonText = JsonUtility.ToJson(playerData);

        string path = $"{Application.dataPath}/Save/";
        if (!Directory.Exists(path)) // Exists : true면 폴더 있음, false는 없음
        {
            // 폴더가 없다.
            Directory.CreateDirectory(path); // path에 지정된 폴더를 만든다.
        }

        string fullPath = $"{path}PlayerSave.json";
        System.IO.File.WriteAllText(fullPath, jsonText);
    }

    /// <summary>
    /// 저장된 랭킹 데이터를 불러오는 함수
    /// </summary>
    /// <returns>성공여부(true면 성공, false면 실패)</returns>
    bool LoadRankData()
    {
        bool result = false;
        return result;
    }

    /// <summary>
    /// 랭크 데이터를 업데이트하는 함수
    /// </summary>
    /// <param name="score"></param>
    void UpdateRankData(int score)
    {

    }

    /// <summary>
    /// 현재 설정된 랭킹 데이터에 맞게 UI를 갱신하는 함수
    /// </summary>
    void RefreshRankLines()
    {
        // setdata
        for(int i = 0; i < rankCount; i++)
        {
            rankLines[i].SetData(rankerNames[i], highScores[i]);
        }
    }

    public void Test_RankPanel()
    {
        SetDefaultData();
        RefreshRankLines();
    }
    public void Test_SaveRankPanel()
    {
        SetDefaultData();
        RefreshRankLines();
        SaveRankData();
        TestSaveData();
    }
}
