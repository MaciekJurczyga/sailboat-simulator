﻿using UnityEngine;

public class BoatController : MonoBehaviour
{
    private Rigidbody _rb;
    private BoatStatistics _boatStatistics;
    private WindIndicatorController _windIndicatorController;
    private WindSystem _windSystem;
    private PhysicsModel _physicsModel;
    private GraphPointsWrapper _graphPointsWrapper;
    public GraphDrawer graphDrawer;
    
    public float turnSpeed = 50f;
    public float tau = 2.5f;
    private float currentSpeed;
    
    public void Initialize(PhysicsModel physicsModel, WindSystem windSystem, GraphPointsWrapper graphPointsWrapper)
    {
        _physicsModel = physicsModel;
        _windSystem = windSystem;
        _graphPointsWrapper = graphPointsWrapper;
    }
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _boatStatistics = GetComponent<BoatStatistics>();
        _windIndicatorController = GetComponent<WindIndicatorController>();
        _rb.isKinematic = false;
    }

    void FixedUpdate()
    {
        float windSpeed = _windSystem.GetWindSpeedKnots();
        BoatData foundBoatData = _physicsModel.getBoatData(transform.eulerAngles.y);
        float leewayAngle = _physicsModel.FindLeewayAngle(foundBoatData);
        MoveBoat(foundBoatData.CalculatedBoatSpeedWithoutWindSpeed * windSpeed, leewayAngle);
        TurnBoat();
        _windIndicatorController.SetWindAngle(foundBoatData);
        graphDrawer.DrawUserPoint(foundBoatData, windSpeed);
        graphDrawer.UpdateGraphView(windSpeed);
        _boatStatistics.UpdateStats(
            foundBoatData,
            currentSpeed,
            windSpeed);
    }

    private void MoveBoat(float targetSpeed, float leewayAngle)
    {
        // apply boat acceleration to target speed
        // if boat is in dead angle and its speed is 0, increase tau for more realistic slowing down
        tau = targetSpeed == 0 ? 5f : 2.5f;
        currentSpeed += (-1 / tau) * (currentSpeed - targetSpeed) * Time.deltaTime;
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