using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public static class Astar
{
    const float sideDistance = 1.0f;
    const float diagonalDistance = 1.0f;

    /// <summary>
    /// ���� �������� �������� �޾� ��θ� ����ϴ� �Լ� 
    /// </summary>
    /// <param name="map">���� ã�� �� </param>
    /// <param name="start">������ </param>
    /// <param name="end">������ </param>
    /// <returns>���������� ������������ ���, ���� ��ã���� null</returns>
    public static List<Vector2Int> PathFine(GridMap map, Vector2Int start, Vector2Int end)
    {
        List<Vector2Int> path = null;


        if (map.IsValidPosition(start) && map.IsValidPosition(end)
            && map.IsWall(start) && map.IsWall(end)) //  ������ �ƴ��� Ȯ���ϸ� �� ������ �ƴ����� Ȯ�ΰ���
        {
            // start�� end�� �� ���̰� ���� �ƴϴ�.

            map.ClearMapData();

            // Opnlist, closelist �߰�
            //1.�������� open list�� �߰��Ѵ�
            List<Node> open = new List<Node>(); // open list : ������ Ž���� ����� ����Ʈ
            List<Node> close = new List<Node>(); // close list : ź���� �Ϸ�� ����� ����Ʈ

            // a* �˰��� �����ϱ�
            Node current = map.GetNode(start);
            current.G = 0.0f;
            current.H = GetHeuristic(current, end);
            open.Add(current);

            // A* ���� ����(�ٹ� ��ƾ)
            while(open.Count > 0)
            {
                //2.open list�� �߰��� �� ���� f�� ����Ѵ�(g�� h�� ����� �Ǿ����)
                //3.open list f ���� ���� ���� ����� �ϳ� �����Ѵ�
                //4.���õ� ����� �ֺ� ��带 openlist�� �߰��Ѵ�(������ ���� closelist�� �ִ� ���� ���� ����, g���� �������� �� ���� ���� �����Ѵ�.)
                //5.������ ���� close list�� ����.
                //6.���õ� ��尡 �������� �ƴϸ� 3������ ���ư� �ٽ� �����Ѵ�
            }

            // ������ �۾� (�������� �����ߴ�. or ���� ��ã�Ҵ�.)
            if(current == end)
            {
                // ��� �����
            }

        }

        return path;
    }

    /// <summary>
    /// �޸���ƽ �Լ� ( ���� ��ġ���� ���������� ����Ÿ�)
    /// </summary>
    /// <param name="Current">������ </param>
    /// <param name="end">�������� </param>
    /// <returns>����Ÿ� </returns>
    private static float GetHeuristic(Node Current, Vector2Int end)
    {
        return Mathf.Abs(Current.X - end.x) + Mathf.Abs(Current.Y - end.y);
    }
}
