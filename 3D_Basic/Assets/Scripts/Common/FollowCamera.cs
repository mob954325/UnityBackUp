using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// �÷��̾ õõ�� ���󰡴� ī�޶�
public class FollowCamera : MonoBehaviour
{
    /// <summary>
    /// ī�޶� ����ٴϴ� ���
    /// </summary>
    public Transform target;

    /// <summary>
    /// ī�޶� ����ٴϴ� �ӵ�
    /// </summary>
    public float speed = 3.0f;

    /// <summary>
    /// �÷��̾�� ī�޶��� ����
    /// </summary>
    Vector3 offset;
    
    /// <summary>
    /// �÷��̾�� ī�޶� ���� �Ÿ�
    /// </summary>
    float length;

    private void Start()
    {
        if(target == null)
        {
            target = GameManager.Instance.Player.transform.GetChild(7);
        }

        offset = transform.position - target.position; // target���� �÷��̾�� ���� ����
        length = offset.magnitude;
    }

    void FixedUpdate()
    {
        // Vector3.Slerp���� õõ�� ���󰡴� ������ ī�޶� �̵���Ű��
        transform.position = Vector3.Slerp(target.position, 
                                           target.position + Quaternion.LookRotation(target.forward) * offset,
                                           Time.fixedDeltaTime * speed);
        transform.LookAt(target);// target �ٶ󺸱�

        // �÷��̾�� ī�޶� ���̿� ��ֹ��� ������ �浹�������� ī�޶� �̵���Ų��.
        // raycastȰ��
        Ray ray = new Ray(target.position, transform.position - target.position);
        if(Physics.Raycast(ray, out RaycastHit hitInfo, length))
        {
            if(hitInfo.collider.gameObject.layer != 6)
                transform.position = hitInfo.point;
        }
    }
}
