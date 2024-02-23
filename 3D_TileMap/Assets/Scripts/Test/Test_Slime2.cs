using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Slime2 : TestBase
{
    /// <summary>
    /// �ڵ�� ����Ʈ�� ������ ���͸����� ���� ������
    /// </summary>
    public Renderer mainRanderer;

    /// <summary>
    /// �ڵ�� ������ ���͸���
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

    void Start()
    {
        mainMaterial = mainRanderer.material; // ���͸��� ��������
    }

    // 2. SlimeEffect �׽�Ʈ�ϱ�
    void resetShaderProperty()
    {
        // - ����
        ShowOutline(false);                             // �ƿ����� ����
        mainMaterial.SetFloat(PhaseThicknessID, 0);     // ������ ���Ⱥ��̰� �ϱ�
        mainMaterial.SetFloat(PhaseSplitID, 0);         // ���� ���̰� �ϱ�
        mainMaterial.SetFloat(DissolveFadeID, 1);       // ������ �Ⱥ��̰� �ϱ�
    }

    /// <summary>
    /// �ƿ����� �Ѱ� ���� �Լ�
    /// </summary>
    /// <param name="isShow">true�� ���̰� false�� ������ �ʴ´�.</param>
    void ShowOutline(bool isShow)
    {
        // - Outline on/off
        if(isShow)
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

        float phaseDuration = 0.5f;                     // ������ ���� �ð�
        float phaseNormalize = 1.0f / phaseDuration;    // ������ �輱�� ���̱� ���� �̸� ���
        float timeElpased = 0.0f;                       // �ð� ������

        mainMaterial.SetFloat(PhaseThicknessID, visiblePhaseThickness); // ������ ���� ���̰� �����

        while(timeElpased < phaseDuration)  // �ð����࿡ ���� ó��
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

        float dissolveDuration = 0.5f;
        float dissolveNormalize = 1.0f / dissolveDuration;
        float timeElpased = 0.0f;

        while (timeElpased < dissolveDuration)
        {
            timeElpased += Time.deltaTime;

            mainMaterial.SetFloat(DissolveFadeID, 1 - (timeElpased * dissolveNormalize));

            yield return null;
        }

        mainMaterial.SetFloat(DissolveFadeID, 0);
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        resetShaderProperty();
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        ShowOutline(true);
    }
    protected override void OnTest3(InputAction.CallbackContext context)
    {
        ShowOutline(false);
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        StartCoroutine(StartPhase());
    }

    protected override void OnTest5(InputAction.CallbackContext context)
    {
        StartCoroutine(StartDissolve());
    }

}
