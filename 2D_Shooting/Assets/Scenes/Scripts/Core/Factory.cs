using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ������Ʈ Ǯ�� ����ϴ� ������Ʈ�� ����
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
    // Ǯ
    BulletPool bullet; // �÷��̾� �Ѿ�
    WavePool enemy; // ��
    BulletEffectPool bulletEffect; // �Ѿ� ������ ����Ʈ
    ExplosionPool explosion; // �� ������ ����Ʈ
    AsteroidPool asteroid;
    AsteroidMiniPool asteroidMini;
    PowerItemPool powerItem;

    /// <summary>
    /// ���� �ε� �Ϸ�� ������ ����Ǵ� �ʱ�ȭ �Լ�
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
    /// Ǯ�� �ִ� ���� ������Ʈ �ϳ� ��������
    /// </summary>
    /// <param name="type">������ ������Ʈ�� ����</param>
    /// <param name="position">������Ʈ�� ����</param>
    /// <returns>Ȱ��ȭ �� ������Ʈ</returns>
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
    /// �Ѿ� �ϳ��� �������� �Լ�
    /// </summary>
    /// <returns>Ȱ��ȭ �� �Ѿ�</returns>
    public Bullet GetBullet()
    {
        return bullet.GetObject();
    }

    /// <summary>
    /// �Ѿ� �ϳ��� �������� �Լ�
    /// </summary>
    /// <param name="position">�Ѿ� ��ġ</param>
    /// <returns>Ȱ��ȭ �� �Ѿ�</returns>
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
