using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        RecycleObject obj = collision.gameObject.GetComponent<RecycleObject>();

        if(obj != null)
        {
            collision.gameObject.SetActive(false);
        }
        else
        {

        }
    }
}
