using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKey : MonoBehaviour
{
    public float rotateSpeed = 360.0f;

    Transform modelTransform; // Key model

    public Action onConsume;

    public bool CanUse => throw new NotImplementedException();

    void Start() 
    {
        modelTransform = transform.GetChild(0);
    }

    void Update()
    {
        modelTransform.Rotate(Time.deltaTime * rotateSpeed * Vector3.up);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            OnConsume();
        }
    }

    /// �� ���踦 ȹ������ �� �Ͼ ���� ó���ϴ� �Լ�
    protected virtual void OnConsume()
    {
        onConsume?.Invoke();
        Destroy(this.gameObject);
    }

    public void Use()
    {
        throw new System.NotImplementedException();
    }
}
