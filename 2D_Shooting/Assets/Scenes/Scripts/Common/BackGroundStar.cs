using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundStar : Background
{
    SpriteRenderer[] spriteRenderer;

    protected override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponentsInChildren<SpriteRenderer>();
    }

    protected override void MoveRight(int index)
    {
        base.MoveRight(index);

        int rand = Random.Range(0, 4); // 0~3
        // 0(0b_00), 1(0b_01), 2(0b_10), 3(0b_11)

        spriteRenderer[index].flipX = (rand & 0b_01) != 0;
        // rand이 첫번째 비트가 1이면 true 아니면 false
        spriteRenderer[index].flipY = (rand & 0b_10) != 0;
        // rand이 두번째 비트가 1이면 true 아니면 false
    }

/*    void FlipStar(int index)
    {
    
        randomFlip = UnityEngine.Random.Range(0, 2);
        bool x = Convert.ToBoolean(randomFlip);
        randomFlip = UnityEngine.Random.Range(0, 2);
        bool y = Convert.ToBoolean(randomFlip);
    
        sprite[index].flipX = x;
        sprite[index].flipY = y;
    }*/

    //move right가 실행될때마다 spritrenderer의 splirt flip 랜덤설정
}
