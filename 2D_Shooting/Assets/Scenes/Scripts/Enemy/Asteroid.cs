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
    /// ȸ���ӵ�
    /// </summary>
    private float rotateSpeed = 360.0f;

    /// <summary>
    /// �̵�����
    /// </summary>
    public Vector3 direction = Vector3.zero;

    /// <summary>
    /// astroidmini num
    /// </summary>
    [SerializeField]private int miniCount = 0;
    [SerializeField]private float baseSpeed = 7;

    /// <summary>
    /// ���� ����
    /// </summary>
    int originalScore;

    void Awake()
    {
        originalScore = score;
    }

    /// <summary>
    /// �������� �̿��� ������ �����ϴ� �Լ�
    /// </summary>
    /// <param name="Destination">��������ġ</param>

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
        transform.Translate(deltaTime * moveSpeed * direction, Space.World);    // direction �������� �̵��ϱ�(�������)
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
        // ���� � �����
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

    // ū ��� ���� �� ���� ��� ������ ���� �����Ѵ�.
    //  ��� ���� ��� ���� ���� �þ߰��� ������.(���� ��� 6�� ���� = �þ߰� 60��)
    // critrate Ȯ���� ���� � 20�� ����
}
