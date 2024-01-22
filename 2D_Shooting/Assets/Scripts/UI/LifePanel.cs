using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifePanel : MonoBehaviour
{
    // �÷��̾� ����� ǥ��

    Player player;
    public Image[] images;

    public Color disableColor;

    void Awake()
    {
        player = GameManager.Instance.Player;

        images = new Image[transform.childCount];

        for(int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            images[i] = child.GetComponent<Image>();
        }
    }

    void OnEnable()
    {
        if(player != null)
        {
            player.onLifeChange += onLifeChange;
        }
    }

    void OnDisable()
    {
        player.onLifeChange -= onLifeChange;
    }

    private void onLifeChange(int life)
    {
        //images[life-1].color = new Color(0, 0, 0, 0.4f);
        // �÷��̾��� ���� ��ġ�� ���� ǥ�� ����
        // ���ư� ������ �������� ȸ������ ǥ��

        // �̹��� ������Ʈ�� ������ ������ �ϴ� ������Ƽ

        for(int i = 0; i < life; i++)
        {
            images[i].color = Color.white;
        }
        for(int i = life; i < images.Length; i++)
        {
            images[i].color = disableColor;
        }
    }
}
