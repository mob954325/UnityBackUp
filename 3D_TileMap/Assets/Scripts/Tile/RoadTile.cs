using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class RoadTile : Tile
{
    [Flags]                     // �� enum�� bit flag�� ����Ѵٰ� ǥ���ϴ� ��Ʈ����Ʈ
    enum AdjTilePosition : byte // �� enum�� ũ�Ⱑ 1byte
    {
        None = 0,   // 0000 0000
        North = 1,  // 0000 0001
        East = 2,   // 0000 0010
        South = 4,  // 0000 0100
        West = 8,   // 0000 1000
        All = North | East | South | West // 0000 1111
    }

    /// <summary>
    /// ���� ������ ��������Ʈ
    /// </summary>
    public Sprite[] sprites;

    /// <summary>
    /// Ÿ���� �׷��� �� �ڵ����� ȣ���� �Ǵ� �Լ�
    /// </summary>
    /// <param name="position">Ÿ���� ��ġ(�׸��� ��ǥ)</param>
    /// <param name="tilemap">�� Ÿ���� �׷����� Ÿ�� ��</param>
    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        // �ֺ��� �ִ� ���� ������ Ÿ���� ����
        for (int y = -1; y < 2; y++)
        {
            for (int x = -1; x < 2; x++)
            {
                Vector3Int location = new Vector3Int(position.x + x, position.y + y, position.z);
                if(HasThisTile(tilemap, location))   //���� Ÿ������ Ȯ��
                {
                    tilemap.RefreshTile(location);  // �ֺ� Ÿ���� Refresh�Ѵ�. (���� Ÿ���̸� ����)
                }
            }
        }
    }

    /// <summary>
    /// Ÿ�ϸ��� RefreshTile�Լ��� ȣ��� �� ȣ��, Ÿ���� ��� ��������Ʈ�� �׸��� �����ϴ� �Լ�
    /// </summary>
    /// <param name="position">Ÿ�� �����͸� ������ Ÿ���� ��ġ</param>
    /// <param name="tilemap">Ÿ�� �����͸� ������ Ÿ�ϸ�</param>
    /// <param name="tileData">������ Ÿ�� �������� ����(�б� ���� �Ѵ� ����)</param>
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        // 4���� Ȯ��, Ȯ���� ������ ����
        AdjTilePosition mask = AdjTilePosition.None;
        mask |= HasThisTile(tilemap, position + new Vector3Int(0, 1, 0)) ? AdjTilePosition.North : 0;
        mask |= HasThisTile(tilemap, position + new Vector3Int(-1, 0, 0)) ? AdjTilePosition.East : 0;
        mask |= HasThisTile(tilemap, position + new Vector3Int(0, -1, 0)) ? AdjTilePosition.South : 0;
        mask |= HasThisTile(tilemap, position + new Vector3Int(1, 0, 0)) ? AdjTilePosition.West : 0;

        // �̹��� �����ϱ�
        int index = GetIndex(mask);
        if(index > -1 && index < sprites.Length) // �ε����� ����� ��������� Ȯ��
        {
            tileData.sprite = sprites[index];   // ��������Ʈ ����
            Matrix4x4 matrix = tileData.transform; // 4by4 ��� �޾ƿ���
            matrix.SetTRS(Vector3.zero, GetRotataion(mask), Vector3.one); // Ÿ�� ȸ�� ��Ű��
            tileData.transform = matrix;
            tileData.flags = TileFlags.LockTransform; // �ٸ� Ÿ���� ȸ���� ����Ű�� ��ױ�
        }
        else
        {
            Debug.LogError($"�߸��� �ε��� : {index}, mask = {mask}");
        }

    }

    /// <summary>
    /// Ư�� Ÿ�ϸ��� Ư�� ��ġ�� �� Ÿ�ϰ� ���� ������ Ÿ���� �ִ��� Ȯ���ϴ� �Լ�
    /// </summary>
    /// <param name="tilemap">Ȯ�� �� Ÿ�ϸ�</param>
    /// <param name="position">Ȯ�� �� ��ġ</param>
    /// <returns>true�� ���� ������ ����, false�� �ٸ������� Ÿ��</returns>
    bool HasThisTile(ITilemap tilemap, Vector3Int position)
    {
        return tilemap.GetTile(position) == this;
    }

    /// <summary>
    /// ����ũ ���� ���� �׸� ��������Ʈ�� �ε����� �����ϴ� �Լ�
    /// </summary>
    /// <param name="mask">�ֺ� Ÿ���� ��Ȳ�� ǥ���� ��Ʈ�÷��װ�</param>
    /// <returns>�׷����� ��������Ʈ�� �ε���</returns>
    int GetIndex(AdjTilePosition mask)
    {
        int index = -1;

        switch(mask)
        {
            case AdjTilePosition.None: // ����
            case AdjTilePosition.North: // ��
            case AdjTilePosition.East: // ��
            case AdjTilePosition.South: // ��
            case AdjTilePosition.West: // ��
            case AdjTilePosition.North | AdjTilePosition.South: // �ϳ�
            case AdjTilePosition.East | AdjTilePosition.West: // ����
                index = 0;// 1�� ���
                break;

            // ����
            case AdjTilePosition.South | AdjTilePosition.West:
            case AdjTilePosition.West | AdjTilePosition.North:
            case AdjTilePosition.North | AdjTilePosition.East:
            case AdjTilePosition.East | AdjTilePosition.South:
                index = 1;// ���� ���
                break;

            case AdjTilePosition.All & ~AdjTilePosition.North:    // 0000 1111 & ~(0000 0001) = 0000 1110
            case AdjTilePosition.All & ~AdjTilePosition.East:     // 0000 1111 & ~(0000 0010) = 0000 1101
            case AdjTilePosition.All & ~AdjTilePosition.South:    // 0000 1111 & ~(0000 0100) = 0000 1011
            case AdjTilePosition.All & ~AdjTilePosition.West:     // 0000 1111 & ~(0000 1000) = 0000 0111
                index = 2; // ���� ���                            // ~north : ~(0000 0001) = 1111 1110
                break;

            case AdjTilePosition.All:
                index = 3;  // +�� ����� ��������Ʈ
                break;
            }

        return index;
    }

    /// <summary>
    /// ����ũ ���� ���� ��������Ʈ�� �󸶳� ȸ����ų ������ �����ϴ� �Լ�
    /// </summary>
    /// <param name="mask">�ֺ� Ÿ�� ��Ȳ�� ����ϴ� ����ũ</param>
    /// <returns>���� ȸ��</returns>
    Quaternion GetRotataion(AdjTilePosition mask)
    {
        Quaternion rotate = Quaternion.identity;
        switch(mask)
        {
            // -90�� ������
            case AdjTilePosition.East:
            case AdjTilePosition.West:
            case AdjTilePosition.East | AdjTilePosition.West:   // 1��
            case AdjTilePosition.North | AdjTilePosition.West:  // ����
            case AdjTilePosition.All & ~AdjTilePosition.West:   // ��
                rotate = Quaternion.Euler(0, 0, -90);
                break;
                // -180�� ������
            case AdjTilePosition.North | AdjTilePosition.East:  // ��
            case AdjTilePosition.All & ~AdjTilePosition.North:  // ��
                rotate = Quaternion.Euler(0, 0, -180);
                break;
                // -270�� ������
            case AdjTilePosition.South | AdjTilePosition.East:   // ��
            case AdjTilePosition.All & ~AdjTilePosition.East:    // ��
                rotate = Quaternion.Euler(0, 0, -270);
                break;
        }

        return rotate;
    }

#if UNITY_EDITOR
    [MenuItem("Assets/Create/2D/Tiles/Custom/RoadTile")]
    public static void CreateRoadTile()
    {
        string path = EditorUtility.SaveFilePanelInProject( // ���� ����â�� ���� �Է°���� �����ִ� �Լ�
            "Save Road Tile",   // ����
            "New Road Tile",    // ����Ʈ ���ϸ�
            "Asset",            // Ǯ�� �޼���
            "Save Road Tile",   // ���� �⺻ ����
            "Assets/Tiles");
        if(path != null)
        {
            AssetDatabase.CreateAsset(CreateInstance<RoadTile>(), path); // road Tile�� ���Ϸ� ����
        }
    }

#endif
}
