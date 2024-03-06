using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GhostsManager : MonoBehaviour
{
    [SerializeField] List<TracePointController> _tracePoints = new List<TracePointController>();
    [SerializeField] LineRenderer _lineRenderer;
    [SerializeField] TracePathController _tracePathControlelr;


    [SerializeField] List<GhostController> _ghosts = new List<GhostController>();

    private void Awake()
    {
        CashingAndSettings();
    }

    private void CashingAndSettings()
    {

        _tracePathControlelr = GetComponentInChildren<TracePathController>(true);

        if (_tracePathControlelr == null)
            _tracePathControlelr = FindObjectOfType<TracePathController>();


        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = _tracePoints.Count;
        _lineRenderer.enabled = true;


        _tracePathControlelr.OnPathComplete.AddListener(InnitGhost);

        _tracePathControlelr.OnPathStart.AddListener(FirstRide);


        _ghosts = GetComponentsInChildren<GhostController>(true).ToList();
    }

    private void FirstRide()
    {
        if (TraceDataManager.Instance.LapData == null)
        {
            return;
        }
        else
        {
            if (TraceDataManager.Instance.LapData.LapPoints == null)
            {
                return;
            }
            else
            {
                if (TraceDataManager.Instance.LapData.LapPoints.Count > 0)
                    InnitGhost(TraceDataManager.Instance.LapData.LapPoints?.Last());
            }
        }


    }
     

    public void InnitGhost(LapPoints lapPoints)
    {

        var readyGhost = _ghosts[0];

        if (readyGhost != null)
        {
            readyGhost.gameObject.SetActive(true);
            readyGhost.Innit(lapPoints.Points);
        }

    }


}
