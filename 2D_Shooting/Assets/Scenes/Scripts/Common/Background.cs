using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Background : MonoBehaviour
{
    /// <summary>
    /// �̵��ӵ�
    /// </summary>
    public float scrollingSpeed = 2.5f;
    /// <summary>
    /// ���� �׸� ����
    /// </summary>
    const float Backgroundwidth = 13.0f;

    /// <summary>
    /// ���
    /// </summary>
    public Transform[] bgSlots;

    /// <summary>
    /// ������ ������ ����
    /// </summary>
    float baseLineX;

    protected virtual void Awake()
    {
        bgSlots = new Transform[transform.childCount]; // �迭�����

        for(int i = 0; i < bgSlots.Length; i++) 
        {
            bgSlots[i] = transform.GetChild(i); // �ڽ� �ֱ�
        }

        baseLineX = transform.position.x - Backgroundwidth; // ���� ����
    }

    void Update()
    {
        for(int i = 0; i < bgSlots.Length; i++)
        {
            bgSlots[i].Translate(Time.deltaTime * scrollingSpeed * -transform.right); // ��ũ�Ѹ�

            if (bgSlots[i].position.x < baseLineX)
            {
                MoveRight(i); // ���ؿ� �����ϸ� ���������� ������
            }
        }




        //transform.position += Time.deltaTime * scrollingSpeed * Vector3.left;
        //if (transform.position.x < -23f) // -23 -10 3
        //    transform.position = new Vector3(transform.position.x + (Backgroundwidth * 3),
        //                                     transform.position.y);
    }

    protected virtual void MoveRight(int index)
    {
        bgSlots[index].Translate(Backgroundwidth * bgSlots.Length * transform.right);
    }
}
