using UnityEngine;

public class FloatingObjectV2 : MonoBehaviour
{
    public Material waterMaterial; 
    public float objectVolume = 1.0f; 
    public Vector3 worldPosition;  
    public float timeOffset;  
    public float buoyancyForce = 10.0f;  
    public float damping = 0.5f; 

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; 
    }

    void Update()
    {
        LuxWaterUtils.GersterWavesDescription waveDescription = new LuxWaterUtils.GersterWavesDescription();
        LuxWaterUtils.GetGersterWavesDescription(ref waveDescription, waterMaterial);
        Vector3 displacement = LuxWaterUtils.GetGestnerDisplacement(worldPosition, waveDescription, timeOffset);
        
        float waveHeight = displacement.y;
        
        float waterSurfaceHeight = waveHeight;
        
        float heightDifference = transform.position.y - waterSurfaceHeight;
        
        if (heightDifference < 0)
        {
            Vector3 buoyancy = Vector3.up * buoyancyForce * -heightDifference * objectVolume;
            
            rb.AddForce(buoyancy, ForceMode.Acceleration);
        }
        
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * damping, rb.velocity.z);
        
        Vector3 targetPosition = new Vector3(transform.position.x, waterSurfaceHeight, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 2f);
    }
}
