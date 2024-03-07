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
// 1. �������� �������� �����ϸ� ���ο� �������� �������� �����Ѵ�
// (onDestinationArrive ��������Ʈ, GetRandomMoveablePosition �Լ� ���)
// 2. ����� ������ ���߿� �������� �ʴ´�.
