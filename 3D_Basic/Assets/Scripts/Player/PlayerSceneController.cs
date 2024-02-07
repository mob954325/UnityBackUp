using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSceneController : MonoBehaviour
{
    public float cartSpeed = 20.0f;

    CinemachineVirtualCamera vCam;
    CinemachineDollyCart cart;
    Player player;

    void Awake()
    {
        vCam = GetComponentInChildren<CinemachineVirtualCamera>();
        cart = GetComponentInChildren<CinemachineDollyCart>();

    }

    void Start()
    {
        player = GameManager.Instance.Player;
        player.onDie += DeadSceneStart;
    }

    void Update()
    {
        transform.position = player.transform.position;    
    }

    void DeadSceneStart()
    {
        vCam.Priority = 100; // 이 가상카메라로 찍기 위해 우선 순위 높이기
        cart.m_Speed = cartSpeed;
    }

    // 플레이어 onDie 델리게이트에 함수를 연결

    // 이 오브젝트의 위치는 플레이어와 항상 같은 위치어야한다.
    // 자식으로 가상 카메라, 트랙, 카트가 있다.
    // 플레이어가 죽으면 카트가 움직이기 시작한다.
    // 플레이어가 죽으면 자식으로 가진 가상카메라의 우선순위가 가장높아진다.
    // 가상 카메라는 항상 플레이어를 바라본다
}
