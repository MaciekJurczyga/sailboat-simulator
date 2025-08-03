using UnityEngine;

public class BoatController : MonoBehaviour
{
    private Rigidbody _rb;
    private BoatStatistics _boatStatistics;
    private WindIndicatorController _windIndicatorController;
    private WindSystem _windSystem = WindSystem.GetInstance();
    private PhysicsModel _physicsModel = PhysicsModel.GetInstance();
    
    public float turnSpeed = 50f;
    public float tau = 2.5f;
    public float currentSpeed { get; private set; }
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _boatStatistics = GetComponent<BoatStatistics>();
        _windIndicatorController = GetComponent<WindIndicatorController>();
        _rb.isKinematic = false;
        _physicsModel.LoadModel();
    }

    void FixedUpdate()
    {
        BoatData foundBoatData = _physicsModel.FindBoatSpeed(transform.eulerAngles.y);
        float leewayAngle = _physicsModel.FindLeewayAngle(foundBoatData);
        MoveBoat(foundBoatData.CalculatedBoatSpeed, leewayAngle);
        TurnBoat();
        _windIndicatorController.SetWindAngle(foundBoatData);
        _boatStatistics.UpdateStats(
            foundBoatData,
            currentSpeed,
            _windSystem.GetWindSpeedKnots());
    }

    private void MoveBoat(float targetSpeed, float leewayAngle)
    {
        // apply boat acceleration to target speed
        // if boat is in dead angle and its speed is 0, increase tau for more realistic slowing down
        tau = targetSpeed == 0 ? 5f : 2.5f;
        currentSpeed += (-1 / tau) * (currentSpeed - targetSpeed) * Time.deltaTime;
        Debug.Log(leewayAngle);        
        Vector3 driftDirection = Quaternion.Euler(0, leewayAngle, 0) * transform.forward;

        _rb.MovePosition(_rb.position + Time.deltaTime * currentSpeed * driftDirection.normalized);
    }
    

    private void TurnBoat()
    {
        var turnInput = Input.GetAxis("Horizontal");
        var rotationAmount = turnInput * turnSpeed * Time.deltaTime;

        Quaternion turnRotation = Quaternion.Euler(0, rotationAmount, 0);
        _rb.MoveRotation(_rb.rotation * turnRotation);
    }

    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }
}