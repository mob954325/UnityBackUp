using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    /// <summary>
    /// 모든 웨이포인트 지점들
    /// </summary>
    Transform[] waypoints;

    /// <summary>
    /// 현재 이동중인 웨이포인트 지점의 인덱스
    /// </summary>
    int index = 0;

    /// <summary>
    /// 현재 이동중인 웨이포인트 지점의 트랜스폼
    /// </summary>
    public Transform CurrentWayPoint => waypoints[index];

    void Awake()
    {
        // waypoints 초기화
        waypoints = new Transform[transform.childCount];
        for(int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = transform.GetChild(i);
        }
    }

    /// <summary>
    /// 다음 웨이포인트 지점을 반환하며 index를 새로 지정하는 함수
    /// </summary>
    /// <returns></returns>
    public Transform GetNextWayPoint()
    {
        // index 를 0-> 1 -> 2 ... -> 0 -> ...
        index++;
        index %= waypoints.Length;

        return waypoints[index];
    }
}
