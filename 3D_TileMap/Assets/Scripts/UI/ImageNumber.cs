using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ImageNumber : MonoBehaviour
{
    public Sprite[] numberImages;
    Image[] digits;

    /// <summary>
    /// 목표값
    /// </summary>
    int number = -1;

    public int Number
    {
        get => number;
        set
        {
            if (number != value)
            {
                number = Mathf.Min(value, 99999);                   // 최대 5자리 숫자 설정

                int temp = number;                                  // 최대 99999까지 출력된다 (5자리)
                for (int i = 0; i < digits.Length; i++)
                {
                    if (temp != 0 || i == 0)                        // temp가 0이 아니면 처리
                    {
                        int digit = temp % 10;                      // 1의 자리 숫자 처리
                        digits[i].sprite = numberImages[digit];     // 추출한 숫자에 맞게 이미지 선택
                        digits[i].gameObject.SetActive(true);
                    }
                    else
                    {
                        digits[i].gameObject.SetActive(false);      // temp가 0이면 그 자리수는 안보이게 만들기 (1제외)

                    }
                    temp /= 10;                                     // 1자리 수 제거
                }
            }
        }
    }

    void Awake()
    {
        digits = GetComponentsInChildren<Image>();
    }
}
