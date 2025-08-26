using TMPro;
using UnityEngine;

public class WindIndicatorController : MonoBehaviour
{
    public RectTransform arrowTransform;
    public TextMeshProUGUI windSpeedText;
    private WindSystem _windSystem = WindSystem.GetInstance();
    private float apparentWindAngle = 0f;

    void Update()
    {
        arrowTransform.localEulerAngles = new Vector3(0, 0, apparentWindAngle);
        windSpeedText.text = _windSystem.GetWindSpeedKnots().ToString("F1");
    }

    public void SetWindAngle(BoatData boatData)
    {
        float vDeg = boatData.vDeg;
        if (vDeg > 180 && vDeg < 360)
        {
            vDeg -= 360;
        }
        apparentWindAngle = vDeg;
    }
}