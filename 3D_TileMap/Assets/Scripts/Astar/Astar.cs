using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public static class Astar
{
    const float sideDistance = 1.0f;
    const float diagonalDistance = 1.0f;

    /// <summary>
    /// 맵의 시작점과 도착점을 받아 경로를 계산하는 함수 
    /// </summary>
    /// <param name="map">길을 찾을 맵 </param>
    /// <param name="start">시작점 </param>
    /// <param name="end">도착점 </param>
    /// <returns>시작점에서 도착점까지의 경로, 길을 못찾으면 null</returns>
    public static List<Vector2Int> PathFine(GridMap map, Vector2Int start, Vector2Int end)
    {
        List<Vector2Int> path = null;


        if (map.IsValidPosition(start) && map.IsValidPosition(end)
            && map.IsWall(start) && map.IsWall(end)) //  벽인지 아닌지 확인하면 맵 안인지 아닌지도 확인가능
        {
            // start와 end가 맵 안이고 벽이 아니다.

            map.ClearMapData();

            // Opnlist, closelist 추가
            //1.시작점을 open list에 추가한다
            List<Node> open = new List<Node>(); // open list : 앞으로 탐색할 노드의 리스트
            List<Node> close = new List<Node>(); // close list : 탄색이 완료된 노드의 리스트

            // a* 알고리즘 시작하기
            Node current = map.GetNode(start);
            current.G = 0.0f;
            current.H = GetHeuristic(current, end);
            open.Add(current);

            // A* 루프 시작(핵미 루틴)
            while(open.Count > 0)
            {
                //2.open list에 추가가 될 때는 f를 계산한다(g와 h도 계산이 되어야함)
                //3.open list f 값이 가장 작은 노드을 하나 선택한다
                //4.선택된 노드의 주변 노드를 openlist에 추가한다(못가는 노드와 closelist에 있는 노드는 하지 않음, g값이 이전보다 더 작은 경우는 갱신한다.)
                //5.선택한 노드는 close list에 들어간다.
                //6.선택된 노드가 도착점이 아니면 3번으로 돌아가 다시 실행한다
            }

            // 마무리 작업 (도착점에 도착했다. or 길을 못찾았다.)
            if(current == end)
            {
                // 경로 만들기
            }

        }

        return path;
    }

    /// <summary>
    /// 휴리스틱 함수 ( 현재 위치에서 목적지까지 예상거리)
    /// </summary>
    /// <param name="Current">현재노드 </param>
    /// <param name="end">도착지점 </param>
    /// <returns>예상거리 </returns>
    private static float GetHeuristic(Node Current, Vector2Int end)
    {
        return Mathf.Abs(Current.X - end.x) + Mathf.Abs(Current.Y - end.y);
    }
}
