using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class GraphDrawer : MonoBehaviour
{
    public RectTransform graphArea;
    public GameObject pointPrefab;
    public float padding = 16f;
    public bool keepAspect = true;
    public Color axisColor = Color.black;

    private List<GameObject> spawnedPoints = new List<GameObject>();
    private List<GameObject> spawnedLabels = new List<GameObject>();
    private GameObject xAxis, yAxis;

    public void DrawGraph(List<Point> points)
    {
        foreach (var go in spawnedPoints) Destroy(go);
        spawnedPoints.Clear();
        foreach (var go in spawnedLabels) Destroy(go);
        spawnedLabels.Clear();
        if (xAxis != null) Destroy(xAxis);
        if (yAxis != null) Destroy(yAxis);

        if (points == null || points.Count == 0) return;

        float minX = float.MaxValue, maxX = float.MinValue;
        float minY = float.MaxValue, maxY = float.MinValue;

        foreach (var p in points)
        {
            minX = Mathf.Min(minX, p.getX());
            maxX = Mathf.Max(maxX, p.getX());
            minY = Mathf.Min(minY, p.getY());
            maxY = Mathf.Max(maxY, p.getY());
        }
        
        if (minX == maxX) { minX -= 1f; maxX += 1f; }
        if (minY == maxY) { minY -= 1f; maxY += 1f; }

        var rect = graphArea.rect;
        float w = rect.width - 2f * padding;
        float h = rect.height - 2f * padding;

        float rangeX = maxX - minX;
        float rangeY = maxY - minY;

        float sx = w / rangeX;
        float sy = h / rangeY;
        float s = keepAspect ? Mathf.Min(sx, sy) : 1f;

        float scaledW = rangeX * s;
        float scaledH = rangeY * s;
        
        Vector2 center = new Vector2(rect.center.x, rect.center.y);
        
        Vector2 plotOrigin = center - new Vector2(scaledW / 2f, scaledH / 2f);

        // Pozycje osi w środku wykresu
        float axisX_Y_globalPos = center.y; // pozioma oś w środku
        float axisY_X_globalPos = center.x; // pionowa oś w środku

        // Oś X
        xAxis = CreateAxis(
            new Vector2(plotOrigin.x, axisX_Y_globalPos),
            new Vector2(plotOrigin.x + scaledW, axisX_Y_globalPos),
            "X Axis"
        );

        // Oś Y
        yAxis = CreateAxis(
            new Vector2(axisY_X_globalPos, plotOrigin.y),
            new Vector2(axisY_X_globalPos, plotOrigin.y + scaledH),
            "Y Axis"
        );

        // Rysowanie punktów
        for (int i = 0; i < points.Count; i++)
        {
            float lx = (points[i].getX() - minX) * s;
            float ly = (points[i].getY() - minY) * s;

            Vector2 pos = new Vector2(plotOrigin.x + lx, plotOrigin.y + ly);

            GameObject newPoint = Instantiate(pointPrefab, graphArea);
            newPoint.GetComponent<RectTransform>().anchoredPosition = pos;
            spawnedPoints.Add(newPoint);
        }

        Debug.Log("Drawn points: " + points.Count);
    }

    private GameObject CreateAxis(Vector2 start, Vector2 end, string name)
    {
        GameObject axis = new GameObject(name);
        axis.transform.SetParent(graphArea, false);
        var line = axis.AddComponent<Image>();
        line.color = axisColor;

        var rt = axis.GetComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f); // ustawienie na lewy dolny róg
        rt.pivot = new Vector2(0.5f, 0.5f); // środek
        rt.sizeDelta = new Vector2(Vector2.Distance(start, end), 5f);
        rt.anchoredPosition = (start + end) / 2f;
        float angle = Mathf.Atan2(end.y - start.y, end.x - start.x) * Mathf.Rad2Deg;
        rt.localRotation = Quaternion.Euler(0, 0, angle);

        return axis;
    }
}
