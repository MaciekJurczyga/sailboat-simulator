public class WindSystem
{
    private static WindSystem _instance;
    
    private float _windAngle = 0f; 
    const float WindSpeedKnots = 10f; 
    
    private WindSystem(){}

    public static WindSystem GetInstance()
    {
        if (_instance == null)
        {
            _instance = new WindSystem();
        }

        return _instance;
    }
    
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