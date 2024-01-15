using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : EnemySpawner
{
    Transform destinationArea;

    void Awake()
    {        
        destinationArea = transform.GetChild(0);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        if(destinationArea == null)
        {
            destinationArea = transform.GetChild(0);
        }


        Gizmos.color = Color.yellow;
        Vector3 p0 = destinationArea.position + Vector3.up * MaxY;
        Vector3 p1 = destinationArea.position + Vector3.up * MinY;
        Gizmos.DrawLine(p0, p1);
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        if (destinationArea == null)
        {
            destinationArea = transform.GetChild(0);
        }

        Gizmos.color = Color.red;
        Vector3 p0 = transform.position + Vector3.up * MaxY + Vector3.right * 0.5f;
        Vector3 p1 = transform.position + Vector3.up * MinY + Vector3.right * 0.5f;
        Vector3 p2 = transform.position + Vector3.up * MaxY - Vector3.right;
        Vector3 p3 = transform.position + Vector3.up * MinY - Vector3.right;

        /// p2   p0
        ///
        /// p3   p1
        Gizmos.DrawLine(p2, p0);
        Gizmos.DrawLine(p0, p1);
        Gizmos.DrawLine(p1, p3);
        Gizmos.DrawLine(p3, p2);
    }

    protected override void Spawn()
    {
        Asteroid asteroid = Factory.Instance.GetAsteroid(GetSpawnPosition());
        asteroid.SetDestination(GetDestination());
    }

    Vector3 GetDestination()
    {
        Vector3 pos = destinationArea.position;
        pos.y += Random.Range(MinY, MaxY);

        return pos;
    }
}
