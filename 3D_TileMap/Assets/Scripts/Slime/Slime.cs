using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : RecycleObject
{
    /// <summary>
    /// 슬라임 이동속도
    /// </summary>
    public float moveSpeed = 2f;

    int hp = 5;
    public int HP
    {
        get => hp;
        set
        {
            hp = value;
            Debug.Log($"{gameObject.name} 의 체력은 [{hp}] 입니다");
        }
    }

    //

    /// <summary>
    /// 페이즈 진행 시간
    /// </summary>
    public float phaseDuration = 0.5f;

    /// <summary>
    /// 디졸브 진행 시간
    /// </summary>
    public float dissolveDuration = 1.0f;

    /// <summary>
    /// 슬라임의 머터리얼
    /// </summary>
    Material mainMaterial;

    /// <summary>
    /// 아웃라인이 보일 때 두께
    /// </summary>
    const float visibleOutlineThickness = 0.004f;

    /// <summary>
    /// 페이즈가 보일 때의 두께
    /// </summary>
    const float visiblePhaseThickness = 0.1f;

    /// <summary>
    /// 쉐이더 프로퍼티 아이디들
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
    /// 이 슬라임이 위치하고 있는 노드
    /// </summary>
    Node current = null;

    Node Current
    {
        get => current;
        set
        {
            if (current != value)
            {
                if(current != null) // 이전 노드가 null이면 스킵
                {
                    current.nodeType = Node.NodeType.Plain; // 이전 노드를 Plain으로 되돌리기
                }
                current = value;                        // 현재 노드 변경
                if(current != null) // 변경후 null 아니면 실행
                {
                    current.nodeType = Node.NodeType.Slime; // 새로 이동한 노드는 Slime으로 바꾸기
                }
            }
        }
    }

    /// <summary>
    /// 슬라임의 이동 활성화 표시용 변수 (true면 움직임, false면 안움직임)
    /// </summary>
    bool isMoveActivate = false;

    /// <summary>
    /// 죽음을 알리는 델리게이트
    /// </summary>
    public Action onDie;

    /// <summary>
    /// 슬라임이 생성된 풀의 트랜스폼
    /// </summary>
    Transform pool;

    /// <summary>
    /// pool에 단 한번만 값을 설정하는 프로퍼티
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

    // order in Layer 수정용
    SpriteRenderer spriteRenderer;

    /// <summary>
    /// 다른 슬라임에 의해 경로가 박혔을 때 기다린 시간
    /// </summary>
    float pathWaitTime = 0.0f;

    /// <summary>
    /// 경로가 막혔을 때 
    /// </summary>
    const float MaxpathWaitTime = 1.0f;

    /// <summary>
    /// 이 슬라임이 죽을 때 플레이어에게 주는 수명 보너스
    /// </summary>
    public float lifeTimeBonus = 2.0f;

    float LifeTimeBonus
    {
        get => lifeTimeBonus;
        set
        {
            if (lifeTimeBonus != value)
            {
                lifeTimeBonus = value; // 값 설정

                lifeTimeBonus = Mathf.Clamp(lifeTimeBonus, 0.0f, 2.0f);    // 일정 범위를 벗어나지 않게 만들기
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
        MoveUpdate(); // 이동 처리
        LifeTimeBonus -= Time.deltaTime * 0.5f;
    }

    /// <summary>
    /// 슬라임 초기화용 함수(스폰 직후에 실행)
    /// </summary>
    /// <param name="gridMap">슬라임이 존재할 타일 맵</param>
    /// <param name="world">슬라임의 시작 위치 (월드 좌표) </param>
    public void Initialized(TileGridMap gridMap, Vector3 world)
    {
        map = gridMap;
        transform.position = map.GridToWolrd(map.WorldToGrid(world)); // 셀의 가운데에 위치
        Current = map.GetNode(world);
    }

    /// <summary>
    /// 슬라임이 죽을 때 실행하는 함수
    /// </summary>
    public void Die()
    {
        isMoveActivate = false;             // 이동 비활성화
        onDie?.Invoke();                    // 사망 알림
        onDie = null;                       // 델리게이트 제거
        StartCoroutine(StartDissolve());    // 디졸브만 실행 (디졸브 코루틴에서 비활성화까지 처리함)
    }

    /// <summary>
    /// 비활성화 시키면서 풀로 돌려보내는 함수
    /// </summary>
    public void ReturnToPool()
    {
        Current = null;
        transform.SetParent(pool);          // 풀로 다시 부모변경
        gameObject.SetActive(false);        // 비활성화
    }

    void resetShaderProperty()
    {
        // - 리셋
        ShowOutline(false);                             // 아웃라인 끄고
        mainMaterial.SetFloat(PhaseThicknessID, 0);     // 페이즈 선안보이게 하기
        mainMaterial.SetFloat(PhaseSplitID, 1);         // 전신 보이게 하기
        mainMaterial.SetFloat(DissolveFadeID, 1);       // 디졸브 안보이게 하기
    }

    /// <summary>
    /// 아웃라인 켜고 끄는 함수
    /// </summary>
    /// <param name="isShow">true면 보이고 false면 보이지 않는다.</param>
    public void ShowOutline(bool isShow = true)
    {
        // - Outline on/off
        if (isShow)
        {
            mainMaterial.SetFloat(OutlineThicknessID, visibleOutlineThickness); // 보이는 것은 두께를 설정
        }
        else
        {
            mainMaterial.SetFloat(OutlineThicknessID, 0); // 안보이게 하는 것은 두께를 0으로 설정
        }
    }

    /// <summary>
    /// 페이즈 진행하는 코루틴 (안보기 -> 보기)
    /// </summary>
    IEnumerator StartPhase()
    {
        // - PhaseReverse로 안보이는 상태에서 보이게 만들기

        float phaseNormalize = 1.0f / phaseDuration;    // 나누기 계선을 줄이기 위해 미리 계산
        float timeElpased = 0.0f;                       // 시간 누적용

        mainMaterial.SetFloat(PhaseThicknessID, visiblePhaseThickness); // 페이즈 선을 보이게 만들기

        while (timeElpased < phaseDuration)  // 시간진행에 따라 처리
        {
            timeElpased += Time.deltaTime;  // 시간 누적

            mainMaterial.SetFloat(PhaseSplitID, 1 - (timeElpased * phaseNormalize)); // split 값을 누적한 시간에 따라 변경

            yield return null;
        }

        mainMaterial.SetFloat(PhaseThicknessID, 0); // 페이즈 선 안보이게 하기
        mainMaterial.SetFloat(PhaseSplitID, 0);     // float 숫자 정리

        onPhaseEnd?.Invoke();
    }

    /// <summary>
    /// 리졸브 진행하는 코루틴 (안보기 -> 보기)
    /// </summary>
    IEnumerator StartDissolve()
    {
        // - Dissolve 실행시키기

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
    /// 슬라임의 목적지를 지정하는 함수
    /// </summary>
    /// <param name="destination">목적지의 그리드 좌표</param>
    public void SetDestination(Vector2Int destination)
    {
        // 목적지 까지 경로 지정
        path = AStar.PathFind(map, GridPosition, destination);

        // 경로 그리기
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
            if (path != null && path.Count > 0 && pathWaitTime < MaxpathWaitTime) // 경로가 남아 있고 오래 기다리지 않았을 때의 처리
            {
                // path의 첫번째 위치로 계속 이동
                Vector2Int destGrid = path[0];                          // path위치 가져오기

                // 다른 슬라임이 있는 칸에는 이동하지 않는다.
                // -> 슬라임으로 표시된 노드가 아니거나, 내가 있는 노드 일 때만 움직이기
                if(!map.IsSlime(destGrid) || map.GetNode(destGrid) == Current)
                {
                    // 실제 이동 처리
                    Vector3 destPosition = map.GridToWolrd(destGrid);       // 목적지 월드 좌표 구하기
                    Vector3 direction = destPosition - transform.position;  // 방향 계산

                    if (direction.sqrMagnitude < 0.001f) // 도착했으면
                    {
                        // 첫번째 위치에 도착하면 path의 첫번째 위치를 제거
                        transform.position = destPosition;                  // 오차 보정
                        path.RemoveAt(0);                                   // 제거
                    }
                    else
                    {
                        // 도착안했으면 direction 방향으로 이동
                        transform.Translate(Time.deltaTime * moveSpeed * direction.normalized);
                        Current = map.GetNode(transform.position); // Current 변경 시도
                    }
                    spriteRenderer.sortingOrder = -Mathf.FloorToInt(transform.position.y * 100); // 아래쪽에 있는 슬라임이 위에 그려지게 하기

                    pathWaitTime = 0.0f;    // 기다리는 시간 초기화
                }
                else
                {
                    // 다른 슬라임이 있는 노드라 기다리기
                    pathWaitTime += Time.deltaTime; // 기다리기 시간 누적
                }
            }
            else
            {
                // 목적지에 도착 or 오래 기다렸음
                pathWaitTime = 0.0f;    // 시간 초기화
                OnDestinationArrive();  // 다음 목적지 설정
            }
        }
    }

    /// <summary>
    /// 목적지에 도착했을 때 실행하는 함수
    /// </summary>
    void OnDestinationArrive()
    {
        SetDestination(map.GetRandomMoveablePosition());
    }

    /// <summary>
    /// 경로를 보여줄지 말지 결정하는 함수
    /// </summary>
    /// <param name="isShow">true면 보이고 false면 보여주지 않는다.</param>
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
