using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 오브젝트 풀을 사용하는 오브젝트의 종류
/// </summary>
public enum PoolObejctType
{ 
    PlayerBullet = 0,
    Hit,
    Explosion,
    EnemyWave,
    EnemyAstroid,
    EnemyAstroidMini,
    Power
}

public class Factory : Singleton<Factory>
{
    // 풀
    BulletPool bullet; // 플레이어 총알
    WavePool enemy; // 적
    BulletEffectPool bulletEffect; // 총알 터지는 이펙트
    ExplosionPool explosion; // 적 터지는 이펙트
    AsteroidPool asteroid;
    AsteroidMiniPool asteroidMini;
    PowerItemPool powerItem;

    /// <summary>
    /// 씬이 로딩 완료될 때마다 실행되는 초기화 함수
    /// </summary>
    protected override void OnInitialize()
    {
        base.OnInitialize();

        bullet = GetComponentInChildren<BulletPool>();
        if(bullet != null)
            bullet.Initialize();

        enemy = GetComponentInChildren<WavePool>();
        if (enemy != null)
            enemy.Initialize();

        bulletEffect = GetComponentInChildren<BulletEffectPool>();
        if (bulletEffect != null)
            bulletEffect.Initialize();

        explosion = GetComponentInChildren<ExplosionPool>();
        if (explosion != null)
            explosion.Initialize();

        asteroid = GetComponentInChildren<AsteroidPool>();
        if(asteroid != null) asteroid.Initialize();

        asteroidMini = GetComponentInChildren<AsteroidMiniPool>();
        if (asteroidMini != null) asteroidMini.Initialize();

        powerItem = GetComponentInChildren<PowerItemPool>();
        if (powerItem != null) powerItem.Initialize();
    }

    /// <summary>
    /// 풀에 있는 게임 오브젝트 하나 가져오기
    /// </summary>
    /// <param name="type">가져올 오브젝트의 종류</param>
    /// <param name="position">오브젝트의 종류</param>
    /// <returns>활성화 될 오브젝트</returns>
    public GameObject GetObject(PoolObejctType type, Vector3? position = null, Vector3? euler = null)
    {
        GameObject result;

        switch (type)
        {
            case PoolObejctType.PlayerBullet:
                result = bullet.GetObject(position, euler).gameObject;
                break;
            case PoolObejctType.Explosion:
                result = explosion.GetObject(position, euler).gameObject;
                break;
            case PoolObejctType.EnemyWave:
                result = enemy.GetObject(position, euler).gameObject;
                break;
            case PoolObejctType.Hit:
                result = bulletEffect.GetObject(position, euler).gameObject;
                break;
            case PoolObejctType.EnemyAstroid:
                result = asteroid.GetObject(position, euler).gameObject;
                break;       
            case PoolObejctType.EnemyAstroidMini:
                result = asteroidMini.GetObject(position, euler).gameObject;
                break;
            case PoolObejctType.Power:
                result = powerItem.GetObject(position, euler).gameObject;
                break;
            default:
                result = null;
                break;
        }
        return result;
    }

    /// <summary>
    /// 총알 하나를 가져오는 함수
    /// </summary>
    /// <returns>활성화 된 총알</returns>
    public Bullet GetBullet()
    {
        return bullet.GetObject();
    }

    /// <summary>
    /// 총알 하나를 가져오는 함수
    /// </summary>
    /// <param name="position">총알 위치</param>
    /// <returns>활성화 된 총알</returns>
    public Bullet GetBullet(Vector3 position, float angle = 0.0f)
    {
        return bullet.GetObject(position, angle * Vector3.forward);
    }
    public BulletEffect GetHitEffect()
    {
        return bulletEffect.GetObject();
    }

    public BulletEffect GetHitEffect(Vector3 position, float angle = 0.0f)
    {
        return bulletEffect.GetObject(position, angle * Vector3.forward);
    }

    public BulletEffect GetExplosion()
    {
        return explosion.GetObject();
    }
    public BulletEffect GetExplosion(Vector3 position, float angle = 0.0f)
    {
        return explosion.GetObject(position, angle * Vector3.forward);
    }

    public WaveEnemy GetEnemyWave()
    {
        return enemy.GetObject();
    }
    public WaveEnemy GetEnemyWave(Vector3 position, float angle = 0.0f)
    {
        return enemy.GetObject(position, angle * Vector3.forward);
    }

    public Asteroid GetAsteroid()
    {
        return asteroid.GetObject();
    }
    public Asteroid GetAsteroid(Vector3 position, float angle = 0.0f)
    {
        return asteroid.GetObject(position, angle * Vector3.forward);
    }
    public AsteroidMini GetAsteroidMini()
    {
        return asteroidMini.GetObject();
    }
    public AsteroidMini GetAsteroidMini(Vector3 position, float angle = 0.0f)
    {
        return asteroidMini.GetObject(position, angle * Vector3.forward);
    }

    public PowerUp GetPowerItem()
    {
        return powerItem.GetObject();
    }
    public PowerUp GetPowerItem(Vector3 position, float angle = 0.0f)
    {
        return powerItem.GetObject(position, angle * Vector3.forward);
    }
}
