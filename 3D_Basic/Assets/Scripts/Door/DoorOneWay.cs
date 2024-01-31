using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOneWay : DoorBase
{
    Transform doorTransform;

    protected override void Awake()
    {
        base.Awake();
        Transform Hinge = transform.GetChild(1);

        doorTransform = Hinge.GetChild(0);
    }

    void OnTriggerEnter(Collider other)
    {
        Vector3 dir = other.transform.position - doorTransform.position;
        dir.y = 0.0f;

        float angle = Vector3.SignedAngle(doorTransform.forward, dir, Vector3.forward);
        Debug.Log($"Angle : {angle}");

        if (angle <= 90f) // forward방향만 열림
        {
            Open();
        }
    }

    void OnTriggerExit(Collider other)
    {
        Close();
    }
}
