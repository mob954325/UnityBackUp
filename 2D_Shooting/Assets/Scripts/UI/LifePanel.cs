using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifePanel : MonoBehaviour
{
    // 플레이어 생명력 표시

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
        // 플레이어의 생명 수치에 따라 표시 변경
        // 날아간 생명은 반투명한 회색으로 표시

        // 이미지 컴포넌트의 색상을 변경을 하는 프로퍼티

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
