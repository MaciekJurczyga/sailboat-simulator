using UnityEngine;

public class WindSystem : MonoBehaviour // Zmiana na MonoBehaviour
{

    public float averageWindSpeed = 10f;
    
    public float fluctuationAmplitude = 2f;
    
    public float changeFrequency = 0.1f;
    
    private float _currentWindSpeedKnots;


    private float _windAngle = 0f;
    
    void Start()
    {
        _currentWindSpeedKnots = averageWindSpeed;
    }
    
    void Update()
    {

        float perlinValue = Mathf.PerlinNoise(Time.time * changeFrequency, 0f);
        
        float fluctuation = Mathf.Lerp(-fluctuationAmplitude, fluctuationAmplitude, perlinValue);

        _currentWindSpeedKnots = averageWindSpeed + fluctuation;
    }


    public float GetWindSpeedMS()
    {
        return _currentWindSpeedKnots * 0.5144f;
    }

    public float GetWindSpeedKnots()
    {
        return _currentWindSpeedKnots;
    }

    public float getWindAngle()
    {
        return _windAngle;
    }
}