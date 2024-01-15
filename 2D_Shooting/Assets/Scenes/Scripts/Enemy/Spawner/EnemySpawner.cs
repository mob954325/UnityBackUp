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

    // 1. 적을 생성한다
    // 2. 랜덤한 위치에서 생성한다 y +-4;
    // 3. 주기적으로 한마리씩

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

    protected Vector3 GetSpawnPosition() // 스폰할 위치를 반환하는 함수
    {
        Vector3 pos = transform.position;
        pos.y += Random.Range( MinY, MaxY ); // 높이만 랜덤값 적용

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
        Gizmos.color = Color.green; // 색지정
        Vector3 p0 = transform.position + Vector3.up * MaxY; // 시작점
        Vector3 p1 = transform.position + Vector3.up * MinY; // 도착점
        Gizmos.DrawLine(p0, p1); // 시작점과 도착점 사이에 선을 그어줌
    }

    protected virtual void OnDrawGizmosSelected()
    {
        // 이 오브젝트를 눌렀을 때 사각형 표현
        Gizmos.color = Color.red; // 색지정
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
