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

    //public GameObject enemyPrefab;
    public float interval = 0.5f;

    protected const float MinY = -4.0f;
    protected const float MaxY = 4.0f;

    void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }

    protected virtual void Spawn() // Enemy Spawn
    {

        Factory.Instance.GetEnemyWave(GetSpawnPosition());
    }

    protected Vector3 GetSpawnPosition() // ������ ��ġ�� ��ȯ�ϴ� �Լ�
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
    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.green; // ������
        Vector3 p0 = transform.position + Vector3.up * MaxY; // ������
        Vector3 p1 = transform.position + Vector3.up * MinY; // ������
        Gizmos.DrawLine(p0, p1); // �������� ������ ���̿� ���� �׾���
    }

    protected virtual void OnDrawGizmosSelected()
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
