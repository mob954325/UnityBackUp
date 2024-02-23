using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : RecycleObject
{

    //
    int hp = 5;
    public int HP
    {
        get => hp;
        set
        {
            hp = value;
            Debug.Log($"{gameObject.name} �� ü���� [{hp}] �Դϴ�");
        }
    }

    //

    /// <summary>
    /// ������ ���� �ð�
    /// </summary>
    public float phaseDuration = 0.5f;

    /// <summary>
    /// ������ ���� �ð�
    /// </summary>
    public float dissolveDuration = 1.0f;

    Renderer mainRanderer;

    /// <summary>
    /// �������� ���͸���
    /// </summary>
    Material mainMaterial;

    /// <summary>
    /// �ƿ������� ���� �� �β�
    /// </summary>
    const float visibleOutlineThickness = 0.004f;

    /// <summary>
    /// ����� ���� ���� �β�
    /// </summary>
    const float visiblePhaseThickness = 0.1f;

    /// <summary>
    /// ���̴� ������Ƽ ���̵��
    /// </summary>
    readonly int OutlineThicknessID = Shader.PropertyToID("_OutlineThickness");
    readonly int PhaseSplitID = Shader.PropertyToID("_PhaseSplit");
    readonly int PhaseThicknessID = Shader.PropertyToID("_PhaseThickness");
    readonly int DissolveFadeID = Shader.PropertyToID("_DissolveFade");

    Action onDissolveEnd;

    void Awake()
    {
        mainRanderer = GetComponent<Renderer>();
        mainMaterial = mainRanderer.material;

        onDissolveEnd += ReturnToPool;
    }



    protected override void OnEnable()
    {
        base.OnEnable();

        resetShaderProperty();
        StartCoroutine(StartPhase());
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    /// <summary>
    /// �������� ���� �� �����ϴ� �Լ�
    /// </summary>
    public void Die()
    {
        StartCoroutine(StartDissolve()); // �����길 ���� (������ �ڷ�ƾ���� ��Ȱ��ȭ���� ó����)
    }

    /// <summary>
    /// ��Ȱ��ȭ ��Ű�鼭 Ǯ�� ���������� �Լ�
    /// </summary>
    private void ReturnToPool()
    {
        gameObject.SetActive(false);
    }

    void resetShaderProperty()
    {
        // - ����
        ShowOutline(false);                             // �ƿ����� ����
        mainMaterial.SetFloat(PhaseThicknessID, 0);     // ������ ���Ⱥ��̰� �ϱ�
        mainMaterial.SetFloat(PhaseSplitID, 1);         // ���� ���̰� �ϱ�
        mainMaterial.SetFloat(DissolveFadeID, 1);       // ������ �Ⱥ��̰� �ϱ�
    }

    /// <summary>
    /// �ƿ����� �Ѱ� ���� �Լ�
    /// </summary>
    /// <param name="isShow">true�� ���̰� false�� ������ �ʴ´�.</param>
    public void ShowOutline(bool isShow = true)
    {
        // - Outline on/off
        if (isShow)
        {
            mainMaterial.SetFloat(OutlineThicknessID, visibleOutlineThickness); // ���̴� ���� �β��� ����
        }
        else
        {
            mainMaterial.SetFloat(OutlineThicknessID, 0); // �Ⱥ��̰� �ϴ� ���� �β��� 0���� ����
        }
    }

    /// <summary>
    /// ������ �����ϴ� �ڷ�ƾ (�Ⱥ��� -> ����)
    /// </summary>
    IEnumerator StartPhase()
    {
        // - PhaseReverse�� �Ⱥ��̴� ���¿��� ���̰� �����

        float phaseNormalize = 1.0f / phaseDuration;    // ������ �輱�� ���̱� ���� �̸� ���
        float timeElpased = 0.0f;                       // �ð� ������

        mainMaterial.SetFloat(PhaseThicknessID, visiblePhaseThickness); // ������ ���� ���̰� �����

        while (timeElpased < phaseDuration)  // �ð����࿡ ���� ó��
        {
            timeElpased += Time.deltaTime;  // �ð� ����

            mainMaterial.SetFloat(PhaseSplitID, 1 - (timeElpased * phaseNormalize)); // split ���� ������ �ð��� ���� ����

            yield return null;
        }

        mainMaterial.SetFloat(PhaseThicknessID, 0); // ������ �� �Ⱥ��̰� �ϱ�
        mainMaterial.SetFloat(PhaseSplitID, 0);     // float ���� ����
    }

    /// <summary>
    /// ������ �����ϴ� �ڷ�ƾ (�Ⱥ��� -> ����)
    /// </summary>
    IEnumerator StartDissolve()
    {
        // - Dissolve �����Ű��

        float dissolveNormalize = 1.0f / dissolveDuration;
        float timeElpased = 0.0f;

        while (timeElpased < dissolveDuration)
        {
            timeElpased += Time.deltaTime;

            mainMaterial.SetFloat(DissolveFadeID, 1 - (timeElpased * dissolveNormalize));

            yield return null;
        }

        mainMaterial.SetFloat(DissolveFadeID, 0);

        onDissolveEnd?.Invoke();
    }

#if UNITY_EDITOR
    public void TestShader(int index)
    {
        switch(index)
        {
            case 0:
                resetShaderProperty();
                break;
            case 1:
                ShowOutline(true);
                break;
            case 2:
                ShowOutline(false);
                break;
            case 3:
                StartCoroutine(StartPhase());
                break;
            case 4:
                StartCoroutine(StartDissolve());
                break;
        }
    }

    public void TestDie()
    {
        Die();
    }
#endif
}
