using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracePointController : MonoBehaviour
{
    [SerializeField] bool _isFinish;
    [SerializeField] bool _isStart;


    public bool IsCurrenTarget = false;
    public bool IsCurrentPassed = false;

    public Vector3 Position
    {
        get { return transform.position; }
    }

    public bool IsFinish { get => _isFinish; set => _isFinish = value; }
    public bool IsStart { get => _isStart; set => _isStart = value; }

    private void OnDrawGizmos()
    {
        Gizmos.color = IsCurrenTarget ? Color.green : Color.yellow;
        Gizmos.color = IsCurrentPassed ? Color.blue : Gizmos.color;

        Gizmos.color = IsFinish || IsStart ? Color.magenta : Gizmos.color;

        var size = IsCurrenTarget ? 10 : 5;
        Gizmos.DrawSphere(transform.position, size);
    }
}
