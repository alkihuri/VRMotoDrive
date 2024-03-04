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


    private void Awake()
    {
        _tracePathController.OnLapFinished.AddListener(OnLapFinished);
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
}

[System.Serializable]
public class LapData
{
    public List<LapPoints> LapPoints;
}