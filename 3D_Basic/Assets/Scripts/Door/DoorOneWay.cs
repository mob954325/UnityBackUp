using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOneWay : DoorBase
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // other�� �÷��̾�
            Vector3 playerToDoor = transform.position - other.transform.position; // �÷��̾�� ������ ���ϴ� ���⺤��

            float angle = Vector3.Angle(transform.forward, playerToDoor); // ����

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
