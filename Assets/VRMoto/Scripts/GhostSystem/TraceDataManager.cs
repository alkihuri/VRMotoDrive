
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
using System.Threading.Tasks;
using System.Threading;
using System.Collections;

public class TraceDataManager : MonoSinglethon<TraceDataManager>
{
    const string DATA_PATH = "DataLapInfo.json";

    [SerializeField] TracePathController _tracePathController;
    [SerializeField, Range(0, 10)] int drawPathIndex;
    public int LastID { get; private set; }
    public LapData LapData;

    private void Awake()
    {
        _tracePathController.OnPathRecordingComplete.AddListener(OnLapFinished);
        LoadData();
    }

    private void OnLapFinished(LapPoints points)
    {


        if (LapData.LapPoints != null)
        {
            points.ID = ++LastID;
        }
        else
        {
            points.ID = 0;
        }


        if (LapData.LapPoints != null 
            && LapData.LapPoints.Count > 0 
                && LapData.LapPoints[0].Points != null 
                    && LapData.LapPoints[0].Points.Count > 0)
                        LapData?.LapPoints[0]?.Points?.Clear();


        foreach (var point in points.Points)
        {
            var newPoint = new GhostPoint(point); 
            LapData.LapPoints[0].Points.Add(newPoint);
        }


        SaveData();
    }

    [Button("Save data")]
    public void SaveData()
    {
        // call the async method
        StartCoroutine(SaveDataAsync());
    }



    private IEnumerator SaveDataAsync()
    {
        string json = JsonUtility.ToJson(LapData);
        string filePath = GetFilePath();
        File.WriteAllText(filePath, json);
        Debug.Log("<color=green>Path points data saved to:</color> " + filePath);
        Time.timeScale = 0.1f;
        yield return new WaitForSecondsRealtime(1); 
        LoadData();
        yield return new WaitForSecondsRealtime(1);
        Time.timeScale = 1;

    }


    [Button("Load data")]
    public void LoadData()
    {
        string filePath = GetFilePath();
        if (!File.Exists(filePath))
        {
            Debug.LogWarning("File not found. Creating new file...");
            File.Create(filePath).Dispose();
            LapData = new LapData();
            return;
        }

        string json = File.ReadAllText(filePath);
        LapData = JsonUtility.FromJson<LapData>(json);
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
