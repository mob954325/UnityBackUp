using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TurretTrace : TurretBase
{
    /// <summary>
    /// 시야 범위
    /// </summary>
    public float sightRange = 10.0f;

    /// <summary>
    /// 터렛의 머리가 돌아가는 속도
    /// </summary>
    public float turnSpeed = 2.0f;

    /// <summary>
    /// 터렛이 총알 발사를 시작하는 좌우 발사각(+-10도)
    /// </summary>
    public float fireAngle = 10.0f;

    /// <summary>
    /// 시야범위 체크용 트리거
    /// </summary>
    SphereCollider sightTrigger;

    /// <summary>
    /// 내 시야각에 들어온 플레이어
    /// </summary>
    Player target;

    /// <summary>
    /// 발사 중인지 아닌지 표시하는 변수 (true면 발사 중)
    /// </summary>
    bool isFiring = false;

#if UNITY_EDITOR
    /// <summary>
    /// 내 공격 영역안에 플레이어가 있고 발사각 안에 있는 상태를 확인하기 위한 프로퍼티
    /// </summary>
    bool isRedState => isFiring;
    /// <summary>
    /// 내 공격 영역안에 플레이어가 있는 상태를 확인하기 위한 프로퍼티
    /// </summary>
    bool isOrangeState => (target != null);

    /// <summary>
    /// 플레이어가 보이는지 아닌지 표시해 놓는 함수(true면 무조건 target이 설정 되어 있다.)
    /// </summary>
    bool isTargetVisible = false;
#endif


    protected override void Awake()
    {
        base.Awake();
        sightTrigger = GetComponent<SphereCollider>();
    }

    void Start()
    {
        sightTrigger.radius = sightRange;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.transform == GameManager.Instance.Player.transform)
        {
            target = GameManager.Instance.Player;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform == GameManager.Instance.Player.transform)
        {
            target = null;
        }
    }

    void Update()
    {
        LookTargetAndAttack();
    }

    private void LookTargetAndAttack()
    {
        bool isStartFire = false;
        if (target != null)
        {
            Vector3 dir = target.transform.position - transform.position;
            dir.y = 0.0f;

            if(IsVisibleTarget(dir))
            {
                //barrelBody.forward = dir; // 즉시 바라보기

                barrelBody.rotation = Quaternion.Slerp(
                    barrelBody.rotation,
                    Quaternion.LookRotation(dir),
                    Time.deltaTime * turnSpeed);

                //Vector3.SignedAngle : 두 벡터의 사이각을 구하는데 방향를 고려하여 계산
                float angle = Vector2.Angle(barrelBody.forward, dir);
                if (angle < fireAngle)
                {
                    isStartFire = true; // 발사 결정
                }
            }
        }
#if UNITY_EDITOR
        else
        {
            isTargetVisible = false;
        }
#endif

        if(isStartFire) // 발사해야 하는 상황인지 확인
        {
            StartFire(); // 발사 시작
        }
        else
        {
            StopFire(); // 발사 정지
        }
    }

    /// <summary>
    /// Target이 보이는지 확인하는 함수
    /// </summary>
    /// <param name="lookDirection">바라보는 방향</param>
    /// <returns>true면 target이 있고 false면 target이 없다.</returns>
    private bool IsVisibleTarget(Vector3 lookDirection)
    {
        bool result = false;

        Ray ray = new Ray(barrelBody.position, lookDirection);

        if(Physics.Raycast(ray, out RaycastHit hitinfo, sightRange))
        {
            if(hitinfo.transform == target.transform) // hitinfo가 plyaer인가
            {
                result = true;
            }
        }
#if UNITY_EDITOR
        isTargetVisible = result;
#endif

        return result;
    }

    /// <summary>
    /// 총알 발사하기 시작 ( 중복 실행 X)
    /// </summary>
    void StartFire()
    {
        if(!isFiring)
        {
            StartCoroutine(fireCoroutine);
            isFiring = true;
        }
    }

    /// <summary>
    /// 총알 발사를 정지
    /// </summary>
    void StopFire()
    {
        if (isFiring)
        {
            StopCoroutine(fireCoroutine);
            isFiring = false;
        }
    }

#if UNITY_EDITOR
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        // sightRange 범위 그리기
        Handles.DrawWireDisc(transform.position, transform.up, sightRange);

        if(barrelBody == null)
        {
            barrelBody = transform.GetChild(2);
        }

        Vector3 from = transform.position;
        Vector3 to = transform.position + barrelBody.forward * sightRange;

        // 시야 중심선
        Handles.color = Color.yellow;
        Handles.DrawDottedLine(from,to, 2.0f);

        Vector3 dir1 = Quaternion.AngleAxis(-fireAngle, transform.up) * barrelBody.forward;
        Vector3 dir2 = Quaternion.AngleAxis(fireAngle, transform.up) * barrelBody.forward;

        // sightRange 내부
        Handles.color = Color.green;

        if(isRedState)
        {
            Handles.color = Color.red;
        }
        else if(isOrangeState)
        {
            Handles.color = new Color(1, 0.45f, 0);
        }

        to = transform.position + dir1 * sightRange;
        Handles.DrawLine(from,to, 3.0f);
        to = transform.position + dir2 * sightRange;
        Handles.DrawLine(from,to, 3.0f);

        Handles.DrawWireArc(from, transform.up, dir1, fireAngle * 2.0f, sightRange, 3.0f);
    }
#endif
}
