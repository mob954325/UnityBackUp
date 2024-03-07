using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Seemless : TestBase
{
    [Range(0, 2)]
    public int targetX = 0;

    [Range(0, 2)]
    public int targetY = 0;

    WorldManager world;

    void Start()
    {
        world = GameManager.Instance.World;
    }
#if UNITY_EDITOR
    protected override void OnTest1(InputAction.CallbackContext context)
    {
        world.TestLoadScene(targetX, targetY);
    }
    protected override void OnTest2(InputAction.CallbackContext context)
    {
        world.TestUnLoadScene(targetX, targetY);
    }
    protected override void OnTest3(InputAction.CallbackContext context)
    {
        world.TestRefreshScenes(targetX, targetY);
    }
    protected override void OnTest4(InputAction.CallbackContext context)
    {
        for(int i = 0; i < 3; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                world.TestUnLoadScene(i, j);
            }
        }
    }
#endif
}
