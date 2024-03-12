using System.Collections.Generic;

[System.Serializable]
public class LapData
{
    public LapData()
    {
        LapPoints = new List<LapPoints>();  
        LapPoints.Add(new LapPoints());
        LapPoints[0].Points = new List<GhostPoint>();
    }
    public List<LapPoints> LapPoints = new List<LapPoints>();
}
