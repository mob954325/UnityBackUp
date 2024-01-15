using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Asteroid : EnemyBase
{
    public float minMoveSpeed = 2.0f;
    public float maxMoveSpeed = 4.0f;

    public float minRotateSpeed = 30.0f;
    public float maxRotateSpeed = 360.0f;

    public float minLifeTime = 4.0f;
    public float maxLifeTime = 7.0f;

    public int minMiniCount = 3;
    public int maxMiniCount = 8;

    [Range(0f,1f)]
    public float criticalRate = 0.05f;
    public int criticalMiniCount = 20;


    /// <summary>
    /// 회전속도
    /// </summary>
    private float rotateSpeed = 360.0f;

    /// <summary>
    /// 이동방향
    /// </summary>
    public Vector3 direction = Vector3.zero;

    /// <summary>
    /// astroidmini num
    /// </summary>
    [SerializeField]private int miniCount = 0;
    [SerializeField]private float baseSpeed = 7;

    /// <summary>
    /// 원래 점수
    /// </summary>
    int originalScore;

    void Awake()
    {
        originalScore = score;
    }

    /// <summary>
    /// 목적지를 이용해 방향을 결정하는 함수
    /// </summary>
    /// <param name="Destination">목적지위치</param>

    public void SetDestination(Vector3 Destination)
    {
        direction = (Destination - transform.position).normalized;
    }

    protected override void onInitialze()
    {
        moveSpeed = baseSpeed + Random.Range(-1.0f, 1.0f);
        rotateSpeed = Random.Range(minRotateSpeed, maxRotateSpeed);
        miniCount = Random.Range(minMiniCount, maxMiniCount);
        score = originalScore;

        StartCoroutine(SelfCrush());
    }

    protected override void OnMoveUpdate(float deltaTime)
    {
        transform.Translate(deltaTime * moveSpeed * direction, Space.World);    // direction 방향으로 이동하기(월드기준)
        transform.Rotate(0, 0, deltaTime * rotateSpeed);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + direction);
    }

    IEnumerator SelfCrush()
    {
        float lifeTime = Random.Range(minLifeTime, maxLifeTime);
        yield return new WaitForSeconds(lifeTime);
        score = 0;
        OnDie();
    }

    /// <summary>
    /// Die for asteroid
    /// </summary>
    protected override void OnDie()
    {
        // 작은 운석 만들기
        int count = criticalMiniCount;

        if(Random.value > criticalRate)
        {
            count = Random.Range(minMiniCount, maxMiniCount);
        }

        float angle = 360.0f / count;
        float startAngle = Random.Range(0, 360.0f);
        
        for(int i = 0; i < count; i++)
        {
            Factory.Instance.GetAsteroidMini(transform.position, startAngle + angle * i);
        }

        base.OnDie();
    }

    // 큰 운석은 죽을 때 작은 운석을 랜덤한 개수 생성한다.
    //  모든 작은 운석은 서로 같은 시야각을 가진다.(작은 운석이 6개 생성 = 시야각 60도)
    // critrate 확률로 작은 운석 20개 생성
}
