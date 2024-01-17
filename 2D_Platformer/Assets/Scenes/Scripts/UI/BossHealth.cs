using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    Action _bossInfo;
    Scrollbar scrollbar;
    Enemy_Boss _boss;

    float _curHealth;
    float _maxHealth;
    string _bossName;

    void Awake()
    {
        scrollbar = GetComponent<Scrollbar>();
        _boss = FindAnyObjectByType<Enemy_Boss>();
    }

    void Start()
    {
            _bossInfo += () => _boss.meetPlayer();
    }

    void LateUpdate()
    {
    }
}
