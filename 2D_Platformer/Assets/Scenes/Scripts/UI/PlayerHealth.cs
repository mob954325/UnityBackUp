using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private GameObject[] _playerHealthObject;
    Player _player;
    int _playerHealthNum;
    int _curDisableHealthNum;


    void Awake()
    {
        _player = FindAnyObjectByType<Player>();    
    }

    void OnEnable()
    {
        _curDisableHealthNum = 0;
        _playerHealthNum = _player._maxHp;
        _playerHealthObject = new GameObject[_playerHealthNum];
        for(int i = 0; i < _playerHealthNum; i++)
        {
            _playerHealthObject[i] = transform.GetChild(i).transform.GetChild(0).gameObject;
        }
    }

    public void ChangeHealth()
    {
        _curDisableHealthNum++;

        for(int i = 1; i <= _curDisableHealthNum; i++)
        {
            _playerHealthObject[_playerHealthNum - i].SetActive(false);
        }
    }
}
