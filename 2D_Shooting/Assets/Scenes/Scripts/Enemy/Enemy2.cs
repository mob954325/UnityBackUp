using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy2 : EnemyBase
{
    public float rotSpeed = 1.2f;
    public float chargeTime = 2f;

    public GameObject target;

    Vector3 targetPos;
    Vector3 playerPos;

    public bool isCharged = false;

    void Awake()
    {
//player = GetComponent<Player>();
    }

    void Start()
    {
        StartCoroutine(Co_Charge());
        //onDie += () => player.AddScore(score);
    }

    void Update()
    {
        target = FindAnyObjectByType<Player>().gameObject;
        playerPos = target.transform.position;
        RotateToPlayer();
    }

    void RotateToPlayer()
    {
        if(isCharged)
        {
            transform.position += Time.deltaTime * targetPos * moveSpeed;
        }
        else
        {
            Vector3 dir = target.transform.position - transform.position;

            Quaternion rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), rotSpeed * Time.deltaTime);

            rot.x = 0;
            rot.y = 0;

            transform.rotation = rot;
        }
    }

    IEnumerator Co_Charge()
    {
        yield return new WaitForSeconds(chargeTime);
        targetPos = target.transform.position - transform.position; // player Pos

        isCharged = true;

        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        if (isCharged)
            return;
        Gizmos.color = Color.green; // 색지정

        Vector3 enemy = transform.position; // 시작점
        Gizmos.DrawLine(enemy, playerPos);
    }
}
