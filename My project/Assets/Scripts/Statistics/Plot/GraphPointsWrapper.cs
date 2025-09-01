using System.Collections.Generic;
using UnityEngine;

public class GraphPointsWrapper
{
    
    private List<Point> basePoints = new List<Point>();
    
    public void LoadPoints(List<BoatData> boatData)
    {
        for (int i = 0; i < boatData.Count / 2; i++)
        {
            BoatData data = boatData[i];
            if (data.CalculatedBoatSpeedWithoutWindSpeed == 0)
            {
                continue;
            }
            float baseX = data.CalculatedBoatSpeedWithoutWindSpeed * Mathf.Sin(data.wDeg * Mathf.PI / 180);
            float baseY = data.CalculatedBoatSpeedWithoutWindSpeed * Mathf.Cos(data.wDeg * Mathf.PI / 180);
            
            basePoints.Add(new Point(baseX, baseY));
            basePoints.Add(new Point(-baseX, baseY));
        }
    }
    
    public List<Point> GetWindSpeedScaledPoints(float windSpeed)
    {
        List<Point> scaled = new List<Point>(basePoints.Count);
        foreach (var p in basePoints)
            scaled.Add(new Point(p.getX() * windSpeed, p.getY() * windSpeed));
        return scaled;
    }
    
}
