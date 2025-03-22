using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BoatController : MonoBehaviour
{
    private WindSystem _wind;
    private PhysicsCalculator _physics;
    private Rigidbody _rb;
    private BoatStatistics _boatStatistics;
    private List<BoatData> _boatDataList = new List<BoatData>();
    public float turnSpeed = 50f;
    public float currentSpeed = 0;
    private float tau = 1.5f;

    private void Start()
    {
        _wind = new WindSystem();
        _physics = new PhysicsCalculator();
        _boatStatistics = GetComponent<BoatStatistics>();
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;
        _rb.isKinematic = false;
        for (float vDeg = 0; vDeg <= 180; vDeg += 0.01f)
        {
            _physics.Calculate(vDeg, _wind.GetWindSpeedKnots());
            float wDeg = _physics.GetTrueWindAttackAngle();
            float boatSpeed = _physics.GetBoatSpeed();
            _boatDataList.Add(new BoatData(vDeg, wDeg, boatSpeed));
        }
        Debug.Log(_boatDataList.Count);
    }

    void Update()
    {
        float trueWindAttackAngle = CalculateAttackAngle();
        float calculatedBoatSpeed = FindBoatSpeed(trueWindAttackAngle);
        MoveBoat(calculatedBoatSpeed);
        BoatTurning();
        _boatStatistics.UpdateStats(
            trueWindAttackAngle, 
            calculatedBoatSpeed,
            currentSpeed, 
            _wind.GetWindSpeedKnots());
    }

    private void MoveBoat(float targetSpeed)
    {
        // apply boat acceleration to target speed
        currentSpeed += (-1 / tau) * (currentSpeed - targetSpeed) * Time.deltaTime;
        _rb.MovePosition(_rb.position + Time.deltaTime * currentSpeed * transform.forward);
    }

    private float CalculateAttackAngle()
    {
        // Calculates true wind attack angle:
        // 0-180 left tack pl: hals
        // 180-360 right tack
        if (_wind == null) return 0f;

        var boatAngle = transform.eulerAngles.y;
        var trueWindAngle = _wind.getWindAngle();

        var diff = boatAngle - trueWindAngle;
        return diff >= 0 ? diff : 360 + diff;
    }
    
    private void BoatTurning()
    {
        var turnInput = Input.GetAxis("Horizontal");
        var rotationAmount = turnInput * turnSpeed * Time.deltaTime;

        Quaternion turnRotation = Quaternion.Euler(0, rotationAmount, 0);
        _rb.MoveRotation(_rb.rotation * turnRotation);
    }
    
    private float FindBoatSpeed(float trueWindAttackAngle)
    {
        if (trueWindAttackAngle > 180 && trueWindAttackAngle <= 360)
        {
            trueWindAttackAngle = 360 - trueWindAttackAngle;
        }

        BoatData bestMatch = null;
        float smallestDifference = float.MaxValue;
        
        foreach (var data in _boatDataList)
        {
            float difference = Mathf.Abs(data.wDeg - trueWindAttackAngle);
            if (difference < smallestDifference)
            {
                smallestDifference = difference;
                bestMatch = data;
            }
        }
        
        if (bestMatch != null)
        {
            return bestMatch.boatSpeed;
        }
        
        return 0f;
    }
}

