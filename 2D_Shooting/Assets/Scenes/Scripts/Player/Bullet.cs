using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Bullet : RecycleObject
{
    // �������ڸ��� ��� ���������� �ʼ� 7�� �����̰� �����

    public float speed = 7.0f;
    public float waveStength = 1.2f;
    float waveTime = 0;

    /// <summary>
    /// �Ѿ� ����
    /// </summary>
    public float lifeTime = 10.0f;

    /// <summary>
    /// effect
    /// </summary>
    public GameObject effectPrefab;

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(LifeOver(lifeTime));
    }

    void Update()
    {
        waveTime += Time.deltaTime;

        if (waveTime > 0.5f) 
        {
            waveTime = 0;

            waveStength *= -1f;
        }

        transform.Translate(Time.deltaTime * speed * Vector2.right);

        //transform.Translate(Time.deltaTime * speed * Vector2.right); ��Į�� * ���� -> ��� Ƚ�� 3
        //transform.Translate(Vector2.right * Time.deltaTime * speed); ���� * ��Į�� -> ��� Ƚ�� 4
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Factory.Instance.GetHitEffect(transform.position);

        //Destroy(gameObject);
        gameObject.SetActive(false);
    }
}
