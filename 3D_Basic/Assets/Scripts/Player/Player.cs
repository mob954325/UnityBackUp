using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // 실습
    // 1. 플레이어는 WS로 전후진
    // 2. 플레이어는 AD로 좌회전 후회전
    // 3. 플레이어가 움직이면 전진/후진/좌,우회전 player_move 애니메이션 재생
    // 4. 이동 입력이 없으면 player_idle 애니메이션이 재생
    // 5. player_move 애니메이션은 팔다리가 앞뒤로 흔들기
    // 6. player_idle 애니메이션은 머리가 살짝 앞뒤로 까딱거린다.
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
