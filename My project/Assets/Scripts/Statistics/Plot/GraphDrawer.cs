using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI.Extensions;

public class GraphDrawer : MonoBehaviour
{
    [Header("References")]
    public RectTransform graphArea;
    public GameObject userPointPrefab;

    [Header("Line Settings")]
    public Color axisColor = Color.black;
    public Color lineColor = Color.red;
    public float axisThickness = 2f;
    public float lineThickness = 5f;
    public float padding = 16f;
    public bool keepAspect = true;

    [Header("UILineRenderer References")]
    public UILineRenderer dataLineLeft;
    public UILineRenderer dataLineRight;
    public UILineRenderer xAxisLine;
    public UILineRenderer yAxisLine;


    private float minX, maxX, minY, maxY;
    private float s;
    private Vector2 plotOrigin;
    private RectTransform userPointRT;
    private GraphPointsWrapper _graphPointsWrapper;


    private float _lastDrawnWindSpeed = float.NaN;
    

    private readonly List<Vector2> _sampledLeft = new List<Vector2>();
    private readonly List<Vector2> _sampledRight = new List<Vector2>();

    public void Initialize(GraphPointsWrapper graphPointsWrapper)
    {
        _graphPointsWrapper = graphPointsWrapper;
        
        if (userPointRT == null && userPointPrefab != null)
        {
            GameObject go = Instantiate(userPointPrefab, graphArea);
            userPointRT = go.GetComponent<RectTransform>();
        }
    }

  
    public void UpdateGraphView(float currentWindSpeed)
    {
  
        float quantizedWindSpeed = Mathf.Round(currentWindSpeed * 10f) / 10f;

       
        if (!Mathf.Approximately(quantizedWindSpeed, _lastDrawnWindSpeed))
        {
            RedrawGraph(currentWindSpeed); 
            _lastDrawnWindSpeed = quantizedWindSpeed; 
        }
        
    }
    
    private void RedrawGraph(float windSpeed)
    {
        if (_graphPointsWrapper == null) return;
        List<Point> points = _graphPointsWrapper.GetWindSpeedScaledPoints(windSpeed);
        if (points == null || points.Count == 0) return;

        minX = float.MaxValue; maxX = float.MinValue;
        minY = float.MaxValue; maxY = float.MinValue;
        

        List<Point> leftPointsRaw = new List<Point>();
        List<Point> rightPointsRaw = new List<Point>();

        foreach (var p in points)
        {
            minX = Mathf.Min(minX, p.getX());
            maxX = Mathf.Max(maxX, p.getX());
            minY = Mathf.Min(minY, p.getY());
            maxY = Mathf.Max(maxY, p.getY());

            if (p.getX() < 0)
                leftPointsRaw.Add(p);
            else
                rightPointsRaw.Add(p);
        }

        if (minX == maxX) { minX -= 1f; maxX += 1f; }
        if (minY == maxY) { minY -= 1f; maxY += 1f; }

        float w = graphArea.rect.width - 2f * padding;
        float h = graphArea.rect.height - 2f * padding;
        float rangeX = maxX - minX;
        float rangeY = maxY - minY;

        float sx = w / rangeX;
        float sy = h / rangeY;
        s = keepAspect ? Mathf.Min(sx, sy) : sx; 
        float scaledW = rangeX * s;
        float scaledH = rangeY * s;
        plotOrigin = new Vector2(-scaledW / 2f, -scaledH / 2f);
        
        DrawAxes(scaledW, scaledH);
        
        int maxSamples = 250;
        int maxPerSide = maxSamples / 2;
        
        _sampledLeft.Clear();
        _sampledRight.Clear();

        if (leftPointsRaw.Count > 0)
        {
            int step = Mathf.Max(1, leftPointsRaw.Count / maxPerSide);
            for (int i = 0; i < leftPointsRaw.Count; i += step)
                _sampledLeft.Add(WorldToGraph(leftPointsRaw[i]));
        }

        if (rightPointsRaw.Count > 0)
        {
            int step = Mathf.Max(1, rightPointsRaw.Count / maxPerSide);
            for (int i = 0; i < rightPointsRaw.Count; i += step)
                _sampledRight.Add(WorldToGraph(rightPointsRaw[i]));
        }

        if (dataLineLeft != null)
        {
            dataLineLeft.Points = _sampledLeft.ToArray(); 
            dataLineLeft.LineThickness = lineThickness;
            dataLineLeft.color = lineColor;
            dataLineLeft.SetAllDirty(); 
        }

        if (dataLineRight != null)
        {
            dataLineRight.Points = _sampledRight.ToArray();
            dataLineRight.LineThickness = lineThickness;
            dataLineRight.color = lineColor;
            dataLineRight.SetAllDirty(); 
        }
        
    }

    private void DrawAxes(float scaledW, float scaledH)
    {
        if (xAxisLine != null)
        {
            xAxisLine.Points = new Vector2[]
            {
                new Vector2(-scaledW / 2f, 0),
                new Vector2(scaledW / 2f, 0)
            };
            xAxisLine.LineThickness = axisThickness;
            xAxisLine.color = axisColor;
        }

        if (yAxisLine != null)
        {
            yAxisLine.Points = new Vector2[]
            {
                new Vector2(0, -scaledH / 2f),
                new Vector2(0, scaledH / 2f)
            };
            yAxisLine.LineThickness = axisThickness;
            yAxisLine.color = axisColor;
        }
    }

    public void DrawUserPoint(BoatData boatData, float windSpeed)
    {
        if (userPointRT == null) return;

        Point userPoint = CalculateUserPoint(boatData, windSpeed);
        Vector2 pos = WorldToGraph(userPoint);
        userPointRT.anchoredPosition = pos;
    }

    private Point CalculateUserPoint(BoatData boatData, float windSpeed)
    {
        float x = boatData.CalculatedBoatSpeedWithoutWindSpeed * windSpeed * Mathf.Sin(boatData.wDeg * Mathf.PI / 180);
        float y = boatData.CalculatedBoatSpeedWithoutWindSpeed * windSpeed * Mathf.Cos(boatData.wDeg * Mathf.PI / 180);
        return new Point(x, y);
    }

    private Vector2 WorldToGraph(Point p)
    {
        if (maxX - minX == 0 || maxY - minY == 0) return plotOrigin;

        float lx = (p.getX() - minX) * s + plotOrigin.x;
        float ly = (p.getY() - minY) * s + plotOrigin.y;
        return new Vector2(lx, ly);
    }
}