using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldManager : MonoBehaviour
{
    /// <summary>
    /// ���� ���� ����
    /// </summary>
    const int HeightCount = 3;

    /// <summary>
    /// ���� ���� ����
    /// </summary>
    const int WidthCount = 3;

    /// <summary>
    /// �� �ϳ��� ���� ����
    /// </summary>
    const float mapHeightSize = 20.0f;

    /// <summary>
    /// �� �ϳ��� ���� ����
    /// </summary>
    const float mapWidthSize = 20.0f;

    /// <summary>
    /// ������ ���� ( ��� ���� ������ �� ���� �Ʒ� ������ ���� )
    /// </summary>
    readonly Vector2 worldOrigin = new Vector3(-mapWidthSize * WidthCount * 0.5f
                                              ,-mapHeightSize * HeightCount * 0.5f);

    /// <summary>
    /// �� �̸� ���տ� �⺻ �̸�
    /// </summary>
    const string SceneNameBase = "Seemless";

    /// <summary>
    /// ��� ���� �̸��� ������ �迭
    /// </summary>
    string[] sceneNames;

    /// <summary>
    /// �� �ε� ���� Enum
    /// </summary>
    enum SceneLoadState : byte
    { 
        Unload = 0,     // �ε��� �ȵǾ��ִ� ���� ( �����Ǿ� �ִ� ���� )
        PendingUnload,  // �ε� ���� �������� ����
        PendingLoad,    // �ε� �������� ����
        Loaded          // �ε� �Ϸ�� ����
    }

    /// <summary>
    /// ��� ���� �ε� ����
    /// </summary>
    SceneLoadState[] sceneLoadState;

    /// <summary>
    /// ��� ���� ��ε� �Ǿ����� Ȯ���ϱ� ���� ������Ƽ(��� ���� Unload ���¸� true, �ƴϸ� false)
    /// </summary>
    public bool IsUnloadAll
    {
        get
        {
            bool result = true;
            foreach(SceneLoadState state in sceneLoadState)
            {
                if(state != SceneLoadState.Unload) // �ϳ��� Unload�� �ƴϸ� false
                {
                    result = false;
                    break;
                }
            }
            return result;
        }
    }

    /// <summary>
    /// �ε� ��û�� ���� ���� ���
    /// </summary>
    [SerializeField] List<int> loadWork = new List<int>();

    /// <summary>
    /// �ε��� �Ϸ�� ���� ���
    /// </summary>
    [SerializeField] List<int> loadWorkComplete = new List<int>();

    /// <summary>
    /// �ε� ���� ��û�� ���� ���� ���
    /// </summary>
    [SerializeField] List<int> unloadWork = new List<int>();

    /// <summary>
    /// �ε� ������ �Ϸ�� ���� ���
    /// </summary>
    [SerializeField] List<int> unloadWorkComplete = new List<int>();

    /// <summary>
    /// ó�� ��������� �� �ѹ��� �����ϴ� �Լ�
    /// </summary>
    public void PreInitialize()
    {
        int mapCount = HeightCount * WidthCount;
        sceneNames = new string[mapCount];
        sceneLoadState = new SceneLoadState[mapCount];

        for (int y = 0; y < HeightCount; y++)
        {
            for(int x = 0; x < WidthCount; x++)
            {
                int index = GetIndex(x, y);
                sceneNames[index] = $"{SceneNameBase}_{x}_{y}";
                sceneLoadState[index] = SceneLoadState.Unload;
            }
        }
    }

    /// <summary>
    /// ���� single�� �ε�� ������ ȣ��� �ʱ�ȭ�� �Լ�
    /// </summary>
    public void Initialize()
    {
        // �� �ε� �ʱ�ȭ
        for(int i = 0; i < sceneLoadState.Length; i++)
        {
            sceneLoadState[i] = SceneLoadState.Unload;
        }

        // �÷��̾� ���� ó��
        Player player = GameManager.Instance.Player;
        if(player != null)
        {
            player.onMapChange += (currentGrid) =>
            {
                // �ʿ��� ��ġ�� �ٲ�� �ֺ� �� ����
                RefreshScenes(currentGrid.x, currentGrid.y);
            };

            // �÷��̾ ������ ��� ���� �ε����� ��û�Ѵ�.
            player.onDie += (_, _) =>
            {
                for(int y =0; y < HeightCount; y++)
                {
                    for(int x = 0; x < WidthCount; x++)
                    {
                        RequestAsyncSceneUnLoad(x,y);
                    }
                }
            };

            Vector2Int grid = WorldToGrid(player.transform.position);
            RequestAsyncSceneLoad(grid.x, grid.y);  // �÷��̾ �ִ� ���� ��켱���� �ε� ��û
            RefreshScenes(grid.x, grid.y);          // �ֺ� �� �ε� ��û
        }
    }

    /// <summary>
    /// ���� �׸��� ��ġ�� �ε����� �������ִ� �Լ�
    /// </summary>
    /// <param name="x">���� x��ġ</param>
    /// <param name="y">���� y��ġ</param>
    /// <returns>�迭�� Index��</returns>
    int GetIndex(int x, int y)
    {
        return y * WidthCount + x;
    }

    /// <summary>
    /// �񵿱� �ε� ��û �Լ�
    /// </summary>
    /// <param name="x">�ε��� ���� x��ġ</param>
    /// <param name="y">�ε��� ���� y��ġ</param>
    void RequestAsyncSceneLoad(int x, int y)
    {
        int index = GetIndex(x, y);     // �ε��� ���
        if (sceneLoadState[index] == SceneLoadState.Unload) // Unload�� ����
        {
            loadWork.Add(index);        // Unload�� ���� �ε� ����Ʈ�� �߰�
        }
    }

    /// <summary>
    /// ���� �񵿱�� �ε��ϴ� �Լ� (Additive �ε�)
    /// </summary>
    /// <param name="index">�ε��� ���� �ε���</param>
    void AsyncSceneLoad(int index)
    {
        if (sceneLoadState[index] == SceneLoadState.Unload)         // Unload ������ �ʸ� ó��
        {   
            sceneLoadState[index] = SceneLoadState.PendingLoad;     // panding ���·� ���� ���� ���̶�� ǥ��

            AsyncOperation async = SceneManager.LoadSceneAsync(sceneNames[index], LoadSceneMode.Additive);   // �񵿱� �ε� ����
            async.completed += (_) =>   // �񵿱� �۾��� ������ �� ����Ǵ� ��������Ʈ�� ���� �Լ� �߰�
            {
                sceneLoadState[index] = SceneLoadState.Loaded;      // Load ���·� ����
                loadWorkComplete.Add(index);                        // �Ϸ� ��Ͽ� �߰�
            };
        }
    }

    /// <summary>
    /// �񵿱� ���� �ε� ��û �Լ�
    /// </summary>
    /// <param name="x">�ε������� ���� x��ġ</param>
    /// <param name="y">�ε������� ���� y��ġ</param>
    void RequestAsyncSceneUnLoad(int x, int y)
    {
        int index = GetIndex(x, y);         // �ε��� ���
        if (sceneLoadState[index] == SceneLoadState.Loaded)
        {   
            unloadWork.Add(index);          // �ε� �Ϸ�Ǿ����� ���� unload ����Ʈ�� �߰�
        }

    }

    /// <summary>
    /// �񵿱� �ε� ������ ó���ϴ� �Լ�
    /// </summary>
    /// <param name="index">�ε� ������ ���� �ε���</param>
    void AsyncSceneUnLoad(int index)
    {
        if(sceneLoadState[index] == SceneLoadState.Loaded)          // �ε��� �Ϸ�� �ʸ� ó��
        {
            sceneLoadState[index] = SceneLoadState.PendingUnload;   // �ε� �������̶�� ǥ��

            // �ʿ� �ִ� �������� Ǯ�� �ǵ����� ( �� ��ε�� �����Ǵ� ���� ���� )
            Scene scene = SceneManager.GetSceneByName(sceneNames[index]);   // �� ã��
            if(scene.isLoaded)  // ���� �ε� �Ǿ� ������
            {
                GameObject[] sceneRoots = scene.GetRootGameObjects();       // ��Ʈ ������Ʈ ��� ã��

                if(sceneRoots != null && sceneRoots.Length > 0)             // ��Ʈ ������Ʈ�� 1�� �̻�������
                {
                    Slime[] slimes = sceneRoots[0].GetComponentsInChildren<Slime>();    // �������� ��� ã�Ƽ�
                    foreach(Slime slime in slimes)
                    {
                        slime.ReturnToPool();                               // Ǯ�� �ǵ�����
                    }
                }
            }

            AsyncOperation async = SceneManager.UnloadSceneAsync(sceneNames[index]);    // �񵿱� �ε� ���� ����
            async.completed += (_) =>
            {
                sceneLoadState[index] = SceneLoadState.Unload;  // unload�� ǥ��
                unloadWorkComplete.Add(index);                  // �Ϸ� ����Ʈ�� �߰�
            };
        }
    }

    void Update()
    {
        // �Ϸ�� �۾� ����Ʈ���� ����
        foreach(var index in loadWorkComplete)
        {
            loadWork.RemoveAll((x) => x == index); // loadWork����Ʈ���� index�� ���� �������� ��� ����
        }
        loadWorkComplete.Clear();

        // ���� ��û ó��
        foreach(var index in loadWork)
        {
            AsyncSceneLoad(index); // �񵿱� �ε� ����
        }

        // �Ϸ�� �۾� ����Ʈ���� ����
        foreach (var index in unloadWorkComplete)
        {
            unloadWork.RemoveAll((x) => x == index); // unloadWork����Ʈ���� index�� ���� �������� ��� ����
        }
        loadWorkComplete.Clear();

        // ���� ��ûó��
        foreach (var index in unloadWork)
        {
            AsyncSceneUnLoad(index); // �񵿱� �ε� ���� ���� (������ �ǵ�����)
        }
    }

    /// <summary>
    /// ������ ��ġ �ֺ� ���� �ε� ��û�ϰ� �� �ܴ� �ε� ������ ��û�ϴ� �Լ�
    /// </summary>
    /// <param name="mapX">������ ���� x ��ġ</param>
    /// <param name="y">������ ���� y ��ġ</param>
    void RefreshScenes(int mapX, int mapY)
    {
        // ��ü ���� : (0,0) ~ (2,2)
        // (x,y) �ֺ��� RequsetAsyncSceneLoad ����
        // ������ �κ��� ��� ReQuestAsyncSceneUnload ����

        int startX = Mathf.Max(0, mapX - 1);            // �ּ����� (mapX, mapY)���� 1�۰ų� (0,0); 
        int startY = Mathf.Max(0, mapY - 1);
        int endX = Mathf.Min(WidthCount, mapX + 2);     // �ִ����� (mapX, mapY)���� 1ũ�ų� (2,2);
        int endY = Mathf.Min(HeightCount, mapY + 2);

        List<Vector2Int> open = new List<Vector2Int>(9);
        for(int y = startY; y < endY; y++)
        {
            for(int x= startX; x < endX; x++)
            {
                RequestAsyncSceneLoad(x, y);        // �ش��ϴ� �͵� �ε� ��û
                open.Add(new Vector2Int(x, y));     // �ε� ��û �� �͵� ���
            }
        }

        for(int y = 0; y < HeightCount; y++)
        {
            for(int x = 0; x< WidthCount; x++)
            {
                // Contains : �ܼ��� �ִ� ���� Ȯ��
                // Exits : Ư�� ������ �����ϴ� ���� �ִ��� ���� �� Ȯ��
                if( !open.Contains(new Vector2Int(x,y)) )
                {
                    RequestAsyncSceneUnLoad(x, y);
                }
            }
        }
    }

    /// <summary>
    /// ���� ��ǥ�� � �ʿ� ���ϴ��� ����ϴ� �Լ�
    /// </summary>
    /// <param name="worldPosition">Ȯ���� ���� ��ǥ</param>
    /// <returns>���� ��ǥ (0,0) ~ (2,2)</returns>
    public Vector2Int WorldToGrid(Vector3 worldPosition)
    {
        Vector2 offset = (Vector2)worldPosition - worldOrigin;

        return new Vector2Int((int)(offset.x/mapWidthSize), (int)(offset.y/mapHeightSize));
    }

#if UNITY_EDITOR

    public void TestLoadScene(int x, int y)
    {
        RequestAsyncSceneLoad(x, y);
    }

    public void TestUnLoadScene(int x, int y)
    {
        RequestAsyncSceneUnLoad(x, y);
    }

    public void TestRefreshScenes(int x, int y)
    {
        RefreshScenes(x, y);
    }
#endif
}
