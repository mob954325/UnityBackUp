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
    /// ��ǥ��
    /// </summary>
    int number = -1;

    public int Number
    {
        get => number;
        set
        {
            if (number != value)
            {
                number = Mathf.Min(value, 99999);                   // �ִ� 5�ڸ� ���� ����

                int temp = number;                                  // �ִ� 99999���� ��µȴ� (5�ڸ�)
                for (int i = 0; i < digits.Length; i++)
                {
                    if (temp != 0 || i == 0)                        // temp�� 0�� �ƴϸ� ó��
                    {
                        int digit = temp % 10;                      // 1�� �ڸ� ���� ó��
                        digits[i].sprite = numberImages[digit];     // ������ ���ڿ� �°� �̹��� ����
                        digits[i].gameObject.SetActive(true);
                    }
                    else
                    {
                        digits[i].gameObject.SetActive(false);      // temp�� 0�̸� �� �ڸ����� �Ⱥ��̰� ����� (1����)

                    }
                    temp /= 10;                                     // 1�ڸ� �� ����
                }
            }
        }
    }

    void Awake()
    {
        digits = GetComponentsInChildren<Image>();
    }
}
