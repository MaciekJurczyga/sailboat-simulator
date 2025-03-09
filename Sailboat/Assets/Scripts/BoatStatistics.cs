using UnityEngine;
using TMPro;


public class BoatStatistics:MonoBehaviour
{
    public TextMeshProUGUI statsText;

    private float _trueWindAttackAngle = 0f;
    private float _boatSpeed = 0f;
    private float _windSpeed = 0f;
    
    public void UpdateStats(float trueWindAttackAngle, float boatSpeed, float windSpeed)
    {
        _trueWindAttackAngle = trueWindAttackAngle;
        _boatSpeed = boatSpeed;
        _windSpeed = windSpeed;
        UpdateStatsUI();
    }

    private void UpdateStatsUI()
    {
        statsText.text = $"U(w): {_boatSpeed}\n " +
                         $"w: {_trueWindAttackAngle}\n" +
                         $"wind Speed: {_windSpeed}\n";
    }
}