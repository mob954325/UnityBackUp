using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TurretBase : MonoBehaviour
{
    // TurretStandard�� TurretTrace�� ���� �θ�
    public PoolObjectType bulletType = PoolObjectType.TurretBullet;

    /// <summary>
    /// �Ѿ� �߻� ���͹�
    /// </summary>
    public float fireInterval = 1.0f;

    /// <summary>
    /// �Ѹ��� Ʈ������
    /// </summary>
    protected Transform barrelBody;

    /// <summary>
    /// �߻� ��ġ
    /// </summary>
    protected Transform fireTransform;

    /// <summary>
    /// �Ѿ��� �ֱ������� �߻��ϴ� �ڷ�ƾ
    /// </summary>
    protected IEnumerator fireCoroutine;

    protected virtual void Awake()
    {
        barrelBody = transform.GetChild(2);
        fireTransform = barrelBody.GetChild(1);

        fireCoroutine = PeriodFire();
    }

    /// <summary>
    /// ���͹� �� �ѹ��� �Ѿ��� ��� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator PeriodFire()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireInterval);
            Factory.Instance.GetObject(bulletType, fireTransform.position, fireTransform.rotation.eulerAngles);
        }
    }

#if UNITY_EDITOR
    protected virtual void OnDrawGizmos()
    {
        Vector3 p0 = transform.position;
        Vector3 p1 = transform.position + transform.forward * 2.0f;
        Vector3 p2 = p1 + Quaternion.AngleAxis(25.0f, transform.up) * (-transform.forward * 0.2f); 
        Vector3 p3 = p1 + Quaternion.AngleAxis(-25.0f, transform.up) * (-transform.forward * 0.2f);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(p0, p1);
        Gizmos.DrawLine(p1, p2);
        Gizmos.DrawLine(p1, p3);
    }
#endif
}
