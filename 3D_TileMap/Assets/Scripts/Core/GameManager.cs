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
    /// 슬라임의 이동경로를 게임에서 보이게 할지 말지 결정하는 변수 (true면 보이고 , false면 안보인다.)
    /// </summary>
    public bool showSlimePath = false;

    //11
    // 인스펙터 창에서 showSlimePath가 변경될 때마다
    // true면 슬라임의 경로가 보이고 false면 슬라임의 경로가 보이지 않게 만들기

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
