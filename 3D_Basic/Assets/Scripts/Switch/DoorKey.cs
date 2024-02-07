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

    /// 이 열쇠를 획득했을 때 일어날 일을 처리하는 함수
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
