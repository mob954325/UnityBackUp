using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : RecycleObject
{
    public float initialSpeed = 20.0f;
    public float lifeTime = 10.0f;

    Rigidbody rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void Start()
    {
        rigid.velocity = initialSpeed * transform.forward;
        StartCoroutine(LifeTime());
    }

    void Update()
    {
    }

    IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(lifeTime);
        gameObject.SetActive(false);
    }
}
