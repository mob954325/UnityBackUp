using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Slime : TestBase
{
    /// <summary>
    /// 슬라임들의 랜더러 (0: 아웃라인, 1:페이즈, 2:리버스페이즈, 3:이너라인, 4:디졸브)
    /// </summary>
    public Renderer[] Slimes;

    /// <summary>
    /// 슬라임들의 머터리얼
    /// </summary>
    Material[] materials;

    /// <summary>
    /// 쉐이퍼 프로퍼티 변경 속도
    /// </summary>
    public float speed = 1.5f;

    // 쉐이더 프로퍼티 변경 on/off용 변수
    public bool outlineThicknessChange = false;
    public bool phaseSplitChange = false;
    public bool phaseThicknessChange = false;
    public bool InnerLineThicknessChage = false;
    public bool DissolveFadeChage = false;


    /// <summary>
    /// 시간 누적용(삼각함수에 사용)
    /// </summary>
    float timeElapsed = 0.0f;

    /// <summary>
    /// split 정도(페이즈, 페이즈 리버스)
    /// </summary>
    [Range(0f,1f)]
    public float split = 0.0f;

    /// <summary>
    /// 페이즈류의 띠 두께
    /// </summary>
    [Range(0.1f, 0.5f)]
    public float phaseThickness = 0.01f;

    /// <summary>
    /// 아웃라인의 띠 두께
    /// </summary>
    [Range(0.0f, 0.01f)]
    public float outlineThickness = 0.005f;

    public float innerThickness = 0.05f;
    public float dissolveThickness = 0.35f;

    // 프로퍼티 ID를 숫자로 미리 변경
    readonly int SplitID = Shader.PropertyToID("_Split");
    readonly int ReverseSplitID = Shader.PropertyToID("_ReverseSplit");
    readonly int OutlineThicknessID = Shader.PropertyToID("_Thickness");
    readonly int PhaseThicknessID = Shader.PropertyToID("_PhaseThickness");
    readonly int ReverseThicknessID = Shader.PropertyToID("_ReverseThickness");
    readonly int InnerThicknessID = Shader.PropertyToID("_InnerThickness"); // 0 - 0.03
    readonly int DissolveFadeID = Shader.PropertyToID("_DissolveFade"); // 0 - 1


    void Start()
    {
        materials = new Material[Slimes.Length]; // 머터리얼 미리 찾아서 저장하기

        for(int i = 0; i < materials.Length; i++)
        {
            materials[i] = Slimes[i].material; // material 가져옴 -> 오브젝트의 개인 material이 됨 (instance) 
        }
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;
        float num = (Mathf.Cos(timeElapsed * speed) + 1.0f) * 0.5f; // 시간 변화에 따라 num값이 0-1로 계속 핑퐁된다.

        if(outlineThicknessChange)
        {
            float min = 0.0f;
            float max = 0.01f;

            float result = min + (max - min) * num; // num값에 따라 최소-최대로 변경

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

        // 아웃라인의 두깨를 변경해보기
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
        // InnerLine 두께 조정하기
        InnerLineThicknessChage = !InnerLineThicknessChage;
    }

    protected override void OnTest5(InputAction.CallbackContext context)
    {
        // Dissolve의 fade 조정하기
        DissolveFadeChage = !DissolveFadeChage;
    }
}
