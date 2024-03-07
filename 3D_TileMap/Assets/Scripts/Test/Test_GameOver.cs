using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_GameOver : TestBase
{
#if UNITY_EDITOR
    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Player player = GameManager.Instance.Player;
        player.Test_Die();
    }
#endif
}
