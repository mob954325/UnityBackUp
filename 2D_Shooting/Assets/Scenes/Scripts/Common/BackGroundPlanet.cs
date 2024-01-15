using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundPlanet : MonoBehaviour
{
    public float moveSpeed = 5.0f;

    public float minRight = 30.0f;
    public float maxRightEnd = 60.0f;
    public float minY = -8.0f;
    public float maxY = -5.0f;

    float baseLineX;

    void Start()
    {
        baseLineX = transform.position.x;// ���ؼ� = �ʱ���ġ x
    }

    void Update()
    {
        // �������� �̵�
        transform.Translate(Time.deltaTime * moveSpeed * -transform.right);

        if(transform.position.x < baseLineX) // ���ؼ��� ������
        {
            transform.position = new Vector3
                (Random.Range(minRight, maxRightEnd), // ������ ������
                 Random.Range(minY, maxY), // ������ y������ ��ġ����
                 0.0f);
        }
    }
}
