using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TraceDataManager : MonoBehaviour
{

    const string DATA_PATH = "DataLapInfo.json";

    public LapData _lapData;

    [SerializeField] TracePathController _tracePathController;

    public int LastID;

    [SerializeField, Range(0, 10)] public int DrawPathIndex;

    private void Awake()
    {
        _tracePathController.OnPathComplete.AddListener(OnLapFinished);
        LoadData();
    }

    private void OnLapFinished(LapPoints points)
    {
        points.ID = LastID + 1;
        _lapData.LapPoints.Add(points);
        SaveData();
    }

    [Button("Save data")]
    public void SaveData()
    {
        string json = JsonUtility.ToJson(_lapData);
        Debug.Log(Application.streamingAssetsPath + "/" + DATA_PATH);
        Debug.Log(json);
        System.IO.File.WriteAllText(Application.streamingAssetsPath + "/" + DATA_PATH, json);
    }

    [Button("Load data")]
    public void LoadData()
    {
        if (!System.IO.File.Exists(Application.streamingAssetsPath + "/" + DATA_PATH))
        {
            Debug.LogWarning("File not found");
            _lapData = new LapData();
            return;
        }

        string json = System.IO.File.ReadAllText(Application.streamingAssetsPath + "/" + DATA_PATH);
        var data = JsonUtility.FromJson<LapData>(json);

        if (data != null)
        {
            _lapData.LapPoints.AddRange(data.LapPoints);
            LastID = _lapData.LapPoints.Count;
        }
        else
        {
            _lapData = new LapData();
            _lapData.LapPoints = new List<LapPoints>();
        }
    }

    private void OnDrawGizmos()
    {
        if (_lapData != null)
        {
            if (_lapData.LapPoints.Count > 0)
                if (_lapData.LapPoints.Where(x => x.ID == DrawPathIndex).ToList().Count > 0)
                {
                    var points = _lapData.LapPoints.Where(x => x.ID == DrawPathIndex).FirstOrDefault();

                    if (points.Points.Count > 0)
                    {
                        // draw line between points
                        for (int i = 0; i < points.Points.Count - 1; i++)
                        {
                            Gizmos.color = Color.blue;
                            Gizmos.DrawLine(points.Points[i].Position, points.Points[i + 1].Position);
                        }
                        // draw betwean first and last point    
                        Gizmos.color = Color.blue;
                        Gizmos.DrawLine(points.Points.First().Position, points.Points.Last().Position);
                    }

                    // draw line betwean last and first point
                    if (points.Points.Count > 0)
                    {
                        Gizmos.color = Color.blue;
                        Gizmos.DrawLine(points.Points.Last().Position, points.Points.First().Position);
                    }
                }
        }

    }
}

[System.Serializable]
public class LapData
{
    public List<LapPoints> LapPoints;
}