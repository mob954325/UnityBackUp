using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileGridMap : GridMap
{
    /// <summary>
    /// ���� ����
    /// </summary>
    Vector2Int origin;

    /// <summary>
    /// ��� Ÿ�ϸ� (ũ�� Ȯ�� �� ��ǥ����)
    /// </summary>
    Tilemap background;

    /// <summary>
    /// �̵� ������ ����(����)�� ��ġ ����
    /// </summary>
    Vector2Int[] moveablePositions;

    public TileGridMap(Tilemap backGround, Tilemap obstacle)
    {
        this.width = backGround.size.x;     // background Ÿ�ϸ��� ũ�⸦ ���μ��� ���̷� ����
        this.height = backGround.size.y;
        this.background = backGround;

        // background�� ����
        origin = (Vector2Int)backGround.origin;

        Vector2Int min = (Vector2Int)backGround.cellBounds.min;
        Vector2Int max = (Vector2Int)backGround.cellBounds.max;

        List<Vector2Int> moveable = new List<Vector2Int>(width * height); // �̵� ������ ������ ������ �ӽ� ����Ʈ

        nodes = new Node[width * height]; // ��� �迭 ����

        for(int y = min.y; y < max.y; y++)
        {
            for(int x = min.x; x < max.x; x++)
            {
                if(GridToIndex(x,y, out int? index))                // �ε����� ���ϱ�
                {
                    Node.NodeType nodeType = Node.NodeType.Plain;   // ��� Ÿ��
                    TileBase tile = obstacle.GetTile(new(x, y));
                    if(tile != null)
                    {
                        nodeType = Node.NodeType.Wall;              // ��ֹ� Ÿ���� �ִ� ���� ������ ����
                    }
                    else
                    {                                       
                        moveable.Add(new(x, y));                    // �̵� ������ ���� ����
                    }

                    nodes[index.Value] = new Node(x, y, nodeType);  // �ε��� ��ġ�� ��� ����
                }
            }
        }

        moveablePositions = moveable.ToArray(); 
    }

    /// <summary>
    /// �ε��� ����
    /// </summary>
    /// <param name="x">x ��ǥ</param>
    /// <param name="y">y ��ǥ</param>
    /// <returns>���� �ε��� ��ȯ ��</returns>
    protected override int CalcIndex(int x, int y)
    {
        // ���� ���� : x + y * width
        // ������ (0,0)�� �ƴ� �� ���� ó�� ((x - origin.x) + (y - origin.y)) * width;
        // �� �Ʒ��� ������ ���� ���� ó�� ((x - origin.x) + ((height - 1) - (y - origin.y))) * width
        return (x - origin.x) + ((height - 1) - (y - origin.y)) * width;
    }

    /// <summary>
    /// Ư�� ��ġ�� �� ������ �ƴ��� Ȯ���ϴ� �Լ�
    /// </summary>
    /// <param name="x">Ȯ���� x��ǥ</param>
    /// <param name="y">Ȯ���� y��ǥ</param>
    /// <returns>true�� �� ��, false�� �� ��</returns>
    public override bool IsValidPosition(int x, int y)
    {
        // ���� �� : x < width && y < height && x >= 0 && y >= 0;

        return x < (width + origin.x) && y < (height + origin.y) && x >= origin.x && y >= origin.y;
    }

    /// <summary>
    /// ���� ��ǥ�� �׸��� ��ǥ�� �����ϴ� �Լ�
    /// </summary>
    /// <param name="worldPosition">���� ��ǥ</param>
    /// <returns>��ȯ�� �׸��� ��ǥ</returns>
    public Vector2Int WorldToGrid(Vector3 worldPosition)
    {
        return (Vector2Int)background.WorldToCell(worldPosition);
    }

    /// <summary>
    /// �׸��� ��ǥ�� ���� ��ǥ�� �����ϴ� �Լ�
    /// </summary>
    /// <param name="gridPosition">�׸��� ��ǥ</param>
    /// <returns>�׸����� ��� ������ �ش��ϴ� ������ǥ</returns>
    public Vector2 GridToWolrd(Vector2Int gridPosition)
    {
        return background.CellToWorld((Vector3Int)gridPosition) + new Vector3(0.5f,0.5f);
    }

    /// <summary>
    /// �̵� ������ ��ġ �� �������� �����ؼ� �����ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public Vector2Int GetRandomMoveablePosition()
    {
        int index = UnityEngine.Random.Range(0, moveablePositions.Length);
        return moveablePositions[index];
    }

    /// <summary>
    /// ���� ��ǥ�� ���� �ش� ��ġ�� node�� ���ϴ� �Լ�
    /// </summary>
    /// <param name="worldPostion">Ȯ���� ��ġ(���� ��ǥ)</param>
    /// <returns></returns>
    public Node GetNode(Vector3 worldPostion)
    {
        return GetNode(WorldToGrid(worldPostion));
    }
}
