using TMPro;
using UnityEngine;


public class BoatStatistics
{
    public TextMeshProUGUI statsText;

    private float _trueWindAttackAngle = 0f;
    private float _calculatedBoatSpeed = 0f;
    private float _currentBoatSpeed = 0f;
    
    private float _windSpeed = 0f;
    
    public void UpdateStats(float trueWindAttackAngle, float calculatedBoatSpeed, float currentBoatSpeed, float windSpeed)
    {
        _trueWindAttackAngle = trueWindAttackAngle;
        _currentBoatSpeed = currentBoatSpeed;
        _calculatedBoatSpeed = calculatedBoatSpeed;
        _windSpeed = windSpeed;
        UpdateStatsUI();
    }

    private void UpdateStatsUI()
    {
        Debug.Log($"U(w): {_calculatedBoatSpeed}\n " +
                  $"Current speed: {_currentBoatSpeed}\n " +
                  $"w: {_trueWindAttackAngle}\n" +
                  $"wind Speed: {_windSpeed}\n");
    }
}