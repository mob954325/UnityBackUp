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
        // 타일맵의 그리드 좌표 구하기
        Vector2 screenPosition = Mouse.current.position.ReadValue();
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition); // screen position -> world Position

        Vector2Int gridPosition = (Vector2Int)background.WorldToCell(worldPosition);

        //Debug.Log(gridPosition);

        if(!isWall(gridPosition))
            start = gridPosition;   

    }

    protected override void OnTestRClick(InputAction.CallbackContext _)
    {
        // 클릭한 위치에 타일이 있는지 없는지 확인

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
    /// 지정된 위치에 타일이 있으면 벽, 아니면 빈 구역
    /// </summary>
    /// <param name="gridPosition">확인할 위치</param>
    /// <returns>true면 벽, false면 빈 곳</returns>
    bool isWall(Vector2Int gridPosition)
    {
        TileBase tile = obstacle.GetTile((Vector3Int)gridPosition);
        return tile != null;
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        // background.size.x; // background의 가로에 들어있는 셀의 개수(가로길이), 지워도 늘어난 size는 줄지 않는다.
        Debug.Log($"background {background.size.x} , {background.size.y}");
        Debug.Log($"obstacle {obstacle.size.x}, {obstacle.size.y}");
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        // background.origin : background에 있는 셀 중에 왼쪽 아래가 
        Debug.Log($"background origin : {background.origin}");
        Debug.Log($"obstacle origin : {obstacle.origin}");
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        // cellBounds.min = 가장 왼쪽아래의 좌표
        // cellbounds.max = 가장 오른쪽위의 좌표
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