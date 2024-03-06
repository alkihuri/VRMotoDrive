using UnityEngine;
using System.Collections.Generic;
using System.Text;



[System.Serializable]
public class LapPoints
{
    public List<GhostPoint> Points;
    public int ID;

    public LapPoints()
    {
        Points = new List<GhostPoint>();
    }
}


[System.Serializable]
public class GhostPoint
{
    public Vector3 Position;
    public float Time;
    public Vector3 Rotation;
}