using TMPro;
using UnityEngine;


public class BoatStatistics:MonoBehaviour
{
    
    private float _trueWindAttackAngle = 0f;
    private float _apparentWindAttackAngle = 0f;
    private float _currentBoatSpeed = 0f;
    private float _windSpeed = 0f;
    
    public TextMeshProUGUI boatSpeedText;
    public TextMeshProUGUI apparentWindAngleText;
    public TextMeshProUGUI trueWindAngleText;
    public TextMeshProUGUI windSpeedText;
    
    public void UpdateStats(BoatData foundBoatData, float currentBoatSpeed, float windSpeed)
    {
        if (foundBoatData.wDeg == 0)
        {
            // if boat is in dead angle, vDeg changes from lets say -30 to + 30, however to avoid nulls 
            // we set wDeg to 0 in such cases. 
            // in dead angle, apparent (vDeg) and true (wDeg) wind is the same, so we update _trueWindAttackAngle with actual value (from vDeg)
            _trueWindAttackAngle = foundBoatData.vDeg;
        }

        _trueWindAttackAngle = foundBoatData.wDeg;
        _apparentWindAttackAngle = foundBoatData.vDeg;
        _currentBoatSpeed = currentBoatSpeed;
        _windSpeed = windSpeed;
        UpdateText();
        
    }

    private void UpdateText()
    {
        windSpeedText.text = $"Prędkość wiatru [Węzły]: {_windSpeed:F1}";
        trueWindAngleText.text = $"Rzeczywisty kąt natarcia [°]: {_trueWindAttackAngle:F1}";
        apparentWindAngleText.text = $"Pozorny kąt natarcia [°]: {_apparentWindAttackAngle:F1}";
        boatSpeedText.text = $"Prędkość łodzi [węzły]: {_currentBoatSpeed:F1}";
    }


}