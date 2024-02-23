using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Slime : TestBase
{
    /// <summary>
    /// �����ӵ��� ������ (0: �ƿ�����, 1:������, 2:������������, 3:�̳ʶ���, 4:������)
    /// </summary>
    public Renderer[] Slimes;

    /// <summary>
    /// �����ӵ��� ���͸���
    /// </summary>
    Material[] materials;

    /// <summary>
    /// ������ ������Ƽ ���� �ӵ�
    /// </summary>
    public float speed = 1.5f;

    // ���̴� ������Ƽ ���� on/off�� ����
    public bool outlineThicknessChange = false;
    public bool phaseSplitChange = false;
    public bool phaseThicknessChange = false;
    public bool InnerLineThicknessChage = false;
    public bool DissolveFadeChage = false;


    /// <summary>
    /// �ð� ������(�ﰢ�Լ��� ���)
    /// </summary>
    float timeElapsed = 0.0f;

    /// <summary>
    /// split ����(������, ������ ������)
    /// </summary>
    [Range(0f,1f)]
    public float split = 0.0f;

    /// <summary>
    /// ��������� �� �β�
    /// </summary>
    [Range(0.1f, 0.5f)]
    public float phaseThickness = 0.01f;

    /// <summary>
    /// �ƿ������� �� �β�
    /// </summary>
    [Range(0.0f, 0.01f)]
    public float outlineThickness = 0.005f;

    public float innerThickness = 0.05f;
    public float dissolveThickness = 0.35f;

    // ������Ƽ ID�� ���ڷ� �̸� ����
    readonly int SplitID = Shader.PropertyToID("_Split");
    readonly int ReverseSplitID = Shader.PropertyToID("_ReverseSplit");
    readonly int OutlineThicknessID = Shader.PropertyToID("_Thickness");
    readonly int PhaseThicknessID = Shader.PropertyToID("_PhaseThickness");
    readonly int ReverseThicknessID = Shader.PropertyToID("_ReverseThickness");
    readonly int InnerThicknessID = Shader.PropertyToID("_InnerThickness"); // 0 - 0.03
    readonly int DissolveFadeID = Shader.PropertyToID("_DissolveFade"); // 0 - 1


    void Start()
    {
        materials = new Material[Slimes.Length]; // ���͸��� �̸� ã�Ƽ� �����ϱ�

        for(int i = 0; i < materials.Length; i++)
        {
            materials[i] = Slimes[i].material; // material ������ -> ������Ʈ�� ���� material�� �� (instance) 
        }
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;
        float num = (Mathf.Cos(timeElapsed * speed) + 1.0f) * 0.5f; // �ð� ��ȭ�� ���� num���� 0-1�� ��� �����ȴ�.

        if(outlineThicknessChange)
        {
            float min = 0.0f;
            float max = 0.01f;

            float result = min + (max - min) * num; // num���� ���� �ּ�-�ִ�� ����

            materials[0].SetFloat(OutlineThicknessID, result);
            outlineThickness = num;
        }
        if(phaseSplitChange)
        {
            materials[1].SetFloat(SplitID, num);
            materials[2].SetFloat(ReverseSplitID, num);
            split = num;
        }
        if(phaseThicknessChange)
        {
            float min = 0.0f;
            float max = 0.5f;

            float result = min + (max - min) * num;

            materials[1].SetFloat(PhaseThicknessID, result);
            materials[2].SetFloat(ReverseThicknessID, result);
            phaseThickness = num;
        }
        if(InnerLineThicknessChage)
        {
            float min = 0.0f;
            float max = 0.03f;

            float result = min + (max - min) * num;

            materials[3].SetFloat(InnerThicknessID, result);
            innerThickness = num;
        }
        if (DissolveFadeChage)
        {
            float min = 0.0f;
            float max = 1f;

            float result = min + (max - min) * num;

            materials[4].SetFloat(DissolveFadeID, result);
            dissolveThickness = num;
        }
    }


    protected override void OnTest1(InputAction.CallbackContext context)
    {
        //Renderer renderer = Slime.GetComponent<Renderer>();
        //Material material = renderer.material;
        //int id = Shader.PropertyToID("_Split");
        //material.SetFloat(id, split);

        // �ƿ������� �α��� �����غ���
        outlineThicknessChange = !outlineThicknessChange;
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        phaseSplitChange = !phaseSplitChange;
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        phaseThicknessChange = !phaseThicknessChange;
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        // InnerLine �β� �����ϱ�
        InnerLineThicknessChage = !InnerLineThicknessChage;
    }

    protected override void OnTest5(InputAction.CallbackContext context)
    {
        // Dissolve�� fade �����ϱ�
        DissolveFadeChage = !DissolveFadeChage;
    }
}
