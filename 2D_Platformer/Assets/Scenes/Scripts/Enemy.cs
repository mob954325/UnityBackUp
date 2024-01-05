using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    public enum Type { Drone, Turret }

    public GameObject _deadEffect;

    [Header("#Enemy Stats")]
    public int hp = 1;
    public float _distance; // CircleCast's distance
    public float _size; // CircleCast's radius

    void Update()
    {
        RaycastHit2D hit = Physics2D.Linecast(transform.position, transform.position + Vector3.right * _distance);

        Debug.DrawRay(transform.position, Vector2.right * _distance, Color.red);

        if(hit.collider != null && hit.collider.gameObject.CompareTag("Player"))
        {
            Debug.Log($"hit Raycast : {hit.collider.gameObject.name}");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Bullet"))
        {
            hp--;

            if (hp < 1)
            {
                Instantiate(_deadEffect, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }

/*    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _distance);
        //Gizmos.color = Color.blue;
        //Gizmos.DrawLine(shootVector, shootVector + (Vector3.right * transform.localScale.x * rangeAttack));
    }*/
}
