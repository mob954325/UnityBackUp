using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeTimeGauge : MonoBehaviour
{
    Slider slider;
    Image fill;

    //public Color StartColor = Color.white;
    //public Color EndColor = Color.red;
    public Gradient color;
    public AnimationCurve curve;

    void Awake()
    {
        slider = GetComponent<Slider>();
        slider.value = 1;

        Transform child = transform.GetChild(1);
        child = child.GetChild(0);
        fill = child.GetComponent<Image>();
    }
    void Start()
    {
        GameManager.Instance.Player.onLifeTimeChange += (value) => LifeGaugeChange(value);
    }

    void LifeGaugeChange(float ratio)
    {
        slider.value = ratio;

        //fill.color = Color.Lerp(StartColor, EndColor, ratio);
        fill.color = color.Evaluate(ratio);
    }
}
