using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveEnemy : EnemyBase
{
    //lambda


    /*    float timer = 0;
        public float speed = 1f;
        public float waveTime = 2f;
        public float waveStength = 1.2f;
        float wave;*/
    [Header("Wave info")]
    float spawnY = 0.0f;
    float elapsedTime = 0.0f;

    public float amplitude = 3.0f; // �� �Ʒ��� �����̴� �ӵ�
    public float frequency = 2.0f; // ���α׷����� �ѹ� �պ��ϴµ� �ɸ��� �ð�

    public void SetStartPosition(Vector3 position)
    {
        spawnY = position.y;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        spawnY = transform.position.y;
        elapsedTime = 0.0f;

        //Action aa = () => Debug.Log("�����Լ�");
        //Action<int> bb = (x) => Debug.Log($"�����Լ� {x}"); // �Ķ���� �ޱ�
        //Func<int> cc = () => 10; // �Ķ���� x, �׻� 10 ���� 

    }

    protected override void OnMoveUpdate(float deltaTime)
    {
        //elapsedTime += Time.deltaTime;
        elapsedTime += deltaTime * frequency; // sin �׷��� ���� ������ �ϱ�

        //waveSelf();
        transform.position = new Vector3(transform.position.x - deltaTime * moveSpeed,
                                  spawnY + Mathf.Sin(elapsedTime) * amplitude,
                                  0.0f);
    }


    // ���� �÷��̾�� ������ �ִ� ����� ��������Ʈ�� ó���ϵ��� ������
    // ����� �Ͼ�� ��ondie(��������Ʈ ����),������ �ۿ�player�� �Ͼ�� ��(��������Ʈ�� �Լ��� ���)
    // ���� ���ڰ� ���ʷ� �ö󰡵��� ����
}