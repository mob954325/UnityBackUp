using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    /*public GameObject enemyPrefab;
    public float SpawnTimer = 3f;
    public Vector3 position;*/
    /*float timer;
    float randomY;*/

    // 1. ���� �����Ѵ�
    // 2. ������ ��ġ���� �����Ѵ� y +-4;
    // 3. �ֱ������� �Ѹ�����

    public GameObject enemyPrefab;
    public float interval = 0.5f;

    const float MinY = -4.0f;
    const float MaxY = 4.0f;

    //float elapsedTime = 0;

    int spawnCounter = 0;

    void Awake()
    {
        // float rand = Random.Range(MinY, MaxY); // ����
    }

    void Start()
    {
        spawnCounter = 0;
        //elapsedTime = 0.0f;

        StartCoroutine(SpawnCoroutine());
    }

    void RandomSpawn()
    {
        /*        timer += Time.deltaTime;
        randomY = Random.Range(-4, 4);


        position = new Vector3 (transform.position.x, 
                                transform.position.y + randomY, 
                                0);
        if(timer > SpawnTimer)
        {
            timer = 0;
            Instantiate(enemyPrefab, position, Quaternion.identity);
        }*/
    }

    void Spawn() // Enemy Spawn
    {
        GameObject obj = Instantiate(enemyPrefab, GetSpawnPosition(), Quaternion.identity);
        obj.transform.SetParent(transform);
        obj.name = $"Enemy_{spawnCounter}";
        spawnCounter++;
    }

    Vector3 GetSpawnPosition() // ������ ��ġ�� ��ȯ�ϴ� �Լ�
    {
        Vector3 pos = transform.position;
        pos.y += Random.Range( MinY, MaxY ); // ���̸� ������ ����

        return pos;
    }

    IEnumerator SpawnCoroutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(interval);
            Spawn();
        }
            
    }

    // Gizmos
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green; // ������
        Vector3 p0 = transform.position + Vector3.up * MaxY; // ������
        Vector3 p1 = transform.position + Vector3.up * MinY; // ������
        Gizmos.DrawLine(p0, p1); // �������� ������ ���̿� ���� �׾���
    }

    private void OnDrawGizmosSelected()
    {
        // �� ������Ʈ�� ������ �� �簢�� ǥ��
        Gizmos.color = Color.red; // ������
        Vector3 p0 = transform.position + Vector3.up * MaxY + Vector3.right * 0.5f;
        Vector3 p1 = transform.position + Vector3.up * MinY + Vector3.right * 0.5f;
        Vector3 p2 = transform.position + Vector3.up * MaxY - Vector3.right;
        Vector3 p3 = transform.position + Vector3.up * MinY - Vector3.right;

        /// p2   p0
        ///
        /// p3   p1
        Gizmos.DrawLine(p2, p0);
        Gizmos.DrawLine(p0, p1);
        Gizmos.DrawLine(p1, p3);
        Gizmos.DrawLine(p3, p2);
    }

}
