using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour
{
    private Rigidbody _rb;
    private BoatStatistics _boatStatistics;
    private WindSystem _windSystem = WindSystem.GetInstance();
    private PhysicsModel _physicsModel = PhysicsModel.GetInstance();
    
    public float turnSpeed = 50f;
    public float tau = 2.5f;
    private float currentSpeed = 0;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _boatStatistics = GetComponent<BoatStatistics>();
        _rb.isKinematic = false;
        _physicsModel.LoadModel();
    }

    void FixedUpdate()
    {
        BoatData foundBoatData = _physicsModel.FindBoatSpeed(transform.eulerAngles.y);
        MoveBoat(foundBoatData.CalculatedBoatSpeed);
        TurnBoat();
        _boatStatistics.UpdateStats(
            foundBoatData,
            currentSpeed,
            _windSystem.GetWindSpeedKnots());
    }

    private void MoveBoat(float targetSpeed)
    {
        // apply boat acceleration to target speed
        // if boat is in dead angle and its speed is 0, increase tau for more realistic slowing down
        tau = targetSpeed == 0 ? 5f : 2.5f;
        currentSpeed += (-1 / tau) * (currentSpeed - targetSpeed) * Time.deltaTime;
        
        _rb.MovePosition(_rb.position + Time.deltaTime * currentSpeed * transform.forward);
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