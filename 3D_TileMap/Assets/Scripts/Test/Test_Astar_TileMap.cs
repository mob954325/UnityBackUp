using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class Test_Astar_TileMap : TestBase
{
    public Tilemap background;
    public Tilemap obstacle;

    Vector2Int start;
    Vector2Int end;

    TileGridMap gridMap;

    public PathLine pathline;

    void Start()
    {
        gridMap = new TileGridMap(background, obstacle);
        pathline.ClearPath();
    }

    protected override void OnTestLClick(InputAction.CallbackContext _)
    {
        // Ÿ�ϸ��� �׸��� ��ǥ ���ϱ�
        Vector2 screenPosition = Mouse.current.position.ReadValue();
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition); // screen position -> world Position

        Vector2Int gridPosition = (Vector2Int)background.WorldToCell(worldPosition);

        //Debug.Log(gridPosition);

        if(!isWall(gridPosition))
            start = gridPosition;   

    }

    protected override void OnTestRClick(InputAction.CallbackContext _)
    {
        // Ŭ���� ��ġ�� Ÿ���� �ִ��� ������ Ȯ��

        Vector2 screenPosition = Mouse.current.position.ReadValue();
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition); // screen position -> world Position

        Vector2Int gridPosition = (Vector2Int)obstacle.WorldToCell(worldPosition);

        if(!isWall(gridPosition))
        {
            end= gridPosition;
            List<Vector2Int> path = AStar.PathFind(gridMap, start, end);
            PrintList(path);
            pathline.DrawPath(gridMap, path);
        }
    }

    /// <summary>
    /// ������ ��ġ�� Ÿ���� ������ ��, �ƴϸ� �� ����
    /// </summary>
    /// <param name="gridPosition">Ȯ���� ��ġ</param>
    /// <returns>true�� ��, false�� �� ��</returns>
    bool isWall(Vector2Int gridPosition)
    {
        TileBase tile = obstacle.GetTile((Vector3Int)gridPosition);
        return tile != null;
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        // background.size.x; // background�� ���ο� ����ִ� ���� ����(���α���), ������ �þ size�� ���� �ʴ´�.
        Debug.Log($"background {background.size.x} , {background.size.y}");
        Debug.Log($"obstacle {obstacle.size.x}, {obstacle.size.y}");
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        // background.origin : background�� �ִ� �� �߿� ���� �Ʒ��� 
        Debug.Log($"background origin : {background.origin}");
        Debug.Log($"obstacle origin : {obstacle.origin}");
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        // cellBounds.min = ���� ���ʾƷ��� ��ǥ
        // cellbounds.max = ���� ���������� ��ǥ
        Debug.Log($"background bound : {background.cellBounds.min} , {background.cellBounds.max}");
        Debug.Log($"obstacle bound : {obstacle.cellBounds.x}, {obstacle.cellBounds.y}");
    }
    
    void PrintList(List<Vector2Int> list)
    {
        string str = "";
        foreach (Vector2Int v in list)
        {
            str += $"{v} -> ";
        }
        Debug.Log(str + "end");
    }
}