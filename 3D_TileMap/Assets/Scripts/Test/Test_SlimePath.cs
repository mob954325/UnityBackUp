using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class Test_SlimePath : TestBase
{
    public Tilemap background;
    public Tilemap obstacle;
    public Slime slime;

    TileGridMap map;

    void Start()
    {
        map = new TileGridMap(background, obstacle);
        slime.Initialized(map, new(1,1));
    }

    protected override void OnTestLClick(InputAction.CallbackContext context)
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector2 WorldPos = Camera.main.ScreenToWorldPoint(screenPos);

        Vector2Int gridPos = map.WorldToGrid(WorldPos);

        if(map.IsValidPosition(gridPos) && map.IsPlain(gridPos))
        {
            slime.SetDestination(gridPos);
        }
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        slime.SetDestination(map.GetRandomMoveablePosition());
    }
}
// 1. 슬라임이 목적지에 도착하면 새로운 목적지를 랜덤으로 설정한다
// (onDestinationArrive 델리게이트, GetRandomMoveablePosition 함수 사용)
// 2. 페이즈나 디졸브 도중에 움직이지 않는다.
