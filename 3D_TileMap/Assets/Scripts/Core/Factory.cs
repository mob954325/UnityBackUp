using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolObjectType
{
    Slime = 0
}

public class Factory : Singleton<Factory>
{
    public SlimePool slime;

    /// <summary>
    /// 초기화
    /// </summary>
    protected override void OnInitialize()
    {
        base.OnInitialize();

        slime = GetComponentInChildren<SlimePool>();
        if (slime != null) slime.Initialize();
    }

    /// <summary>
    /// 타입에 따라서 오브젝트 가져오기
    /// </summary>
    /// <param name="type">오브젝트 풀 타입</param>
    /// <param name="position">오브젝트 위치</param>
    /// <param name="euler">오브젝트 각도</param>
    /// <returns></returns>
    public GameObject GetObject(PoolObjectType type, Vector3? position = null, Vector3? euler = null)
    {
        GameObject result = null;
        switch (type)
        {
            case PoolObjectType.Slime:
                result = slime.GetObject(position, euler).gameObject;
                break;
        }

        return result;
    }

    /// <summary>
    /// 슬라임 생성하는 함수
    /// </summary>
    /// <return> 배치 될 슬라임 하나 </return>
    public Slime GetSlime()
    {
        return slime.GetObject();
    }

    /// <summary>
    /// 슬라임 생성하는 함수
    /// </summary>
    /// <param name="position">위치</param>
    /// <param name="angle">각도</param>
    public Slime GetSlime(Vector3 position, float angle = 0.0f)
    {
        return slime.GetObject(position, angle * Vector3.forward);
    }
}