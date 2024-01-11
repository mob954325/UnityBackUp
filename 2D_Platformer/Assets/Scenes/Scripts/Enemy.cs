using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum Type { Drone, Turret }
public class Enemy : MonoBehaviour
{
    public Type type;

    Action _onDie;

    [Header("#Enemy Stats")]
    [SerializeField]private int _hp = 1;
    [SerializeField]private int _score = 10;
    public int _Hp
    {
        get => _hp;
        set 
        { 
            _hp = value; 
            if(_hp <= 0)
            {
                _hp = 0;
                OnDie();
            }
        }
    }

    [Header("#Effect")]
    public GameObject _deadEffect;

    protected virtual void Start()
    {
        Player player = FindAnyObjectByType<Player>();
        _onDie += () => player.AddScore(_score);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("PlayerBullet"))
        {
            _Hp--;
        }
    }

    void OnDie()
    {
        Instantiate(_deadEffect, transform.position, Quaternion.identity);
        _onDie?.Invoke();
        Destroy(gameObject);

    }

}
