using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class asteroidMove : MonoBehaviour
{
    public GameObject asteroidObj;

    public int hp = 5;
    public float speed = 0.8f;
    public float degrees = 1.2f;

    void Update()
    {
        transform.Translate(Time.deltaTime * speed * Vector2.left);

        asteroidObj.transform.Rotate(Vector3.forward * degrees);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            hp--;
            if (hp < 1)
            {
                // Instantiate(explosionEffect, transform.position, Quaternion.identity); -> 나중에 운석 조각 넣음
                Destroy(gameObject);
            }

        }
    }
}
 