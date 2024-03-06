using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class TracePathController : MonoBehaviour
{
    [SerializeField] List<TracePointController> _tracePoints;
    [SerializeField] GameObject _player;
    [SerializeField] GameObject _playerChasis;
    [SerializeField] float distanceThreshold = 50;

    private Coroutine recordingCoroutine;
    private bool recordingInProgress;
    private LapPoints lapPointsRecorded;

    public UnityEvent<LapPoints> OnPathComplete = new UnityEvent<LapPoints>();
    public UnityEvent OnPathFinish = new UnityEvent();
    public UnityEvent OnPathStart = new UnityEvent();

    public bool FirstLap { get; private set; }

    private void Awake()
    {
        FirstLap = true;
        CacheAndSettings();
        recordingCoroutine = StartCoroutine(CurrentPointSystem());
    }

    private void Start()
    {

    }

    private void CacheAndSettings()
    {
        _tracePoints = GetComponentsInChildren<TracePointController>().ToList();

        OnPathStart.AddListener(() =>
        {
            recordingInProgress = true;
            StartRecording();
            Debug.Log("Lap started");
        });

        OnPathFinish.AddListener(() =>
        {
            recordingInProgress = false;
            OnPathComplete.Invoke(lapPointsRecorded);
            Debug.Log("Lap finished");
        });
    }

    private IEnumerator CurrentPointSystem()
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
        lapPointsRecorded = new LapPoints();
        StartCoroutine(RecordPlayerPosition());
    }

    private IEnumerator RecordPlayerPosition()
    {
        while (recordingInProgress)
        {
            yield return new WaitForFixedUpdate();
            var point = new GhostPoint
            {
                Position = _player.transform.position,
                Rotation = _playerChasis.transform.eulerAngles,
                Time = Time.time
            };
            lapPointsRecorded.Points.Add(point);
            Debug.DrawRay(_player.transform.position, Vector3.up * 10, Color.red, 1);
        }
    }

    public void SetCurrentPoint(int currentPointIndex)
    {

        if (currentPointIndex >= _tracePoints.Count)
        {
            currentPointIndex = 0;
        }



        if (currentPointIndex - 1 == 0)
            OnPathStart.Invoke();

        _tracePoints[(currentPointIndex - 1 + _tracePoints.Count) % _tracePoints.Count].IsCurrentPassed = true;


        FirstLap = !_tracePoints.All(p => p.IsCurrentPassed);


        // If the player has passed all the points, the lap is finished _tracePoints.All(p => p.IsCurrentPassed)
        if (_tracePoints.First().IsCurrentPassed && !FirstLap)
        {
            OnPathFinish.Invoke();
            _tracePoints.ForEach(p => p.IsCurrentPassed = false);
        }

        _tracePoints.ForEach(p => p.IsCurrenTarget = false);
        _tracePoints[currentPointIndex].IsCurrenTarget = true;
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
