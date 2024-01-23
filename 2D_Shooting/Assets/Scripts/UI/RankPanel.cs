using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class RankPanel : MonoBehaviour
{
    // ��ŷ�� ���Ϸ� ������ �� �־���Ѵ�.
    // ��ŷ ���� �ҷ�����
    // ��ŷ ������Ʈ
    // ��ŷ ���� �ʱ�ȭ

    /// <summary>
    /// ���п��� ǥ�õǴ� ��ũ ����
    /// </summary>
    RankLine[] rankLines;

    /// <summary>
    /// �ְ� ���� (1-5��)
    /// </summary>
    int[] highScores;

    /// <summary>
    /// �ְ� ������ �̸�(1��-5��)
    /// </summary>
    string[] rankerNames;

    /// <summary>
    /// ���⼭ ǥ���� ��ũ ��
    /// </summary>
    const int rankCount = 5;

    void Awake()
    {
        rankLines = GetComponentsInChildren<RankLine>();
        highScores = new int[rankCount];
        rankerNames = new string[rankCount];
    }

    /// <summary>
    /// ��ŷ �����͸� �ʱⰩ�·� ��� �����ϴ� ����
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
    /// ��ŷ�����͸� ���Ͽ� �����ϴ� �Լ�
    /// </summary>
    void SaveRankData()
    {
        //JsonUtility.ToJson();

        SaveData data = new SaveData(); // ����� Ŭ���� �ν��Ͻ� �����
        data.rankerName = rankerNames; // ����� ��ü�� ������ �ֱ�
        data.highScore = highScores; // ����� ��ü�� ������ json������ ���ڿ��� ����

        string jsonText = JsonUtility.ToJson(data); // ���ڿ���ȯ �� ������ �־���

        string path = $"{Application.dataPath}/Save/";
        if(!Directory.Exists(path)) // Exists : true�� ���� ����, false�� ����
        {
            // ������ ����.
            Directory.CreateDirectory(path); // path�� ������ ������ �����.
        }
        string fullPath = $"{path}Save.json"; // ��ü ���
        System.IO.File.WriteAllText(fullPath, jsonText); // ���Ϸ� ����
        
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
        if (!Directory.Exists(path)) // Exists : true�� ���� ����, false�� ����
        {
            // ������ ����.
            Directory.CreateDirectory(path); // path�� ������ ������ �����.
        }

        string fullPath = $"{path}PlayerSave.json";
        System.IO.File.WriteAllText(fullPath, jsonText);
    }

    /// <summary>
    /// ����� ��ŷ �����͸� �ҷ����� �Լ�
    /// </summary>
    /// <returns>��������(true�� ����, false�� ����)</returns>
    bool LoadRankData()
    {
        bool result = false;
        return result;
    }

    /// <summary>
    /// ��ũ �����͸� ������Ʈ�ϴ� �Լ�
    /// </summary>
    /// <param name="score"></param>
    void UpdateRankData(int score)
    {

    }

    /// <summary>
    /// ���� ������ ��ŷ �����Ϳ� �°� UI�� �����ϴ� �Լ�
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
