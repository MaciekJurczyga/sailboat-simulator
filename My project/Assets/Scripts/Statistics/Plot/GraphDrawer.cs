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

    
    public void Initialize(GraphPointsWrapper graphPointsWrapper) 
    {
        _graphPointsWrapper = graphPointsWrapper;
    }
    
    public void DrawGraph()
    {
        List<Point> points = _graphPointsWrapper.getPoints();
        if (points == null || points.Count == 0) return;

        minX = float.MaxValue; maxX = float.MinValue;
        minY = float.MaxValue; maxY = float.MinValue;
        foreach (var p in points)
        {
            minX = Mathf.Min(minX, p.getX());
            maxX = Mathf.Max(maxX, p.getX());
            minY = Mathf.Min(minY, p.getY());
            maxY = Mathf.Max(maxY, p.getY());
        }
        if (minX == maxX) { minX -= 1f; maxX += 1f; }
        if (minY == maxY) { minY -= 1f; maxY += 1f; }
        
        float w = graphArea.rect.width - 2f * padding;
        float h = graphArea.rect.height - 2f * padding;
        float rangeX = maxX - minX;
        float rangeY = maxY - minY;

        float sx = w / rangeX;
        float sy = h / rangeY;
        s = keepAspect ? Mathf.Min(sx, sy) : 1f;

        float scaledW = rangeX * s;
        float scaledH = rangeY * s;
        plotOrigin = new Vector2(-scaledW / 2f, -scaledH / 2f);
        
        if (xAxisLine != null)
        {
            xAxisLine.Points = new Vector2[]
            {
                new Vector2(-scaledW/2f, 0),
                new Vector2(scaledW/2f, 0)
            };
            xAxisLine.LineThickness = axisThickness;
            xAxisLine.color = axisColor;
        }

        if (yAxisLine != null)
        {
            yAxisLine.Points = new Vector2[]
            {
                new Vector2(0, -scaledH/2f),
                new Vector2(0, scaledH/2f)
            };
            yAxisLine.LineThickness = axisThickness;
            yAxisLine.color = axisColor;
        }

        int maxSamples = 5000;
        int maxPerSide = maxSamples / 2;

        List<Point> leftPointsRaw = new List<Point>();
        List<Point> rightPointsRaw = new List<Point>();

        foreach (var p in points)
        {
            if (p.getX() < 0)
            {
                leftPointsRaw.Add(p);
            }
            else
            {
                rightPointsRaw.Add(p);
            }
        }
        
        List<Vector2> sampledLeft = new List<Vector2>();
        List<Vector2> sampledRight = new List<Vector2>();

        if (leftPointsRaw.Count > 0)
        {
            int step = Mathf.Max(1, leftPointsRaw.Count / maxPerSide);
            for (int i = 0; i < leftPointsRaw.Count; i += step)
                sampledLeft.Add(WorldToGraph(leftPointsRaw[i]));
        }

        if (rightPointsRaw.Count > 0)
        {
            int step = Mathf.Max(1, rightPointsRaw.Count / maxPerSide);
            for (int i = 0; i < rightPointsRaw.Count; i += step)
                sampledRight.Add(WorldToGraph(rightPointsRaw[i]));
        }
        
        if (dataLineLeft != null)
        {
            dataLineLeft.Points = sampledLeft.ToArray();
            dataLineLeft.LineThickness = lineThickness;
            dataLineLeft.color = lineColor;
        }
        else
        {
            Debug.LogError("dataLineLeft is not assigned!");
        }

        if (dataLineRight != null)
        {
            dataLineRight.Points = sampledRight.ToArray();
            dataLineRight.LineThickness = lineThickness;
            dataLineRight.color = lineColor;
        }
        else
        {
            Debug.LogError("dataLineRight is not assigned!");
        }

        
        if (userPointRT == null && userPointPrefab != null)
        {
            GameObject go = Instantiate(userPointPrefab, graphArea);
            userPointRT = go.GetComponent<RectTransform>();
        }

        Debug.Log($"DrawGraph: {points.Count} punktów, lewa strona próbkowana do {sampledLeft.Count}, prawa do {sampledRight.Count}");
    }
    
    public void DrawUserPoint(BoatData boatData)
    {
        if (userPointRT == null) return;

        Point userPoint = calculateUserPoint(boatData);
        Vector2 pos = WorldToGraph(userPoint);
        userPointRT.anchoredPosition = pos;
    }
    
    
    private Point calculateUserPoint(BoatData boatData)
    {
        float x = boatData.CalculatedBoatSpeed * Mathf.Sin(boatData.wDeg * Mathf.PI / 180);
        float y = boatData.CalculatedBoatSpeed * Mathf.Cos(boatData.wDeg * Mathf.PI / 180);
        return new Point(x, y);
    }
    
    private Vector2 WorldToGraph(Point p)
    {
        float lx = (p.getX() - minX) * s + plotOrigin.x;
        float ly = (p.getY() - minY) * s + plotOrigin.y;
        return new Vector2(lx, ly);
    }
}
