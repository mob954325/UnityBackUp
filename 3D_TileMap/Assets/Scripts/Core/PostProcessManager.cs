using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessManager : MonoBehaviour
{
    /// <summary>
    /// ����Ʈ���μ����� ����Ǵ� ����
    /// </summary>
    Volume postProcessVolume;

    /// <summary>
    /// �����ȿ� �ִ� ���Ʈ�� ��ü
    /// </summary>
    Vignette vignette;

    FilmGrain grain;

    public AnimationCurve curve;

    void Awake()
    {
        postProcessVolume = GetComponent<Volume>();
        postProcessVolume.profile.TryGet<Vignette>(out vignette); // �������� ���Ʈ�� ���������� �õ�(������ NULL)
        postProcessVolume.profile.TryGet<FilmGrain>(out grain);
    }

    void Start()
    {
        Player player = GameManager.Instance.Player;
        player.onLifeTimeChange += OnLifeChange;
    }

    private void OnLifeChange(float ratio)
    {
        vignette.intensity.value = curve.Evaluate(ratio); // �׷����� y��
        grain.intensity.value = curve.Evaluate(ratio);
    }
}
