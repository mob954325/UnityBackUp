using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 오브젝트 풀을 사용하는 오브젝트의 종류
/// </summary>
public enum PoolObjectType
{
    TurretBullet = 0,   // 플레이어의 총알
}

public class Factory : Singleton<Factory>
{
    BulletPool bullet;

    /// <summary>
    /// 초기화
    /// </summary>
    protected override void OnInitialize()
    {
        base.OnInitialize();

        bullet = GetComponentInChildren<BulletPool>();
        if (bullet != null)
            bullet.Initialize();
    }

    /// <summary>
    /// 풀에서 오브젝트 가져오기
    /// </summary>
    /// <param name="type">오브젝트 타입</param>
    /// <param name="position">위치</param>
    /// <param name="euler">각도</param>
    /// <returns></returns>
    public GameObject GetObject(PoolObjectType type, Vector3? position = null, Vector3? euler = null) 
    {
        GameObject result = null;

        switch(type)
        {
            case PoolObjectType.TurretBullet:
                result = bullet.GetObject(position, euler).gameObject;
                break;
        }

        return result;
    }
    
    // Get(Obejct) Functions
    public Bullet GetBullet()
    {
        return bullet.GetObject();
    }

    public Bullet GetBullet(Vector3 position, float angle = 0.0f)
    {
        return bullet.GetObject(position, angle * Vector3.forward);
    }
}
