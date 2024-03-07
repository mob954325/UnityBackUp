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
    /// 경로를 그리는 함수
    /// </summary>
    /// <param name="map">월드 좌표 계산용 맵</param>
    /// <param name="path">그리드 좌표로 구성된 경로</param>
    public void DrawPath(TileGridMap map, List<Vector2Int> path)
    {       
        if(map != null && path != null) // 맵과 경로가 둘다 있어야한다.
        {
            lineRenderer.positionCount = path.Count;    // 경로 개수 만큼 라인랜더러의 위치 추가

            int index = 0; 
            foreach(Vector2Int pos in path)             // list 순회
            {
                Vector2 wolrd = map.GridToWolrd(pos);   // 리스트에 있는 위치를 월드 좌표로 변경
                lineRenderer.SetPosition(index, wolrd); // 라인랜더러에 설정
                index++;                                // 인덱스 증가
            }
        }
        else
        {
            lineRenderer.positionCount = 0;
        }
    }

    /// <summary>
    /// 경로 초기화
    /// </summary>
    public void ClearPath()
    {
        if(lineRenderer != null)
        {
            lineRenderer.positionCount = 0; // 안보이게 만들기
        }
    }
}
