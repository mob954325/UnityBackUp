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
            Debug.Log($"{gameObject.name} 의 체력은 [{hp}] 입니다");
        }
    }

    //

    /// <summary>
    /// 페이즈 진행 시간
    /// </summary>
    public float phaseDuration = 0.5f;

    /// <summary>
    /// 디졸브 진행 시간
    /// </summary>
    public float dissolveDuration = 1.0f;

    Renderer mainRanderer;

    /// <summary>
    /// 슬라임의 머터리얼
    /// </summary>
    Material mainMaterial;

    /// <summary>
    /// 아웃라인이 보일 때 두께
    /// </summary>
    const float visibleOutlineThickness = 0.004f;

    /// <summary>
    /// 페이즈가 보일 때의 두께
    /// </summary>
    const float visiblePhaseThickness = 0.1f;

    /// <summary>
    /// 쉐이더 프로퍼티 아이디들
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
    /// 슬라임이 죽을 때 실행하는 함수
    /// </summary>
    public void Die()
    {
        StartCoroutine(StartDissolve()); // 디졸브만 실행 (디졸브 코루틴에서 비활성화까지 처리함)
    }

    /// <summary>
    /// 비활성화 시키면서 풀로 돌려보내는 함수
    /// </summary>
    private void ReturnToPool()
    {
        gameObject.SetActive(false);
    }

    void resetShaderProperty()
    {
        // - 리셋
        ShowOutline(false);                             // 아웃라인 끄고
        mainMaterial.SetFloat(PhaseThicknessID, 0);     // 페이즈 선안보이게 하기
        mainMaterial.SetFloat(PhaseSplitID, 1);         // 전신 보이게 하기
        mainMaterial.SetFloat(DissolveFadeID, 1);       // 디졸브 안보이게 하기
    }

    /// <summary>
    /// 아웃라인 켜고 끄는 함수
    /// </summary>
    /// <param name="isShow">true면 보이고 false면 보이지 않는다.</param>
    public void ShowOutline(bool isShow = true)
    {
        // - Outline on/off
        if (isShow)
        {
            mainMaterial.SetFloat(OutlineThicknessID, visibleOutlineThickness); // 보이는 것은 두께를 설정
        }
        else
        {
            mainMaterial.SetFloat(OutlineThicknessID, 0); // 안보이게 하는 것은 두께를 0으로 설정
        }
    }

    /// <summary>
    /// 페이즈 진행하는 코루틴 (안보기 -> 보기)
    /// </summary>
    IEnumerator StartPhase()
    {
        // - PhaseReverse로 안보이는 상태에서 보이게 만들기

        float phaseNormalize = 1.0f / phaseDuration;    // 나누기 계선을 줄이기 위해 미리 계산
        float timeElpased = 0.0f;                       // 시간 누적용

        mainMaterial.SetFloat(PhaseThicknessID, visiblePhaseThickness); // 페이즈 선을 보이게 만들기

        while (timeElpased < phaseDuration)  // 시간진행에 따라 처리
        {
            timeElpased += Time.deltaTime;  // 시간 누적

            mainMaterial.SetFloat(PhaseSplitID, 1 - (timeElpased * phaseNormalize)); // split 값을 누적한 시간에 따라 변경

            yield return null;
        }

        mainMaterial.SetFloat(PhaseThicknessID, 0); // 페이즈 선 안보이게 하기
        mainMaterial.SetFloat(PhaseSplitID, 0);     // float 숫자 정리
    }

    /// <summary>
    /// 리졸브 진행하는 코루틴 (안보기 -> 보기)
    /// </summary>
    IEnumerator StartDissolve()
    {
        // - Dissolve 실행시키기

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
