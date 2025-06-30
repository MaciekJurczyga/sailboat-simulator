using UnityEngine;

public class BoatCameraFollow : MonoBehaviour
{
    public Transform boatTransform;
    public Vector3 offset = new Vector3(-0.25f, 5.25f, -15f); 

    void LateUpdate()
    {
        Vector3 targetPosition = boatTransform.position + boatTransform.rotation * offset;
        
        transform.position = targetPosition;

        Quaternion boatYRotation = Quaternion.Euler(0f, boatTransform.eulerAngles.y, 0f);
        transform.rotation = boatYRotation;
    }
}