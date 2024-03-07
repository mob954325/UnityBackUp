using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathLine : MonoBehaviour
{
    LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    /// <summary>
    /// ��θ� �׸��� �Լ�
    /// </summary>
    /// <param name="map">���� ��ǥ ���� ��</param>
    /// <param name="path">�׸��� ��ǥ�� ������ ���</param>
    public void DrawPath(TileGridMap map, List<Vector2Int> path)
    {       
        if(map != null && path != null) // �ʰ� ��ΰ� �Ѵ� �־���Ѵ�.
        {
            lineRenderer.positionCount = path.Count;    // ��� ���� ��ŭ ���η������� ��ġ �߰�

            int index = 0; 
            foreach(Vector2Int pos in path)             // list ��ȸ
            {
                Vector2 wolrd = map.GridToWolrd(pos);   // ����Ʈ�� �ִ� ��ġ�� ���� ��ǥ�� ����
                lineRenderer.SetPosition(index, wolrd); // ���η������� ����
                index++;                                // �ε��� ����
            }
        }
        else
        {
            lineRenderer.positionCount = 0;
        }
    }

    /// <summary>
    /// ��� �ʱ�ȭ
    /// </summary>
    public void ClearPath()
    {
        if(lineRenderer != null)
        {
            lineRenderer.positionCount = 0; // �Ⱥ��̰� �����
        }
    }
}
