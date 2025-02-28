using UnityEngine;
using TMPro;


public class BoatStatistics:MonoBehaviour
{
    public TextMeshProUGUI statsText;

    private float _trueWindAttackAngle = 0f;
    private float _apparentWindAttackAngle = 0f;
    private float _boatSpeed = 0f;
    
    public void UpdateStats(float trueWindAttackAngle, float apparentWindAttackAngle, float boatSpeed)
    {
        _trueWindAttackAngle = trueWindAttackAngle;
        _apparentWindAttackAngle = apparentWindAttackAngle;
        _boatSpeed = boatSpeed;
        UpdateStatsUI();
    }
    
    private void UpdateStatsUI()
    {
        if (statsText != null)
        {
            statsText.text = $"Boat Speed: {_boatSpeed}\nTrue Attack Angle: {_trueWindAttackAngle}\nApparent Attack Angle: {_apparentWindAttackAngle}";
        }
        else
        {
            Debug.LogError("statsText is not assigned in BoatStatistics!");
        }
    }
}