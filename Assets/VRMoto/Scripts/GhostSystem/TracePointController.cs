using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracePointController : MonoBehaviour
{
    public bool IsCurrenTarget = false;
    public bool IsCurrentPassed = false;

    public Vector3 Position
    {
        get { return transform.position; }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = IsCurrenTarget ? Color.green : Color.yellow;
        Gizmos.color = IsCurrentPassed ? Color.blue : Gizmos.color;
        var size = IsCurrenTarget ? 10 : 5;
        Gizmos.DrawSphere(transform.position, size);
    }
}
