using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : RecycleObject
{
    /// <summary>
    /// ������ �̵��ӵ�
    /// </summary>
    public float moveSpeed = 2f;

    int hp = 5;
    public int HP
    {
        get => hp;
        set
        {
            hp = value;
            Debug.Log($"{gameObject.name} �� ü���� [{hp}] �Դϴ�");
        }
    }

    //

    /// <summary>
    /// ������ ���� �ð�
    /// </summary>
    public float phaseDuration = 0.5f;

    /// <summary>
    /// ������ ���� �ð�
    /// </summary>
    public float dissolveDuration = 1.0f;

    /// <summary>
    /// �������� ���͸���
    /// </summary>
    Material mainMaterial;

    /// <summary>
    /// �ƿ������� ���� �� �β�
    /// </summary>
    const float visibleOutlineThickness = 0.004f;

    /// <summary>
    /// ����� ���� ���� �β�
    /// </summary>
    const float visiblePhaseThickness = 0.1f;

    /// <summary>
    /// ���̴� ������Ƽ ���̵��
    /// </summary>
    readonly int OutlineThicknessID = Shader.PropertyToID("_OutlineThickness");
    readonly int PhaseSplitID = Shader.PropertyToID("_PhaseSplit");
    readonly int PhaseThicknessID = Shader.PropertyToID("_PhaseThickness");
    readonly int DissolveFadeID = Shader.PropertyToID("_DissolveFade");

    Action onPhaseEnd;
    Action onDissolveEnd;

    // path
    public List<Vector2Int> path;
    PathLine pathLine;
    TileGridMap map;

    Vector2Int GridPosition => map.WorldToGrid(transform.position);

    /// <summary>
    /// �� �������� ��ġ�ϰ� �ִ� ���
    /// </summary>
    Node current = null;

    Node Current
    {
        get => current;
        set
        {
            if (current != value)
            {
                if(current != null) // ���� ��尡 null�̸� ��ŵ
                {
                    current.nodeType = Node.NodeType.Plain; // ���� ��带 Plain���� �ǵ�����
                }
                current = value;                        // ���� ��� ����
                if(current != null) // ������ null �ƴϸ� ����
                {
                    current.nodeType = Node.NodeType.Slime; // ���� �̵��� ���� Slime���� �ٲٱ�
                }
            }
        }
    }

    /// <summary>
    /// �������� �̵� Ȱ��ȭ ǥ�ÿ� ���� (true�� ������, false�� �ȿ�����)
    /// </summary>
    bool isMoveActivate = false;

    /// <summary>
    /// ������ �˸��� ��������Ʈ
    /// </summary>
    public Action onDie;

    /// <summary>
    /// �������� ������ Ǯ�� Ʈ������
    /// </summary>
    Transform pool;

    /// <summary>
    /// pool�� �� �ѹ��� ���� �����ϴ� ������Ƽ
    /// </summary>
    public Transform Pool
    {
        set
        {
            if(pool == null)
            {
                pool = value;
            }
        }
    }

    // order in Layer ������
    SpriteRenderer spriteRenderer;

    /// <summary>
    /// �ٸ� �����ӿ� ���� ��ΰ� ������ �� ��ٸ� �ð�
    /// </summary>
    float pathWaitTime = 0.0f;

    /// <summary>
    /// ��ΰ� ������ �� 
    /// </summary>
    const float MaxpathWaitTime = 1.0f;

    /// <summary>
    /// �� �������� ���� �� �÷��̾�� �ִ� ���� ���ʽ�
    /// </summary>
    public float lifeTimeBonus = 2.0f;

    float LifeTimeBonus
    {
        get => lifeTimeBonus;
        set
        {
            if (lifeTimeBonus != value)
            {
                lifeTimeBonus = value; // �� ����

                lifeTimeBonus = Mathf.Clamp(lifeTimeBonus, 0.0f, 2.0f);    // ���� ������ ����� �ʰ� �����
            }
        }
    }

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainMaterial = spriteRenderer.material;

        onPhaseEnd += () =>
        {
            isMoveActivate = true;
        };

        onDissolveEnd += ReturnToPool;

        path = new List<Vector2Int>();
        pathLine = GetComponentInChildren<PathLine>();

        ShowPath(false);
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        resetShaderProperty();
        StartCoroutine(StartPhase());

        isMoveActivate = false;
    }

    protected override void OnDisable()
    {
        path.Clear();
        pathLine.ClearPath();

        base.OnDisable();
    }

    void Update()
    {
        MoveUpdate(); // �̵� ó��
        LifeTimeBonus -= Time.deltaTime * 0.5f;
    }

    /// <summary>
    /// ������ �ʱ�ȭ�� �Լ�(���� ���Ŀ� ����)
    /// </summary>
    /// <param name="gridMap">�������� ������ Ÿ�� ��</param>
    /// <param name="world">�������� ���� ��ġ (���� ��ǥ) </param>
    public void Initialized(TileGridMap gridMap, Vector3 world)
    {
        map = gridMap;
        transform.position = map.GridToWolrd(map.WorldToGrid(world)); // ���� ����� ��ġ
        Current = map.GetNode(world);
    }

    /// <summary>
    /// �������� ���� �� �����ϴ� �Լ�
    /// </summary>
    public void Die()
    {
        isMoveActivate = false;             // �̵� ��Ȱ��ȭ
        onDie?.Invoke();                    // ��� �˸�
        onDie = null;                       // ��������Ʈ ����
        StartCoroutine(StartDissolve());    // �����길 ���� (������ �ڷ�ƾ���� ��Ȱ��ȭ���� ó����)
    }

    /// <summary>
    /// ��Ȱ��ȭ ��Ű�鼭 Ǯ�� ���������� �Լ�
    /// </summary>
    public void ReturnToPool()
    {
        Current = null;
        transform.SetParent(pool);          // Ǯ�� �ٽ� �θ𺯰�
        gameObject.SetActive(false);        // ��Ȱ��ȭ
    }

    void resetShaderProperty()
    {
        // - ����
        ShowOutline(false);                             // �ƿ����� ����
        mainMaterial.SetFloat(PhaseThicknessID, 0);     // ������ ���Ⱥ��̰� �ϱ�
        mainMaterial.SetFloat(PhaseSplitID, 1);         // ���� ���̰� �ϱ�
        mainMaterial.SetFloat(DissolveFadeID, 1);       // ������ �Ⱥ��̰� �ϱ�
    }

    /// <summary>
    /// �ƿ����� �Ѱ� ���� �Լ�
    /// </summary>
    /// <param name="isShow">true�� ���̰� false�� ������ �ʴ´�.</param>
    public void ShowOutline(bool isShow = true)
    {
        // - Outline on/off
        if (isShow)
        {
            mainMaterial.SetFloat(OutlineThicknessID, visibleOutlineThickness); // ���̴� ���� �β��� ����
        }
        else
        {
            mainMaterial.SetFloat(OutlineThicknessID, 0); // �Ⱥ��̰� �ϴ� ���� �β��� 0���� ����
        }
    }

    /// <summary>
    /// ������ �����ϴ� �ڷ�ƾ (�Ⱥ��� -> ����)
    /// </summary>
    IEnumerator StartPhase()
    {
        // - PhaseReverse�� �Ⱥ��̴� ���¿��� ���̰� �����

        float phaseNormalize = 1.0f / phaseDuration;    // ������ �輱�� ���̱� ���� �̸� ���
        float timeElpased = 0.0f;                       // �ð� ������

        mainMaterial.SetFloat(PhaseThicknessID, visiblePhaseThickness); // ������ ���� ���̰� �����

        while (timeElpased < phaseDuration)  // �ð����࿡ ���� ó��
        {
            timeElpased += Time.deltaTime;  // �ð� ����

            mainMaterial.SetFloat(PhaseSplitID, 1 - (timeElpased * phaseNormalize)); // split ���� ������ �ð��� ���� ����

            yield return null;
        }

        mainMaterial.SetFloat(PhaseThicknessID, 0); // ������ �� �Ⱥ��̰� �ϱ�
        mainMaterial.SetFloat(PhaseSplitID, 0);     // float ���� ����

        onPhaseEnd?.Invoke();
    }

    /// <summary>
    /// ������ �����ϴ� �ڷ�ƾ (�Ⱥ��� -> ����)
    /// </summary>
    IEnumerator StartDissolve()
    {
        // - Dissolve �����Ű��

        float dissolveNormalize = 1.0f / dissolveDuration;
        float timeElpased = 0.0f;

        while (timeElpased < dissolveDuration)
        {
            timeElpased += Time.deltaTime;

            mainMaterial.SetFloat(DissolveFadeID, 1 - (timeElpased * dissolveNormalize));

            yield return null;
        }

        mainMaterial.SetFloat(DissolveFadeID, 0);

        onDissolveEnd?.Invoke();
    }

    /// <summary>
    /// �������� �������� �����ϴ� �Լ�
    /// </summary>
    /// <param name="destination">�������� �׸��� ��ǥ</param>
    public void SetDestination(Vector2Int destination)
    {
        // ������ ���� ��� ����
        path = AStar.PathFind(map, GridPosition, destination);

        // ��� �׸���
        pathLine.DrawPath(map, path);
    }

    public void SetNewDestination()
    {
        Vector2Int newPos = map.GetRandomMoveablePosition();

        while(true)
        {
            if (map.IsWall(newPos))
            {
                newPos = map.GetRandomMoveablePosition();
            }
            else
                break;
        }

        path = AStar.PathFind(map, GridPosition, newPos);

        pathLine.DrawPath(map, path);
    }

    void MoveUpdate()
    {
        if (isMoveActivate) 
        {
            if (path != null && path.Count > 0 && pathWaitTime < MaxpathWaitTime) // ��ΰ� ���� �ְ� ���� ��ٸ��� �ʾ��� ���� ó��
            {
                // path�� ù��° ��ġ�� ��� �̵�
                Vector2Int destGrid = path[0];                          // path��ġ ��������

                // �ٸ� �������� �ִ� ĭ���� �̵����� �ʴ´�.
                // -> ���������� ǥ�õ� ��尡 �ƴϰų�, ���� �ִ� ��� �� ���� �����̱�
                if(!map.IsSlime(destGrid) || map.GetNode(destGrid) == Current)
                {
                    // ���� �̵� ó��
                    Vector3 destPosition = map.GridToWolrd(destGrid);       // ������ ���� ��ǥ ���ϱ�
                    Vector3 direction = destPosition - transform.position;  // ���� ���

                    if (direction.sqrMagnitude < 0.001f) // ����������
                    {
                        // ù��° ��ġ�� �����ϸ� path�� ù��° ��ġ�� ����
                        transform.position = destPosition;                  // ���� ����
                        path.RemoveAt(0);                                   // ����
                    }
                    else
                    {
                        // ������������ direction �������� �̵�
                        transform.Translate(Time.deltaTime * moveSpeed * direction.normalized);
                        Current = map.GetNode(transform.position); // Current ���� �õ�
                    }
                    spriteRenderer.sortingOrder = -Mathf.FloorToInt(transform.position.y * 100); // �Ʒ��ʿ� �ִ� �������� ���� �׷����� �ϱ�

                    pathWaitTime = 0.0f;    // ��ٸ��� �ð� �ʱ�ȭ
                }
                else
                {
                    // �ٸ� �������� �ִ� ���� ��ٸ���
                    pathWaitTime += Time.deltaTime; // ��ٸ��� �ð� ����
                }
            }
            else
            {
                // �������� ���� or ���� ��ٷ���
                pathWaitTime = 0.0f;    // �ð� �ʱ�ȭ
                OnDestinationArrive();  // ���� ������ ����
            }
        }
    }

    /// <summary>
    /// �������� �������� �� �����ϴ� �Լ�
    /// </summary>
    void OnDestinationArrive()
    {
        SetDestination(map.GetRandomMoveablePosition());
    }

    /// <summary>
    /// ��θ� �������� ���� �����ϴ� �Լ�
    /// </summary>
    /// <param name="isShow">true�� ���̰� false�� �������� �ʴ´�.</param>
    public void ShowPath(bool isShow = true)
    {
        //pathLine.gameObject.SetActive(isShow);
        if(isShow)
        {
            pathLine.DrawPath(map, path);
        }
        else
        {
            pathLine.ClearPath();
        }
    }

#if UNITY_EDITOR
    public void TestShader(int index)
    {
        switch(index)
        {
            case 0:
                resetShaderProperty();
                break;
            case 1:
                ShowOutline(true);
                break;
            case 2:
                ShowOutline(false);
                break;
            case 3:
                StartCoroutine(StartPhase());
                break;
            case 4:
                StartCoroutine(StartDissolve());
                break;
        }
    }

    public void TestDie()
    {
        Die();
    }
#endif
}
