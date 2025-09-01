public class BoatData
{
    public float vDeg;
    public float wDeg;
    public float CalculatedBoatSpeedWithoutWindSpeed;

    public BoatData(float vDeg, float wDeg, float calculatedBoatSpeedWithoutWindSpeed)
    {
        this.vDeg = vDeg;
        this.wDeg = wDeg;
        CalculatedBoatSpeedWithoutWindSpeed = calculatedBoatSpeedWithoutWindSpeed;
    }
}