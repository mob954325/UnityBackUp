using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �� Ŭ����
/// </summary>
public class GridMap : MonoBehaviour
{
    /// <summary>
    /// �� �ʿ� �ִ� ��� ����
    /// </summary>
    Node[] nodes;

    /// <summary>
    /// ���� ���� ����
    /// </summary>
    int width;

    /// <summary>
    /// ���� ���� ����
    /// </summary>
    int height;

    /// <summary>
    /// ������
    /// </summary>
    /// <param name="width">���α���</param>
    /// <param name="height">���α���</param>
    public GridMap(int width, int height)
    {
        this.width = width;
        this.height = height;

        nodes = new Node[width * height]; // ��� �迭 ����

        for(int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (GridToIndex(x, y, out int? index))
                    nodes[index.Value] = new Node(x, y); // ��� ����
            }
        }
    }

    /// <summary>
    /// ��� ����� a* ���� ������ �ʱ�ȭ
    /// </summary>
    public void ClearMapData()
    {
        foreach(Node node in nodes)
        {
            node.ClearData();
        }

    }

    /// <summary>
    /// Ư�� ��ġ�� �ִ� ��� �����ϴ� �Լ�
    /// </summary>
    /// <param name="x">�ʿ����� x��</param>
    /// <param name="y">�ʿ����� y��</param>
    /// <returns></returns>
    public Node GetNode(int x, int y)
    {
        Node node = null;
        if( GridToIndex(x,y,out int? index) )
        {
            node = nodes[index.Value];
        }
        return node;
    }

    /// <summary>
    /// Ư�� ��ġ�� �ִ� ��� �����ϴ� �Լ�
    /// </summary>
    /// <param name="grid"></param>
    /// <returns></returns>
    public Node GetNode(Vector2Int grid)
    {
        return GetNode(grid.x, grid.y);
    }

    /// <summary>
    /// Ư�� ��ġ�� ������ Ȯ���ϴ� �Լ�
    /// </summary>
    /// <param name="x"><�ʿ����� x��ǥ/param>
    /// <param name="y">�ʿ����� y��ǥ</param>
    /// <returns></returns>
    public bool IsWall(int x, int y)
    {
        Node node = GetNode(x, y);
        return IsWall(x, y);
    }

    public bool IsWall(Vector2Int gridPosition)
    {
        return IsWall(gridPosition.x, gridPosition.y);
    }

    public bool IsSlime(int x, int y)
    {
        Node node = GetNode(x, y);
        return IsSlime(x, y);   
    }

    public bool IsSlime(Vector2Int gridPosition)
    {
        return IsSlime(gridPosition.x, gridPosition.y);
    }

    /// <summary>
    /// �׸��� ��ǥ�� �ε��������� �������ִ� �Լ�
    /// </summary>
    /// <param name="x">�ʿ����� x��ǥ</param>
    /// <param name="y">�ʿ����� y��ǥ</param>
    /// <param name="index">(��¿�) ����� �ε���</param>
    /// <returns>��ǥ�� �����ϸ� true, �� ���̸� false/returns>
    bool GridToIndex(int x, int y, out int? index)
    {
        bool result = false;
        index = null;

        if(IsValidPosition(x,y))
        {
            index = x + y * width;
            return true;
        }

        return result;
    }

    /// <summary>
    /// �ε��� ���� �׸��� ��ǥ�� �����ϴ� �Լ�
    /// </summary>
    /// <param name="index">������ index��</param>
    /// <returns>����� �׸��� ��ǥ</returns>
    Vector2Int IndexToGrid(int index)
    {
        return new Vector2Int(index % width, index / width);
    }

    /// <summary>
    /// Ư�� ��ġ�� �� ������ �ƴ��� Ȯ���ϴ� �Լ�
    /// </summary>
    /// <param name="x">Ȯ���� ��ǥ x</param>
    /// <param name="y">Ȯ���� ��ǥ y</param>
    /// <returns>true�� �ʾ�, false�� �� ��</returns>
    public bool IsValidPosition(int x, int y)
    {
        return x < width && y < height && x >= 0 && y >= 0;
    }

    /// <summary>
    /// Ư�� ��ġ�� �� ������ �ƴ��� Ȯ���ϴ� �Լ�
    /// </summary>
    /// <param name="gridPosition">Ȯ���� �׸��� ��ǥ</param>
    /// <returns>true �ʾ�, false �� ��</returns>
    public bool IsValidPosition(Vector2Int gridPosition)
    {
        return IsValidPosition(gridPosition.x, gridPosition.y);
    }
}
