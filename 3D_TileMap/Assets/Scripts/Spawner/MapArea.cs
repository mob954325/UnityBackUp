using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapArea : MonoBehaviour
{
    /// <summary>
    /// ���� ��� (��ü ũ��)�� Ÿ�� ��
    /// </summary>
    Tilemap background;
    /// <summary>
    /// ���� �� Ȯ�ο� Ÿ�� ��
    /// </summary>
    Tilemap obstacle;

    /// <summary>
    /// Ÿ�ϸ����� ������ �׸��� ��(A*��)
    /// </summary>
    TileGridMap gridMap;

    /// <summary>
    /// �׸��� Ȯ�ο� ������Ƽ
    /// </summary>
    public TileGridMap GridMap => gridMap;

    /// <summary>
    /// �� �� ������ �ִ� ��� ������
    /// </summary>
    Spawner[] spawners;

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        background = child.GetComponent<Tilemap>();             // ��� Ÿ�ϸ� ã��

        child = transform.GetChild(1);
        obstacle = child.GetComponent<Tilemap>();               // �� Ÿ�� �� ã��
            
        gridMap = new TileGridMap(background, obstacle);        // �׸���� ����

        child = transform.GetChild(2);
        spawners = child.GetComponentsInChildren<Spawner>();     // ��� ������ ã��
    }

    /// <summary>
    /// ���� ������ ������ �̸� ã�µ� ����ϴ� �Լ�
    /// </summary>
    /// <param name="spawner"></param>
    /// <returns></returns>
    public List<Node> CalcSpawnArea(Spawner spawner)
    {
        List<Node> result = new List<Node>();

        Vector2Int min = gridMap.WorldToGrid(spawner.transform.position);
        Vector2Int max = gridMap.WorldToGrid(spawner.transform.position + (Vector3)spawner.size);

        for(int y = min.y; y < max.y; y++)
        {
            for(int x = min.x; x < max.x; x++)
            {
                if(!gridMap.IsWall(x,y))
                {
                    result.Add(gridMap.GetNode(x, y));
                }
            }
        }

        return result;
    }

    /// <summary>
    /// ���� �׸��� ��ǥ�� ���� ��ǥ�� �������ִ� �Լ�
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public Vector2 GridToWorld(int x, int y)
    {
        return gridMap.GridToWolrd(new(x,y));
    }
}