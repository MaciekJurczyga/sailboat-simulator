using UnityEngine;

public class FollowBoat : MonoBehaviour
{
    public Transform boat;
    private Vector3 offset = new Vector3(-0.25f, 5.25f, -15f);

        
    void LateUpdate()
    {
        
        transform.position = boat.position + offset;
        transform.LookAt(boat);
    }
}