using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavePool : ObjectPool<WaveEnemy>
{
    protected override void OnGetObject(WaveEnemy component)
    {
        component.SetStartPosition(component.transform.position);
    }
}
