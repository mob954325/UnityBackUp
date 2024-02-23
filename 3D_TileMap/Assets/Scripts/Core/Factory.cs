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
    /// �ʱ�ȭ
    /// </summary>
    protected override void OnInitialize()
    {
        base.OnInitialize();

        slime = GetComponentInChildren<SlimePool>();
        if (slime != null) slime.Initialize();
    }

    /// <summary>
    /// Ÿ�Կ� ���� ������Ʈ ��������
    /// </summary>
    /// <param name="type">������Ʈ Ǯ Ÿ��</param>
    /// <param name="position">������Ʈ ��ġ</param>
    /// <param name="euler">������Ʈ ����</param>
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
    /// ������ �����ϴ� �Լ�
    /// </summary>
    /// <return> ��ġ �� ������ �ϳ� </return>
    public Slime GetSlime()
    {
        return slime.GetObject();
    }

    /// <summary>
    /// ������ �����ϴ� �Լ�
    /// </summary>
    /// <param name="position">��ġ</param>
    /// <param name="angle">����</param>
    public Slime GetSlime(Vector3 position, float angle = 0.0f)
    {
        return slime.GetObject(position, angle * Vector3.forward);
    }
}