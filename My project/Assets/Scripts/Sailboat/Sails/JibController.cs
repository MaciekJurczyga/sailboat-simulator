using UnityEngine;
    
public class JibController : MonoBehaviour
{
    [Tooltip("The speed at which the jib rotates.")]
    public float rotationSpeed = 50f;
    [Tooltip("The minimum rotation angle (e.g., -70).")]
    public float minAngle = -70f;

    [Tooltip("The maximum rotation angle (e.g., 70).")]
    public float maxAngle = 70f;
    
    private float currentYAngle = 0f;

    private Quaternion startRotation;

    void Start()
    { 
        startRotation = transform.localRotation;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.M))
        {
         
            currentYAngle += rotationSpeed * Time.deltaTime;
        }

        else if (Input.GetKey(KeyCode.B))
        {
     
            currentYAngle -= rotationSpeed * Time.deltaTime;
        }


        currentYAngle = Mathf.Clamp(currentYAngle, minAngle, maxAngle);


        Quaternion rotationAmount = Quaternion.Euler(0, currentYAngle, 0);
        
        transform.localRotation = startRotation * rotationAmount;
    }
}