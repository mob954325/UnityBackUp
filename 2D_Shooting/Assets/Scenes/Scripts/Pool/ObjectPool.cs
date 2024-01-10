using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPool<T> : MonoBehaviour where T : RecycleObject
{ // where T : RecycleObject ->  RecycleObject��ӹ��� Ŭ������ �����ϴ�.
    /// <summary>
    /// Ǯ���� ������ ������Ʈ�� ������ 
    /// </summary>
    public GameObject originalPrefab;

    /// <summary>
    /// Ǯ�� ũ��, ó�� �����ϴ� ������Ʈ�� ����, ��� ������ 2^n�� ����
    /// </summary>
    public int poolSize = 64;

    /// <summary>
    /// TŸ������ ������ ������Ʈ�� �迭, ������ ��� ������Ʈ�� �ִ� �迭
    /// </summary>
    T[] pool;

    /// <summary>
    /// ���� ��밡����(��Ȱ��ȭ) ������Ʈ���� �����ϴ� ť
    /// </summary>
    Queue<T> readyQueue;

    void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        if(pool == null) // Ǯ�� ���� �ȸ������
        {
        pool = new T[poolSize]; // pool ������ ��ŭ TŸ�� �ʱ�ȭ (�����Ҵ�)
        readyQueue = new Queue<T>(poolSize); // readyQueue�� ����� capacity�� poolsize�� ����

            GenerateObject(0, poolSize, pool);
        }
        else
        {
            // Ǯ�� ������� (ex> addtional loading scene or scene restart)
            foreach(T obj in pool)
            {
                obj.gameObject.SetActive(false); // 
            }
        }
    }

    /// <summary>
    /// Ǯ���� ������� �ʴ� ������Ʈ�� �ϳ� ������ �����ϴ� �Լ�
    /// </summary>
    /// <returns>Ǯ���� ���� ������Ʈ(Ȱ��ȭ��)</returns>
    public T GetObject()
    {
        if(readyQueue.Count > 0) // check readyqueue 
        {
            T comp = readyQueue.Dequeue(); // ���������� �ϳ� ����
            comp.gameObject.SetActive(true); // Ȱ��ȭ
            return comp; // return
        }
        else
        {
            // readyqueue is empty == no object
            ExpandPool(); // expandPool twice
            return GetObject(); // turn self
        }
    }

    /// <summary>
    /// Ǯ�� �ι�� �ø��� �Լ�
    /// </summary>
    void ExpandPool()
    {
        // �ִ��� �Ͼ�� �ȵǴ� �Լ� -> ���ǥ��
        Debug.LogWarning($"{gameObject.name} Ǯ ������ ����. {poolSize} -> {poolSize * 2}");
        int newSize = poolSize * 2;
        T[] newPool = new T[newSize];

        for(int i = 0; i< poolSize; i++) // ���� Ǯ�� �ִ��� ��������
        {
            newPool[i] = pool[i];
        }

        GenerateObject(poolSize, newSize, newPool);
        pool = newPool; // Ǯ �缳��
        poolSize = newSize; // ������ �缳�� 
    }

    /// <summary>
    /// Ǯ���� ����� ������Ʈ�� �����ϴ� ����
    /// </summary>
    /// <param name="start">���� ������ ������ �ε���</param>
    /// <param name="end">���� ������ ������ �ε���</param>
    /// <param name="result">������ ������Ʈ�� �� �迭</param>
    /// <returns></returns>
    void GenerateObject(int start, int end, T[] result)
    {
        for(int i = start; i < end; i++)
        {
            GameObject obj = Instantiate(originalPrefab, transform); // instantiate
            obj.name = $"{originalPrefab.name}_{i}";

            T comp = obj.GetComponent<T>();
            comp.onDisable += () => readyQueue.Enqueue(comp); // ��Ȱ�� ������Ʈ�� ��Ȱ��ȭ �Ǹ� ����ť�� �ǵ�����
            //readyQueue.Enqueue(comp); // add to readyqueue

            result[i] = comp; // add to array
            obj.SetActive(false); // ��Ȱ��ȭ
        }
    }
}
