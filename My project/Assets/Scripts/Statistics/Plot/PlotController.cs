using System.Collections.Generic;
using UnityEngine;

public class PlotController
{
    private static PlotController _instance;
    private List<Point> points = new List<Point>();

    public static PlotController getInstance()
    {
        if (_instance == null)
        {
            _instance = new PlotController();
        }

        return _instance;
    }

    public void loadPoints(List<BoatData> boatData)
    {
        for (int i = 0; i < boatData.Count / 2; i++)
        {
            BoatData data = boatData[i];
            if (data.CalculatedBoatSpeed == 0)
            {
                continue;
            }
            float x = data.CalculatedBoatSpeed * Mathf.Sin(data.wDeg * Mathf.PI / 180);
            float y = data.CalculatedBoatSpeed * Mathf.Cos(data.wDeg * Mathf.PI / 180);
            
            points.Add(new Point(x, y));
            points.Add(new Point(-x, y));
        }
    }

    public List<Point> getPoints()
    {
        return points;
    }
}
