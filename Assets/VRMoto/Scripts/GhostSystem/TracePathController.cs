using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class TracePathController : MonoBehaviour
{
    [SerializeField] List<TracePointController> _tracePoints;
    [SerializeField] GameObject _player;
    [SerializeField] GameObject _playerChasis;
    [SerializeField] float distanceThreshold = 50;

    private Coroutine recordingCoroutine;
    private bool recordingInProgress;

    [Header("POINTS")]
    [SerializeField] private LapPoints lapPointsRecorded;

    public UnityEvent<LapPoints> OnPathRecordingComplete = new UnityEvent<LapPoints>();

    public UnityEvent OnPathFinish = new UnityEvent();
    public UnityEvent OnPathStart = new UnityEvent();


    [Header("Debug")]

    [SerializeField] float _currentPointIndex;
    [SerializeField] float _passedPointIndex;


    private void Awake()
    {
        CacheAndSettings();
    }

    private void Start()
    {
        StartCoroutine(CheckPointSystem());
    }

    private void CacheAndSettings()
    {
        _tracePoints = GetComponentsInChildren<TracePointController>().ToList();

        lapPointsRecorded = new LapPoints();

        OnPathStart.AddListener(() =>
        {
            recordingInProgress = true;
            StartRecording();
            Debug.Log("Lap started");
        });

        OnPathFinish.AddListener(() =>
        {
            recordingInProgress = false;
            Debug.Log("Lap finished");
        });
    }

    private IEnumerator CheckPointSystem()
    {
        int currentPointIndex = 0;
        while (true)
        {
            yield return new WaitForFixedUpdate();
            if (Vector3.Distance(_player.transform.position, _tracePoints[currentPointIndex].Position) < distanceThreshold)
            {
                currentPointIndex = (currentPointIndex + 1) % _tracePoints.Count;
                SetCurrentPoint(currentPointIndex);
            }
        }
    }

    private void StartRecording()
    {
        StartCoroutine(RecordPlayerPosition());
    }

    private IEnumerator RecordPlayerPosition()
    {
        lapPointsRecorded?.Points?.Clear();
        Debug.Log("Recording started...");
        while (recordingInProgress)
        {
            yield return new WaitForFixedUpdate();
            GhostPoint point = new GhostPoint(
                _player.transform.position,
                _playerChasis.transform.eulerAngles,
                  Time.time
            );
            lapPointsRecorded.Points.Add(point);
            Debug.DrawRay(_player.transform.position, Vector3.up * 10, Color.red, 1);
        }
        Debug.Log("Recording finished");
        OnPathRecordingComplete.Invoke(lapPointsRecorded);
    }

    public void SetCurrentPoint(int currentPointIndex)
    {

        if (currentPointIndex >= _tracePoints.Count)
        {
            currentPointIndex = 0;
        }

        _currentPointIndex = currentPointIndex;

        _tracePoints[(currentPointIndex - 1 + _tracePoints.Count) % _tracePoints.Count].IsCurrentPassed = true;


        if (_tracePoints.First().IsCurrentPassed)
        {
            _tracePoints.ForEach(p => p.IsCurrentPassed = false);
        }

        _tracePoints.ForEach(p => p.IsCurrenTarget = false);
        _tracePoints[currentPointIndex].IsCurrenTarget = true;

        _passedPointIndex = currentPointIndex > 0 ? --currentPointIndex : _tracePoints.Count - 1;

        if (_passedPointIndex == 0)
            OnPathStart.Invoke();
        if (_passedPointIndex == _tracePoints.Count - 1)
            OnPathFinish.Invoke();

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
