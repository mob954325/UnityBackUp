using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Number : TestBase
{
    public ImageNumber imageNumber;
    public int number;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        imageNumber.Number = number;
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        Slime[] slimes = FindObjectsByType<Slime>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

        foreach (Slime slime in slimes)
        {
            slime.onDie();
        }
    }
}
