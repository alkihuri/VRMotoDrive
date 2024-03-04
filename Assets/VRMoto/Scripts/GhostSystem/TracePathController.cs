using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class TracePathController : MonoBehaviour
{
    [SerializeField] List<TracePointController> _tracePoints;
    [SerializeField] GameObject _player;

    public TracePointController CurrentPoint;

    [SerializeField] LapPoints _lapPoints;
    public int CurrentPointIndex
    {
        set
        {
            SetCurrentPoint(value);
            if (value == 0)
            {
                StartCoroutine(RecordPlayerPosition());
            }
        }
        get
        {
            return _tracePoints.IndexOf(CurrentPoint);
        }
    }

    public bool RecordingInProgress;

    public UnityEvent<LapPoints> OnLapFinished = new UnityEvent<LapPoints>();
    private void Awake()
    {
        CashingAndSettings();
        StartCoroutine(CurrentPointSystem());
    }

    [Button("Innit points")]
    private void CashingAndSettings()
    {
        _lapPoints = new LapPoints();
        _tracePoints = GetComponentsInChildren<TracePointController>().ToList();
    }




    IEnumerator CurrentPointSystem()
    {
        CurrentPointIndex = 0;
        while (true)
        {
            yield return new WaitForSeconds(1 / 10);
            var distance = Vector3.Distance(_player.transform.position, CurrentPoint.Position);
           
            if (distance < 25)
            {
                CurrentPointIndex = CurrentPointIndex + 1;
            }

        }
    }


    IEnumerator RecordPlayerPosition()
    {
        RecordingInProgress = true;
        LapPoints lapPoints = new LapPoints();
        while (!_tracePoints.Last().IsCurrent)
        {
            yield return new WaitForSeconds(.2f);

            var point = new GhostPoint();
            point.Position = _player.transform.position;
            point.Time = Time.time; 
            lapPoints.Points.Add(point); 

            Debug.DrawRay(_player.transform.position, Vector3.up * 10, Color.red, 1);   
        }
        OnLapFinished.Invoke(lapPoints);
        RecordingInProgress = false;
    }

    public void SetCurrentPoint(int currentPoint)
    {

        if (currentPoint >= _tracePoints.Count)
        {
            currentPoint = 0;
        }




        var point = _tracePoints[currentPoint];
        _tracePoints.ForEach(p => p.IsCurrent = false);
        point.IsCurrent = true;
        CurrentPoint = point;
    }

    private void OnDrawGizmos()
    {
        if (_tracePoints == null || _tracePoints.Count == 0)
            return;

        for (int i = 0; i < _tracePoints.Count - 1; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(_tracePoints[i].transform.position, _tracePoints[i + 1].transform.position);
        }

        Gizmos.DrawLine(_tracePoints.Last().transform.position, _tracePoints.First().transform.position);
    }
}
