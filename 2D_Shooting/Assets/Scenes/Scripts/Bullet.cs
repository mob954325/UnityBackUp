using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // �������ڸ��� ��� ���������� �ʼ� 7�� �����̰� �����

    public float speed = 7.0f;
    float waveTime = 0;
    public float waveStength = 1.2f;

    void Update()
    {
        waveTime += Time.deltaTime;

        //transform.position += Vector3.right * Time.deltaTime * speed;

        if (waveTime > 0.5f) 
        {
            waveTime = 0;

            waveStength *= -1f;
        }

        transform.position += new Vector3(speed * Time.deltaTime, waveStength * Time.deltaTime);

        //transform.Translate(Time.deltaTime * speed * Vector2.right); ��Į�� * ���� -> ��� Ƚ�� 3
        //transform.Translate(Vector2.right * Time.deltaTime * speed); ���� * ��Į�� -> ��� Ƚ�� 4
    }
}
