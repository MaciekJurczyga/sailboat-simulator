using UnityEngine;

public class BoatDirectionLogger : MonoBehaviour
{
    public float logInterval = 1f; 
    private float nextLogTime = 0f; 
    void Update()
    {
        if (Time.time >= nextLogTime) 
        {
            float rotationY = transform.eulerAngles.y;
            Debug.Log("Kąt obrotu łodzi (Y): " + rotationY);
            
            nextLogTime = Time.time + logInterval; 
        }
    }
}