using UnityEngine;

public class BoatController : MonoBehaviour
{
    public float turnSpeed = 50f;
    private WindSystem _wind;
    public float logInterval = 1f;
    private float nextLogTime = 0f;
    private PhysicsCalculator _physics;
    private float boatSpeed = 0f;

    private void Start()
    {
        _wind = FindObjectOfType<WindSystem>();
        _physics = new PhysicsCalculator();
    }

    void Update()
    {
        float turnInput = Input.GetAxis("Horizontal");
        float rotationAmount = turnInput * turnSpeed * Time.deltaTime;
        transform.Rotate(0, rotationAmount, 0);

        float attackAngle = CalculateAttackAngle();
        _physics.Calculate(attackAngle, _wind.GetWindSpeedKnots());

        boatSpeed = _physics.GetBoatSpeed();
        
        MoveBoat();

        if (Time.time >= nextLogTime)
        {
            Debug.Log("True wind attack angle: " + attackAngle + " | Boat Speed: " + boatSpeed + " | Apparent wind attack angle: " + _physics.GetWDeg());
            nextLogTime = Time.time + logInterval;
        }
    }

    private void MoveBoat()
    {
        transform.position += transform.forward * boatSpeed * Time.deltaTime;
    }

    private float CalculateAttackAngle()
    {
        if (_wind == null) return 0f;

        float boatAngle = transform.eulerAngles.y;
        float windAngle = _wind.windAngle;
        
        float diff = boatAngle - windAngle;
        return diff >= 0 ? diff : 360 + diff;
    }
}

public class PhysicsCalculator
{
    private const float L_TO_D_WATER_RATIO = 15f;
    private const float L_TO_D_AIR_RATIO = 5f;
    private const float S0 = 1.5f;
    private const float W_SPEED_WIND = 10f;
    private static readonly float V_BORDER_RAD = -(Mathf.Atan(L_TO_D_AIR_RATIO) - Mathf.PI);

    private float s_of_v;
    private float S_of_v;
    private float w_rad;
    private float w_deg;
    private float U_of_w;
    
    public void Calculate(float v_deg, float wind_speed_konts)
    {
        bool v_in_range_180_360 = v_deg > 180;
        v_deg = v_in_range_180_360 ? 360 - v_deg : v_deg;
        float v_rad = v_deg * Mathf.Deg2Rad;

        int dead_angle_checker_1 = Mathf.Cos(V_BORDER_RAD - v_rad) >= 0 ? 1 : 0;
        int dead_angle_check_2 = Mathf.Pow(Mathf.Tan((V_BORDER_RAD - v_rad)) / L_TO_D_WATER_RATIO, 2) <= 1 ? 1 : 0;

        s_of_v = CalculateSofV(dead_angle_checker_1, dead_angle_check_2, v_rad);
        S_of_v = CalculateSofVAdjusted(v_rad);
        w_rad = CalculateWRad(v_rad);
        w_deg = CalculateWDeg();
        U_of_w = CalculateUofW(v_rad, wind_speed_konts);
    }

    private float CalculateSofV(int checker1, int checker2, float vRad)
    {
        if (checker1 + checker2 != 2) return 0;
        return 0.5f * S0 * S0 * Mathf.Cos(V_BORDER_RAD - vRad) * (1 + Mathf.Sqrt(1 - Mathf.Pow(Mathf.Tan(V_BORDER_RAD - vRad) / L_TO_D_WATER_RATIO, 2)));
    }

    private float CalculateSofVAdjusted(float vRad)
    {
        return (vRad < V_BORDER_RAD) ? s_of_v : S0 * S0;
    }

    private float CalculateWRad(float vRad)
    {
        if (S_of_v > 0)
        {
            return Mathf.Atan(Mathf.Sin(vRad) / (Mathf.Cos(vRad) - S_of_v));
        }
        return 0;
    }

    private float CalculateWDeg()
    {
        float angleInDegrees = w_rad * Mathf.Rad2Deg;
        return (angleInDegrees >= 0) ? angleInDegrees : (180 + angleInDegrees);
    }

    private float CalculateUofW(float vRad, float wind_speed_konts)
    {
        if (S_of_v > 0)
        {
            return wind_speed_konts * S_of_v / Mathf.Sqrt(1 + S_of_v * S_of_v - 2 * S_of_v * Mathf.Cos(vRad));
        }
        return 0;
    }

    public float GetBoatSpeed()
    {
        return U_of_w;
    }

    public float GetWDeg()
    {
        return w_deg;
    }
}
