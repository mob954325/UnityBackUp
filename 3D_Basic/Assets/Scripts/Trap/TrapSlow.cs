using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrapSlow : TrapBase
{
    //protected override void Awake()
    //{
    //    base.Awake();
    //}
    //
    //protected override void OnActivateTrap()
    //{
    //
    //}
    //
    //protected override void OnActivateTrapAction(Object obj)
    //{
    //    base.OnActivateTrapAction(obj);
    //
    //    Player player = obj.GetComponent<Player>();
    //
    //    if (player != null)
    //    {
    //        StartCoroutine(GetSlow(player));
    //    }
    //    else
    //        return;
    //}
    //
    //IEnumerator GetSlow(Player player)
    //{
    //    float preMoveSpeed = player.moveSpeed;
    //    player.moveSpeed /= 2;
    //    yield return new WaitForSeconds(2.5f);
    //    player.moveSpeed = preMoveSpeed;
    //}

    /// <summary>
    /// ����� �� ���ο� ���ӽð�
    /// </summary>
    public float slowDuration = 5.0f;

    /// <summary>
    /// ����� �� �������� ����
    /// </summary>
    [Range(0.1f, 2.0f)]
    public float slowRatio = 0.5f;
    ParticleSystem ps;

    void Awake()
    {
        Transform child = transform.GetChild(1);
        ps = child.GetComponent<ParticleSystem>();
    }

    protected override void OnTrapActivate(GameObject target)
    {
        ps.Play();
        Player player = target.GetComponent<Player>();
        if (player != null)
        {
            player.SetSpeedModifier(slowRatio); // ����� �÷��̾�� �ӵ� ����
        }
    }

    void OnTriggerExit(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            StopAllCoroutines();
            StartCoroutine(ResotoreSpeed(player)); // Ʈ���ſ��� ���� ���� �÷��̾��̸� slowDuration�Ŀ� �ӵ� ����
        }
    }

    IEnumerator ResotoreSpeed(Player player)
    {
        yield return new WaitForSeconds(slowDuration);
        player.RestoreMoveSpeed();
    }
}
