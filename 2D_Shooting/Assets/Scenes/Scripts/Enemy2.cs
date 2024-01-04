using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy2 : MonoBehaviour
{
    public float speed;
    public float rotSpeed = 1.2f;
    public float chargeTime = 2f;

    public GameObject target;
    Rigidbody2D rigid;

    Vector3 targetPos;

    public bool isCharged = false;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        StartCoroutine(Co_Charge());
    }

    void Update()
    {
        RotateToPlayer();
    }

    void RotateToPlayer()
    {
        if(isCharged)
        {
            transform.position += Time.deltaTime * targetPos * speed;
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
        Vector3 player = target.transform.position; // 도착점
        Gizmos.DrawLine(enemy,player);
    }
}
