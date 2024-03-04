using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    [SerializeField] List<TracePointController> _tracePoints = new List<TracePointController>();
    [SerializeField] LineRenderer _lineRenderer;

    private void Awake()
    {
        CashingAndSettings();
    }

    private void CashingAndSettings()
    { 


        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = _tracePoints.Count;
        _lineRenderer.enabled = true;

        
    }
}
