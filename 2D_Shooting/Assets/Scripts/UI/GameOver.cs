using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    // player가 죽었을 때 보일것

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
