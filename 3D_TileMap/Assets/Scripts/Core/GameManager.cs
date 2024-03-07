using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    Player player;
    public Player Player
    {
        get
        {
            if (player == null)
                player = FindAnyObjectByType<Player>();
            return player;
        }
    }

    WorldManager worldManager;

    public WorldManager World => worldManager;

    /// <summary>
    /// �������� �̵���θ� ���ӿ��� ���̰� ���� ���� �����ϴ� ���� (true�� ���̰� , false�� �Ⱥ��δ�.)
    /// </summary>
    public bool showSlimePath = false;

    //11
    // �ν����� â���� showSlimePath�� ����� ������
    // true�� �������� ��ΰ� ���̰� false�� �������� ��ΰ� ������ �ʰ� �����

    void OnValidate()
    {
        // Unity calls when the script is loaded or a value changes in the Inspector.

        Slime[] slimes = FindObjectsOfType<Slime>(true);
        foreach(Slime slime in slimes)
        {
            slime.ShowPath(showSlimePath);
        }
    }

    protected override void OnPreInitialize()
    {
        base.OnPreInitialize();
        worldManager = GetComponent<WorldManager>();
        worldManager.PreInitialize();
    }
    protected override void OnInitialize()
    {
        player = FindAnyObjectByType<Player>();
        worldManager.Initialize();
    }
}
