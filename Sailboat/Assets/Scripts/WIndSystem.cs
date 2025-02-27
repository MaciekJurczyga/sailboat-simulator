using UnityEngine;

public class WindSystem : MonoBehaviour
{
    public float windAngle = 0f; 
    public float windSpeedKnots = 10f; 
    
    public float GetWindSpeedMS()
    {
        return windSpeedKnots * 0.5144f; 
    }

    public float GetWindSpeedKnots()
    {
        return windSpeedKnots;
    }
}