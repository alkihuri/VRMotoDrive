using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
using System;
using System.Threading.Tasks;
using System.Collections;

public class TraceDataManager : MonoSinglethon<TraceDataManager>
{
    const string DATA_PATH = "DataLapInfo.json";

    [SerializeField] TracePathController _tracePathController;
    [SerializeField, Range(0, 10)] int drawPathIndex;
    public int LastID { get; private set; }
    public bool IsReading { get; private set; }
    public bool IsWriting { get; private set; }

    public LapData LapData;
     

    private void Start()
    {
        _tracePathController.OnPathRecordingComplete.AddListener(OnLapFinished);
        LoadData();
    }

    private void OnLapFinished(LapPoints points)
    {
        

        if (LapData.LapPoints != null && LapData.LapPoints.Count > 0 && LapData.LapPoints[0].Points != null && LapData.LapPoints[0].Points.Count > 0)
        {
            LapData.LapPoints[0].Points.Clear();
        }

        foreach (var point in points.Points)
        {
            var newPoint = new GhostPoint(point);
            LapData.LapPoints[0].Points.Add(newPoint);
        }


        GhostsManager.Instance.ReadyGhost.InnitPathWay(LapData.LapPoints[0].Points);

        Task.Run(() => SaveData()); 
       
    }

    [Button("Save data")]
    public void SaveData()
    {
        StartCoroutine(SaveDataRoutine());
    }

    IEnumerator SaveDataRoutine()
    {
        yield return new WaitWhile(() => IsReading);

        string json = JsonUtility.ToJson(LapData);
        string filePath = GetFilePath();
        var task = File.WriteAllTextAsync(filePath, json);
        yield return task;
        Debug.Log("<color=green>Path points data saved to:</color> " + filePath);

        LoadData();
    }


    [Button("Load data")]
    public void LoadData()
    {
        StartCoroutine(LoadDataRoutine());
    }

    IEnumerator LoadDataRoutine()
    {

        yield return new WaitWhile(() => IsWriting);

        LapData = new LapData();
        string filePath = GetFilePath();
        if (!File.Exists(filePath))
        {
            Debug.LogWarning("File not found. Creating new file...");
            File.Create(filePath).Dispose();
            yield break;
        }

        var json = File.ReadAllTextAsync(filePath);

        yield return json;
        LapData = JsonUtility.FromJson<LapData>(json.Result);
        LastID = LapData.LapPoints.Count;
    }

    private string GetFilePath()
    {
        var path = Path.Combine(PathFinder.ConfigsPath, DATA_PATH);
        return path;
    }

    private void OnDrawGizmos()
    {
        if (LapData == null) return;
        if (LapData.LapPoints == null) return;
        if (LapData.LapPoints.Count == 0) return;

        var selectedPoints = LapData.LapPoints[0];
        if (selectedPoints == null || !selectedPoints.Points.Any()) return;

        DrawPath(selectedPoints.Points);
    }

    private void DrawPath(List<GhostPoint> points)
    {
        Gizmos.color = Color.blue;

        for (int i = 0; i < points.Count - 1; i++)
        {
            Gizmos.DrawLine(points[i].Position, points[i + 1].Position);
        }

        Gizmos.DrawLine(points.First().Position, points.Last().Position);
    }
}
