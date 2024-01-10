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

    //void Start()
    //{
    //    // Destroy(gameObject, lifeTime); // lifeTime���� ������ ����
    //}

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(LifeOver(lifeTime));
    }

    void Update()
    {
        waveTime += Time.deltaTime;

        //transform.position += Vector3.right * Time.deltaTime * speed;

        if (waveTime > 0.5f) 
        {
            waveTime = 0;

            waveStength *= -1f;
        }

        transform.position += new Vector3(speed * Time.deltaTime, waveStength * Time.deltaTime);

        //transform.Translate(Time.deltaTime * speed * Vector2.right); ��Į�� * ���� -> ��� Ƚ�� 3
        //transform.Translate(Vector2.right * Time.deltaTime * speed); ���� * ��Į�� -> ��� Ƚ�� 4
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

/*        if (collision.gameObject.CompareTag("Player") ||
            collision.gameObject.CompareTag("Bullet"))
            return;*/

        Instantiate(effectPrefab, transform.position, Quaternion.identity); // hit ����Ʈ ����

        //Destroy(gameObject);
        gameObject.SetActive(false);
    }


    //1. Bullet �����鿡 �ʿ��� ������Ʈ �߰��ϰ� �����ϱ�
    //2. �Ѿ��� "Enemy" �±׸� ���� ������Ʈ�� �ε�ġ�� �ε�ģ ����� �����Ѵ�.
    //3. �Ѿ��� �ٸ� ������Ʈ�� �ε�ġ�� �ڱ� �ڽ��� �����Ѵ�.

    //4. Hit ��������Ʈ�� �̿��� HItEffect��� �������� �����
    //5. �Ѿ��� �ε�ģ ��ġ�� HitEffect�� �����Ѵ�.
    //6. HitEffect�� �ѹ��� ����ǰ� �������. -> ���� ������Ʈ�� ���ŵǾ���
}
