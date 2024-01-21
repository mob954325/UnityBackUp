using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingMap : MonoBehaviour
{
    public float _scrollingSpeed;

    /// <summary>
    /// �����ϸ� �ٽ� ���ư� ��ġ ��, ���� �� X
    /// </summary>
    public float _setPos = 32f;
    Vector3 _returnPos;

    void Update()
    {
        if(transform.position.x < -_setPos)
        {
            _returnPos = new Vector3(_setPos, transform.position.y);
            transform.localPosition = _returnPos;
        }

        transform.position += Time.deltaTime * Vector3.left * _scrollingSpeed;
    }
}
