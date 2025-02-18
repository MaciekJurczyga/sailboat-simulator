using UnityEngine;


public class BoatController : MonoBehaviour
{
    public float turnSpeed = 50f;
    private WindSystem _wind;
    public float logInterval = 1f; 
    private float nextLogTime = 0f; 
    
    private void Start()
    {
        _wind = FindObjectOfType<WindSystem>();
    }

    void Update()
    { 
        float turnInput = Input.GetAxis("Horizontal");
        float rotationAmount = turnInput * turnSpeed * Time.deltaTime;

        transform.Rotate(0, rotationAmount, 0);
        if (Time.time >= nextLogTime)
        {
            Debug.Log( "Attack angle: " + CalculateAttackAngle());
            
            nextLogTime = Time.time + logInterval; 
        }
    }

    // V angle
    private float CalculateAttackAngle()
    {
        float boatAngle = transform.eulerAngles.y;
        float windAngle = _wind.windAngle;

        float diff = boatAngle - windAngle;
        float attackAngle;
        if (diff >= 0)
        {
            attackAngle = diff;
        }
        else
        {
            attackAngle = 360 + diff;
        }

        return attackAngle;
    }
}