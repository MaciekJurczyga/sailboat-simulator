﻿using UnityEngine;


/**
 * v - apparent wind attack angle
 * w - true wind attack angle
 * sOfV = s(v) speed ratio parameter without including type of sailing
 * adjustedSOfV = S(V) speed ration parameter including type of sailing
 *
 */
public class PhysicsCalculator
{
    public static int MaxLiftToDragAirRation = 5;
    private const float LiftToDragWaterRatio = 15f;
    private const float S0 = 1.5f;
    private static float _borderAngleRad = 0;
    
    private float _sOfV;
    private float _adjustedSOfV;
    private float _wRad;
    private float _wDeg;
    private float _boatSpeed;

    public void Calculate(float vDeg, float windSpeedKnots, float liftToDragAirRatio)
    {
        _borderAngleRad = calculateBorderAngleRad(liftToDragAirRatio);
        float vRad = vDeg * Mathf.Deg2Rad;

        int deadAngleCheck1 = Mathf.Cos(_borderAngleRad - vRad) >= 0 ? 1 : 0;
        int deadAngleCheck2 = Mathf.Pow(Mathf.Tan((_borderAngleRad - vRad)) / LiftToDragWaterRatio, 2) <= 1 ? 1 : 0;

        _sOfV = CalculateSOfV(deadAngleCheck1, deadAngleCheck2, vRad);
        _adjustedSOfV = CalculateAdjustedSOfV(vRad);
        _wRad = CalculateWRad(vRad);
        _wDeg = CalculateWDegrees();
        _boatSpeed = CalculateBoatSpeed(vRad, windSpeedKnots);
    }

    private float calculateBorderAngleRad(float liftToDragAirRation)
    {
        return  -(Mathf.Atan(liftToDragAirRation) - Mathf.PI);
    }
    private float CalculateSOfV(int check1, int check2, float vRad)
    {
        if (check1 + check2 != 2) return 0;
        return 0.5f * S0 * S0 * Mathf.Cos(_borderAngleRad - vRad) *
               (1 + Mathf.Sqrt(1 - Mathf.Pow(Mathf.Tan(_borderAngleRad - vRad) / LiftToDragWaterRatio, 2)));
    }

    private float CalculateAdjustedSOfV(float vRad)
    {
        return (vRad < _borderAngleRad) ? _sOfV : S0 * S0;
    }

    private float CalculateWRad(float vRad)
    {
        if (_adjustedSOfV > 0)
        {
            return Mathf.Atan(Mathf.Sin(vRad) / (Mathf.Cos(vRad) - _adjustedSOfV));
        }
        return 0;
    }

    private float CalculateWDegrees()
    {
        float angleInDegrees = _wRad * Mathf.Rad2Deg;
        return (angleInDegrees >= 0) ? angleInDegrees : (180 + angleInDegrees);
    }

    private float CalculateBoatSpeed(float vRad, float windSpeedKnots)
    {
        if (_adjustedSOfV > 0)
        {
            return windSpeedKnots * _adjustedSOfV / Mathf.Sqrt(1 + _adjustedSOfV * _adjustedSOfV - 2 * _adjustedSOfV * Mathf.Cos(vRad));
        }
        return 0;
    }

    public float GetBoatSpeed()
    {
        return _boatSpeed;
    }

    public float GetTrueWindAttackAngle()
    {
        return _wDeg;
    }

    public float GetLDWater()
    {
        return LiftToDragWaterRatio;
    }
}