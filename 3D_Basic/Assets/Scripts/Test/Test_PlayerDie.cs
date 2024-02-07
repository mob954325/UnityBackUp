using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder.Shapes;

public class Test_PlayerDie : TestBase
{
    protected override void OnTest1(InputAction.CallbackContext context)
    {
        GameManager.Instance.Player.Die();
    }
}
