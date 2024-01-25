using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectRandom : MonoBehaviour
{
    public bool reroll = true;

    void Awake()
    {
            Randmize();
    }
    private void OnValidate()
    {

            Randmize();
            reroll = false;
    }

    void Randmize()
    {
        if (reroll)
        {
            transform.localScale = new Vector3(
                1 + Random.Range(-0.15f, 0.15f),
                1 + Random.Range(-0.15f, 0.15f),
                1 + Random.Range(-0.15f, 0.15f));

            transform.Rotate(0, Random.Range(0, 360f), 0);
            reroll = false;
        }
    }
}
