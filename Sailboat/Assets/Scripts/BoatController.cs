using UnityEngine;

public class BoatController : MonoBehaviour
{
    private WindSystem _wind;
    private PhysicsCalculator _physics;
    private Rigidbody _rb;
    
    public float logInterval = 1f;
    private float nextLogTime = 0f;
    private float boatSpeed = 0f;
    public float turnSpeed = 50f;
    private float drag = 0.1f;

    private void Start()
    {
        _wind = new WindSystem();
        _physics = new PhysicsCalculator();
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;
        _rb.isKinematic = false;
    }

    void Update()
    {
        
        float trueWindAttackAngle = CalculateAttackAngle();
        _physics.Calculate(trueWindAttackAngle, _wind.GetWindSpeedKnots());
        float apparentWindAttackAngle = _physics.GetApparentWindAngleDegrees();
        if (trueWindAttackAngle > 180 && trueWindAttackAngle <= 360)
        {
            apparentWindAttackAngle = 360 - apparentWindAttackAngle;
        }

        boatSpeed = _physics.GetBoatSpeed();
        
        MoveBoat();
        BoatTurning();
        LogBoatStatistics(trueWindAttackAngle, apparentWindAttackAngle);
        
    }

    private void MoveBoat()
    {
        if (boatSpeed == 0)
        {
       
            var currentSpeed = _rb.linearVelocity.magnitude;
            if (currentSpeed > 0)
            {
                boatSpeed = Mathf.Max(currentSpeed - drag * Time.deltaTime, 0f); 
            }
        }
        _rb.MovePosition(_rb.position + Time.deltaTime * boatSpeed * transform.forward);
    }

    private float CalculateAttackAngle()
    {
        // Calculates attack angle:
        // 0-180 left tack pl: hals
        // 180-360 right tack
        if (_wind == null) return 0f;

        var boatAngle = transform.eulerAngles.y;
        var windAngle = _wind.getWindAngle();

        var diff = boatAngle - windAngle;
        return diff >= 0 ? diff : 360 + diff;
    }

    private void BoatTurning()
    {
        var turnInput = Input.GetAxis("Horizontal");
        var rotationAmount = turnInput * turnSpeed * Time.deltaTime;

        Quaternion turnRotation = Quaternion.Euler(0, rotationAmount, 0);
        _rb.MoveRotation(_rb.rotation * turnRotation);
    }


    private void LogBoatStatistics(float trueWindAttackAngle, float apparentWindAttackAngle)
    {
        if (Time.time >= nextLogTime)
        {
            Debug.Log("True wind attack angle: " + trueWindAttackAngle + " | Boat Speed: " + boatSpeed +
                      " | Apparent wind attack angle: " + apparentWindAttackAngle);
            nextLogTime = Time.time + logInterval;
        }
    }
}

