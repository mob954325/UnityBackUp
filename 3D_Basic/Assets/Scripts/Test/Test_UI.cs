using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_UI : TestBase
{
    Player player;
    protected override void OnTest1(InputAction.CallbackContext context)
    {
        player = GameManager.Instance.Player;
        player.Die();
    }
}
