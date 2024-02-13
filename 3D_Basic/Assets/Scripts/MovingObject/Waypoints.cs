using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    /// <summary>
    /// ��� ��������Ʈ ������
    /// </summary>
    Transform[] waypoints;

    /// <summary>
    /// ���� �̵����� ��������Ʈ ������ �ε���
    /// </summary>
    int index = 0;

    /// <summary>
    /// ���� �̵����� ��������Ʈ ������ Ʈ������
    /// </summary>
    public Transform CurrentWayPoint => waypoints[index];

    void Awake()
    {
        // waypoints �ʱ�ȭ
        waypoints = new Transform[transform.childCount];
        for(int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = transform.GetChild(i);
        }
    }

    /// <summary>
    /// ���� ��������Ʈ ������ ��ȯ�ϸ� index�� ���� �����ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public Transform GetNextWayPoint()
    {
        // index �� 0-> 1 -> 2 ... -> 0 -> ...
        index++;
        index %= waypoints.Length;

        return waypoints[index];
    }
}
