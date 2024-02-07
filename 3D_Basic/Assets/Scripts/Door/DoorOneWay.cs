using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOneWay : DoorBase
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // other는 플레이어
            Vector3 playerToDoor = transform.position - other.transform.position; // 플레이어에서 문으로 향하는 방향벡터

            float angle = Vector3.Angle(transform.forward, playerToDoor); // 각도

            if(angle > 90.0f)
            {
                Open();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Close();
        }
    }
}
