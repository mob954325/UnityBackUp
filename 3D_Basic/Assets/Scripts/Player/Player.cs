using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // �ǽ�
    // 1. �÷��̾�� WS�� ������
    // 2. �÷��̾�� AD�� ��ȸ�� ��ȸ��
    // 3. �÷��̾ �����̸� ����/����/��,��ȸ�� player_move �ִϸ��̼� ���
    // 4. �̵� �Է��� ������ player_idle �ִϸ��̼��� ���
    // 5. player_move �ִϸ��̼��� �ȴٸ��� �յڷ� ����
    // 6. player_idle �ִϸ��̼��� �Ӹ��� ��¦ �յڷ� ����Ÿ���.
    PlayerInput playerAction;
    void Awake()
    {
        playerAction = new PlayerInput();
        
    }
    void OnEnable()
    {
        playerAction.Player.Enable();
        playerAction.Player.Move.performed += OnMove;
    }

    private void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    void OnDisable()
    {
        playerAction.Player.Disable();
        
    }
}
