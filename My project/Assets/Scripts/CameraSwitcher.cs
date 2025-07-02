using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera[] cameras;
    private int currentCameraIndex = 0;

    void Start()
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].enabled = (i == currentCameraIndex);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            cameras[currentCameraIndex].enabled = false;
            
            currentCameraIndex = (currentCameraIndex + 1) % cameras.Length;
            
            cameras[currentCameraIndex].enabled = true;
        }
    }
}