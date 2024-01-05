using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float _speed = 1.2f;
    bool _isFlip = false;

    public GameObject _hitEffect;
    public Player _player;
    SpriteRenderer _sprite;

    void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();    
    }
    void Start()
    {
        _isFlip = _player._isFlipX;
    }

    void Update()
    {
        if(_isFlip)
        {
            transform.position += Vector3.left * _speed * Time.deltaTime;
            _sprite.flipX = true;
        }
        else if(!_isFlip)
        {
            transform.position += Vector3.right * _speed * Time.deltaTime;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            Instantiate(_hitEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
