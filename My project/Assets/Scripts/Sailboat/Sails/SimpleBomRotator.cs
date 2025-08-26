using UnityEngine;

public class SimpleBomRotator : MonoBehaviour
{
    public Transform bomTransform;
    
    public float rotationSpeed = 50f;

    void FixedUpdate()
    {
        if (bomTransform == null)
        {
            return;
        }
        float rotationAmount = rotationSpeed * Time.deltaTime;

     
        if (Input.GetKey(KeyCode.J))
        {
           
            bomTransform.Rotate(0f, 0f, -rotationAmount, Space.Self);
        }
       
        else if (Input.GetKey(KeyCode.L))
        {
            bomTransform.Rotate(0f, 0f, rotationAmount, Space.Self);
        }
    }
}