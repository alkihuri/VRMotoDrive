using Sirenix.OdinInspector;
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


    public GhostController ReadyGhost
    {
        get
        {
            return _ghosts[0];
        }
    }

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


        _tracePathControlelr.OnPathStart.AddListener(FirstRide);
        _tracePathControlelr.OnPathStart.AddListener(PlayGhost);
        _tracePathControlelr.OnPathRecordingComplete.AddListener(CreateLastRideGhostPath);


        _ghosts = GetComponentsInChildren<GhostController>(true).ToList();
    }

    private void FirstRide()
    {

        if (TraceDataManager.Instance.LapData.LapPoints.Count > 0)
            CreateLastRideGhostPath(TraceDataManager.Instance.LapData.LapPoints[0]);


    }


    public void CreateLastRideGhostPath(LapPoints lapPoints)
    {

        if (ReadyGhost != null)
        {
            ReadyGhost.gameObject.SetActive(true);
            ReadyGhost.InnitPathWay(lapPoints.Points);
        }

    }


    [Button("PlayGhost")]
    public void PlayGhost()
    {
        if (ReadyGhost != null)
        {
            if (!ReadyGhost.gameObject.activeInHierarchy)
                ReadyGhost.gameObject.SetActive(true);

            ReadyGhost.PlayGhost();
        }
    }


}
