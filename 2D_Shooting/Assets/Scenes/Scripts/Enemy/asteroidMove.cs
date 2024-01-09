using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class asteroidMove : MonoBehaviour
{
    public bool isSmall = false;
    public GameObject asteroidObj = null;
    public GameObject smallAsteroidObj = null;

    public int hp = 5;
    public float speed = 0.8f;
    public float degrees = 1.2f;

    void Awake()
    {
        asteroidObj = transform.GetChild(0).gameObject;
    }

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
                MakeSmall();
                Destroy(gameObject);
            }

        }
    }

    void MakeSmall()
    {
        if (isSmall)
            return;

        GameObject smallParent = new GameObject();
        smallParent.transform.position = transform.position;

        asteroidMove _asteroidMove = smallParent.AddComponent<asteroidMove>();
        _asteroidMove.isSmall = true;

        GameObject smallObj = Instantiate(smallAsteroidObj, transform.position, Quaternion.identity, smallParent.transform);
    }
}
 