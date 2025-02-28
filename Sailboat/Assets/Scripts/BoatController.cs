using UnityEngine;

public class BoatController : MonoBehaviour
{
    private WindSystem _wind;
    private PhysicsCalculator _physics;
    private Rigidbody _rb;
    private BoatStatistics _boatStatistics;
    
    private float _calculatedBoatSpeed = 0f;
    public float turnSpeed = 50f;

    private void Start()
    {
        _wind = new WindSystem();
        _physics = new PhysicsCalculator();
        _boatStatistics = GetComponent<BoatStatistics>();
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

        _calculatedBoatSpeed = _physics.GetBoatSpeed();
        
        MoveBoat();
        BoatTurning();
        _boatStatistics.UpdateStats(trueWindAttackAngle, apparentWindAttackAngle, _calculatedBoatSpeed);
    }

    private void MoveBoat()
    {
        
        _rb.MovePosition(_rb.position + Time.deltaTime * _calculatedBoatSpeed * transform.forward);
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
}

