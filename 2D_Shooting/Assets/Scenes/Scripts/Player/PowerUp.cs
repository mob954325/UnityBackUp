using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : RecycleObject
{
    // 파워업 아이템
    // 플레잉어가 먹었을 때
    // 1단계 : 2개씩 나감 
    // 2단계 : 3개씩 나감
    // 3단계 : 보너스 점수 100+
    // 파워업 아이템은 랜덤한 방향으로 움직인다.
    // 일정한 간격으로 이동 방향이 번경된다.
    // 높은 확률로 플레이어 반대쪽 방향을 선택한다.

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
            player = GameManager.Instance.Player; // 플레이어 찾기
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
