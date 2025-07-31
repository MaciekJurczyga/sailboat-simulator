using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PhysicsModel {
    private static PhysicsModel _instance; 
    private PhysicsCalculator _physics = new PhysicsCalculator();
    private WindSystem _windSystem = WindSystem.GetInstance();
    private List<BoatData> _boatDataList = new List<BoatData>();
    private List<BoatData> _sortedBoatDataList;

    
    private PhysicsModel() {}
    
    public static PhysicsModel GetInstance() {
        if (_instance == null) {
            _instance = new PhysicsModel();
        }
        return _instance;
    }

    public void LoadModel()
    {
        int steps = 18000; // 180 / 0.001
        for (int i = 0; i < steps; i++)
        {
            float vDeg = i * 0.01f;
            _physics.Calculate(vDeg, _windSystem.GetWindSpeedKnots());
            float wDeg = _physics.GetTrueWindAttackAngle();
            float boatSpeed = _physics.GetBoatSpeed();

            _boatDataList.Add(new BoatData(vDeg, wDeg, boatSpeed));
        }
        FillMissingWDegValues();
        for (int i = 0; i < steps; i++)
        {
            var original = _boatDataList[i];
            _boatDataList.Add(new BoatData(
                360f - original.vDeg,
                360f - original.wDeg,
                original.CalculatedBoatSpeed
            ));
        }
        _sortedBoatDataList = _boatDataList.OrderBy(data => data.wDeg).ToList();
    }

    private void FillMissingWDegValues()
    {
        int firstNonZeroIndex = _boatDataList.FindIndex(data => data.wDeg != 0);

        if (firstNonZeroIndex <= 0)
            return; 

        float delta = _boatDataList[firstNonZeroIndex].wDeg / firstNonZeroIndex;

        for (int i = 1; i < firstNonZeroIndex; i++)
        {
            _boatDataList[i].wDeg = _boatDataList[i - 1].wDeg + delta;
        }
    }
    
    public float FindLeewayAngle(BoatData boatData)
    {
        float vDeg = boatData.vDeg;

        // Martwy kąt wiatru — brak dryfu
        if (vDeg > 135f && vDeg < 225f)
        {
            return 0f;
        }

        // TODO: zapytac się o wzór bo z arctg(LDWody), bo wychodzi 86 stopni
        float baseLeeway = 5f;

        // Lewy hals: vDeg < 180 → dryf w prawo → dodatni kąt dryfu
        // Prawy hals: vDeg > 180 → dryf w lewo → ujemny kąt dryfu
        
        float signedLeeway = (vDeg < 180f) ? baseLeeway : -baseLeeway;

        return signedLeeway;
    }


    public BoatData FindBoatSpeed(float boatAngle)
    {
        float targetAttackAngle = CalculateAttackAngle(boatAngle);

        if (_sortedBoatDataList == null || _sortedBoatDataList.Count == 0)
            return new BoatData(0, 0, 0);

        int left = 0;
        int right = _sortedBoatDataList.Count - 1;
        BoatData closest = _sortedBoatDataList[0];
        float smallestDiff = float.MaxValue;

        while (left <= right)
        {
            int mid = (left + right) / 2;
            var midData = _sortedBoatDataList[mid];
            float diff = Mathf.Abs(midData.wDeg - targetAttackAngle);

            if (diff < smallestDiff)
            {
                smallestDiff = diff;
                closest = midData;
            }

            if (midData.wDeg < targetAttackAngle)
                left = mid + 1;
            else
                right = mid - 1;
        }

        return closest;
    }


    public float CalculateAttackAngle(float boatAngle)
    {
        // Calculates true wind attack angle:
        // 0-180 left tack pl: hals
        // 180-360 right tack
        if (_windSystem == null) return 0f;

        var trueWindAngle = _windSystem.getWindAngle();

        var diff = boatAngle - trueWindAngle;

        // diff is in range -180:180, return is 0-360
        return diff >= 0 ? diff : 360 + diff;
    }
}
