public class WindSystem
{
    private float _windAngle = 0f; 
    const float WindSpeedKnots = 10f; 
    
    public float GetWindSpeedMS()
    {
        return WindSpeedKnots * 0.5144f; 
    }

    public float GetWindSpeedKnots()
    {
        return WindSpeedKnots;
    }

    public float getWindAngle()
    {
        return _windAngle;
    }
}