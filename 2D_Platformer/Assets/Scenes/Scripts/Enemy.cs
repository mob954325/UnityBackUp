using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum Type { Drone, Turret }
public class Enemy : MonoBehaviour
{
    public Type type;

    [Header("#Enemy Stats")]
    public int _hp = 1;

    [Header("#Effect")]
    public GameObject _deadEffect;




    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("PlayerBullet"))
        {
            _hp--;

            if (_hp < 1)
            {
                Instantiate(_deadEffect, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }

}
