using UnityEngine;

public class WaveHeightExample : MonoBehaviour
{
    public Material waterMaterial;  
    public Vector3 worldPosition;  
    public float timeOffset;  

    void Update()
    {
        LuxWaterUtils.GersterWavesDescription waveDescription = new LuxWaterUtils.GersterWavesDescription();
        
        LuxWaterUtils.GetGersterWavesDescription(ref waveDescription, waterMaterial);

        Vector3 displacement = LuxWaterUtils.GetGestnerDisplacement(worldPosition, waveDescription, timeOffset);

   
        float waveHeight = displacement.y;
        
        Debug.Log("Wave Height at position: " + waveHeight);
    }
}
