using UnityEngine;
using LuxWater;

public class CameraSwitcher : MonoBehaviour
{
    public Camera[] cameras;
    private int currentCameraIndex = 0;

    void Start()
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            SetCameraState(cameras[i], false);
        }
        
        if (cameras.Length > 0)
        {
            SetCameraState(cameras[currentCameraIndex], true);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SetCameraState(cameras[currentCameraIndex], false);
            
            currentCameraIndex = (currentCameraIndex + 1) % cameras.Length;
            SetCameraState(cameras[currentCameraIndex], true);
        }
    }

    void SetCameraState(Camera cam, bool state)
    {
        cam.enabled = state;
        
        AudioListener listener = cam.GetComponent<AudioListener>();
        if (listener != null)
        {
            listener.enabled = state;
        }
        
        LuxWater_ProjectorRenderer luxRenderer = cam.GetComponent<LuxWater_ProjectorRenderer>();
        if (luxRenderer != null)
        {
            luxRenderer.enabled = state;
        }
    }
}