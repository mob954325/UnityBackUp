using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float _speed = 1.2f;


    public GameObject _hitEffect;
    Rigidbody2D _rigid;

    void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _rigid.MovePosition(transform.position + Vector3.right * _speed * Time.deltaTime);
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
