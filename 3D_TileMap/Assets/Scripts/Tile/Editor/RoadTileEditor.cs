using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(RoadTile))] // RoadTile�� Ŀ���� �����Ͷ�� ǥ���ϴ� ��Ʈ����Ʈ
public class RoadTileEditor : Editor
{
    /// <summary>
    /// ���õ� roadTile
    /// </summary>
    RoadTile roadtile;

    void OnEnable()
    {
        roadtile = target as RoadTile; // target�� Ŭ���� ����, target�� RoadTile���� Ȯ���ؼ� ĳ����
    }

    /// <summary>
    /// �ν����� â�� �׸��� �Լ�
    /// </summary>
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI(); // ���� �׸��� ��� �׸���

        // �߰��� �׸���
        if(roadtile != null) // roadTile�� �־����
        {
            if(roadtile.sprites != null) // sprites�� �־����
            {
                EditorGUILayout.LabelField("Sprites Image Preview");    // ���� ����

                Texture2D texture;
                for(int i = 0; i < roadtile.sprites.Length; i++)        // sprites�� �ִ� �̹����� �ϳ��� �׸���
                {
                    texture = AssetPreview.GetAssetPreview(roadtile.sprites[i]);    // sprite�� texture�� ����
                    if(texture != null)
                    {
                        GUILayout.Label("", GUILayout.Height(64), GUILayout.Width(64)); // 64*64ũ�� ���
                        GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);       // ũ�� ���� ���� �ؽ��� �׸���
                    }
                }
            }
        }
    }
}

#endif
