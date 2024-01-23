using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    // player�� �׾��� �� ���ϰ�

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        GameManager.Instance.Player.onDie += (_) => animator.SetTrigger("GameOver"); 
    }
}
