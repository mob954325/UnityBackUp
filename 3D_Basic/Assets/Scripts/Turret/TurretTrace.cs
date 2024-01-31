using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TurretTrace : TurretBase
{
    /// <summary>
    /// �þ� ����
    /// </summary>
    public float sightRange = 10.0f;

    /// <summary>
    /// �ͷ��� �Ӹ��� ���ư��� �ӵ�
    /// </summary>
    public float turnSpeed = 2.0f;

    /// <summary>
    /// �ͷ��� �Ѿ� �߻縦 �����ϴ� �¿� �߻簢(+-10��)
    /// </summary>
    public float fireAngle = 10.0f;

    /// <summary>
    /// �þ߹��� üũ�� Ʈ����
    /// </summary>
    SphereCollider sightTrigger;

    /// <summary>
    /// �� �þ߰��� ���� �÷��̾�
    /// </summary>
    Player target;

    /// <summary>
    /// �߻� ������ �ƴ��� ǥ���ϴ� ���� (true�� �߻� ��)
    /// </summary>
    bool isFiring = false;

#if UNITY_EDITOR
    /// <summary>
    /// �� ���� �����ȿ� �÷��̾ �ְ� �߻簢 �ȿ� �ִ� ���¸� Ȯ���ϱ� ���� ������Ƽ
    /// </summary>
    bool isRedState => isFiring;
    /// <summary>
    /// �� ���� �����ȿ� �÷��̾ �ִ� ���¸� Ȯ���ϱ� ���� ������Ƽ
    /// </summary>
    bool isOrangeState => (target != null);

    /// <summary>
    /// �÷��̾ ���̴��� �ƴ��� ǥ���� ���� �Լ�(true�� ������ target�� ���� �Ǿ� �ִ�.)
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
                //barrelBody.forward = dir; // ��� �ٶ󺸱�

                barrelBody.rotation = Quaternion.Slerp(
                    barrelBody.rotation,
                    Quaternion.LookRotation(dir),
                    Time.deltaTime * turnSpeed);

                //Vector3.SignedAngle : �� ������ ���̰��� ���ϴµ� ���⸦ ����Ͽ� ���
                float angle = Vector2.Angle(barrelBody.forward, dir);
                if (angle < fireAngle)
                {
                    isStartFire = true; // �߻� ����
                }
            }
        }
#if UNITY_EDITOR
        else
        {
            isTargetVisible = false;
        }
#endif

        if(isStartFire) // �߻��ؾ� �ϴ� ��Ȳ���� Ȯ��
        {
            StartFire(); // �߻� ����
        }
        else
        {
            StopFire(); // �߻� ����
        }
    }

    /// <summary>
    /// Target�� ���̴��� Ȯ���ϴ� �Լ�
    /// </summary>
    /// <param name="lookDirection">�ٶ󺸴� ����</param>
    /// <returns>true�� target�� �ְ� false�� target�� ����.</returns>
    private bool IsVisibleTarget(Vector3 lookDirection)
    {
        bool result = false;

        Ray ray = new Ray(barrelBody.position, lookDirection);

        if(Physics.Raycast(ray, out RaycastHit hitinfo, sightRange))
        {
            if(hitinfo.transform == target.transform) // hitinfo�� plyaer�ΰ�
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
    /// �Ѿ� �߻��ϱ� ���� ( �ߺ� ���� X)
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
    /// �Ѿ� �߻縦 ����
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

        // sightRange ���� �׸���
        Handles.DrawWireDisc(transform.position, transform.up, sightRange);

        if(barrelBody == null)
        {
            barrelBody = transform.GetChild(2);
        }

        Vector3 from = transform.position;
        Vector3 to = transform.position + barrelBody.forward * sightRange;

        // �þ� �߽ɼ�
        Handles.color = Color.yellow;
        Handles.DrawDottedLine(from,to, 2.0f);

        Vector3 dir1 = Quaternion.AngleAxis(-fireAngle, transform.up) * barrelBody.forward;
        Vector3 dir2 = Quaternion.AngleAxis(fireAngle, transform.up) * barrelBody.forward;

        // sightRange ����
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
