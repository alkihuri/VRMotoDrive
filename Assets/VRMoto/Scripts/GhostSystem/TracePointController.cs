using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracePointController : MonoBehaviour
{
    public bool IsCurrent = false;

    public Vector3 Position
    {
        get { return transform.position; }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = IsCurrent ? Color.green : Color.yellow;
        var size = IsCurrent ? 10 : 5;
        Gizmos.DrawSphere(transform.position, size);
    }
}
