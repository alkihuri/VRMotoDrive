using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    [SerializeField] private List<GhostPoint> _points;
    private Vector3 _startPostion;
    [SerializeField] Transform _shape;

    public bool IsPlaying { get; private set; }


    private void Awake()
    {
        IsPlaying = false;
        _startPostion = transform.position;
    }
    public void InnitPathWay(List<GhostPoint> points)
    {



        if (IsPlaying)
        {
            ResetPosition();
            IsPlaying = false;
            points.Clear();
        }
        foreach(var point in points)
        {
            GhostPoint newPoint = new GhostPoint(point); 
        }
    }


    public void PlayGhost()
    {
        if (_points.Count > 0)
        {
            StartCoroutine(PlayGhostRoutine());
        }
        else
        {
            if (
                TraceDataManager.Instance.LapData != null &&
                TraceDataManager.Instance.LapData.LapPoints != null &&
                TraceDataManager.Instance.LapData.LapPoints.Count > 0 &&
                TraceDataManager.Instance.LapData.LapPoints[0] != null &&
                TraceDataManager.Instance.LapData.LapPoints[0].Points != null &&
                TraceDataManager.Instance.LapData.LapPoints[0].Points.Count > 0
                )
            {
                _points = TraceDataManager.Instance.LapData.LapPoints[0].Points;
                StartCoroutine(PlayGhostRoutine());
            }
            else
            {
                Debug.LogWarning("No points to play");
            }

        }
    }

    [Button("InnitPathWay from eit mode")]
    public void InnitFromEdit()
    {
        if (IsPlaying)
        {
            ResetPosition();
            IsPlaying = false;
            _points.Clear();
        }
        _points = TraceDataManager.Instance.LapData.LapPoints[0].Points;
        StartCoroutine(PlayGhostRoutine());
    }

    private IEnumerator PlayGhostRoutine()
    {
        IsPlaying = true;
        for (int i = 0; i < _points.Count - 1; i++)
        {
            var time = _points[i + 1].Time - _points[i].Time;
            transform.position = _points[i].Position;
            _shape.eulerAngles = _points[i].Rotation;


            if (i > 0)
                Debug.DrawLine(_points[i - 1].Position, _points[i].Position, Color.cyan, 1);


            yield return new WaitForFixedUpdate();
        }
        IsPlaying = false;
    }

    private void ResetPosition()
    {
        transform.position = _startPostion;
    }
}
