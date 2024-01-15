using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : RecycleObject
{
    // �Ŀ��� ������
    // �÷��׾ �Ծ��� ��
    // 1�ܰ� : 2���� ���� 
    // 2�ܰ� : 3���� ����
    // 3�ܰ� : ���ʽ� ���� 100+
    // �Ŀ��� �������� ������ �������� �����δ�.
    // ������ �������� �̵� ������ ����ȴ�.
    // ���� Ȯ���� �÷��̾� �ݴ��� ������ �����Ѵ�.

    public Player player;


    protected override void OnEnable()
    {
        base.OnEnable();
        onInitialze();
    }

    void onInitialze()
    {
        if (player = null)
        {
            player = GameManager.Instance.Player; // �÷��̾� ã��
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            player.PlayerPowerUp();
            gameObject.SetActive(false);
        }
    }
}
