using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSceneController : MonoBehaviour
{
    public float cartSpeed = 20.0f;

    CinemachineVirtualCamera vCam;
    CinemachineDollyCart cart;
    Player player;

    void Awake()
    {
        vCam = GetComponentInChildren<CinemachineVirtualCamera>();
        cart = GetComponentInChildren<CinemachineDollyCart>();

    }

    void Start()
    {
        player = GameManager.Instance.Player;
        player.onDie += DeadSceneStart;
    }

    void Update()
    {
        transform.position = player.transform.position;    
    }

    void DeadSceneStart()
    {
        vCam.Priority = 100; // �� ����ī�޶�� ��� ���� �켱 ���� ���̱�
        cart.m_Speed = cartSpeed;
    }

    // �÷��̾� onDie ��������Ʈ�� �Լ��� ����

    // �� ������Ʈ�� ��ġ�� �÷��̾�� �׻� ���� ��ġ����Ѵ�.
    // �ڽ����� ���� ī�޶�, Ʈ��, īƮ�� �ִ�.
    // �÷��̾ ������ īƮ�� �����̱� �����Ѵ�.
    // �÷��̾ ������ �ڽ����� ���� ����ī�޶��� �켱������ �����������.
    // ���� ī�޶�� �׻� �÷��̾ �ٶ󺻴�
}
