using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FloatingObjectV3 : MonoBehaviour
{
    [Header("Pływaki (Floaters)")]
    public Transform[] floaters;

    [Header("Ustawienia Wyporności")]
    public float floatingPower = 15f;
    

    [Header("Skalowanie Tłumienia z Prędkością (y = a*x + b)")]
    [Tooltip("Współczynnik 'a' w równaniu. Określa, jak mocno prędkość wpływa na tłumienie.")]
    public float dampingFactorA = 0.5f; 
    [Tooltip("Współczynnik 'b' w równaniu. To bazowa wartość tłumienia, gdy łódź stoi w miejscu.")]
    public float baseDampingB = 5f;   

    [Header("Ustawienia Oporu (Drag)")]
    public float underWaterDrag = 3f;
    public float underWaterAngularDrag = 1f;
    public float airDrag = 0f;
    public float airAngularDrag = 0.05f;

    [Header("Ustawienia Fal (Lux Water)")]
    public float timeOffset;
    public Material waterMaterial;

    private Rigidbody m_Rigidbody;
    private int floatersUnderWater;
    private LuxWaterUtils.GersterWavesDescription waveDescription;
    
    private BoatController boatController;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        
        boatController = GetComponent<BoatController>();
        if (boatController == null)
        {
            Debug.LogError("BoatController object not found!", this);
        }

        waveDescription = new LuxWaterUtils.GersterWavesDescription();
        LuxWaterUtils.GetGersterWavesDescription(ref waveDescription, waterMaterial);
    }

    void FixedUpdate()
    {
        floatersUnderWater = 0;


        float boatSpeed = Mathf.Abs(boatController.GetCurrentSpeed());
        
        float calculatedDamping = (dampingFactorA * boatSpeed) + baseDampingB;
        
        calculatedDamping = Mathf.Max(0, calculatedDamping);

        for (int i = 0; i < floaters.Length; i++)
        {
            Vector3 waveDisplacement = LuxWaterUtils.GetGestnerDisplacement(floaters[i].position, waveDescription, timeOffset);
            float waveHeight = waveDisplacement.y;

            float diff = floaters[i].position.y - waveHeight;

            if (diff < 0)
            {
                m_Rigidbody.AddForceAtPosition(Vector3.up * (floatingPower * Mathf.Abs(diff)), floaters[i].position, ForceMode.Force);
                
                float verticalVelocity = m_Rigidbody.GetPointVelocity(floaters[i].position).y;
     
                m_Rigidbody.AddForceAtPosition(Vector3.down * (verticalVelocity * calculatedDamping), floaters[i].position, ForceMode.Force);
                
                floatersUnderWater++;
            }
        }
        
        SwitchState();
    }

    void SwitchState()
    {
        if (floaters.Length > 0)
        {
            float underwaterRatio = (float)floatersUnderWater / (float)floaters.Length;
            
            m_Rigidbody.drag = Mathf.Lerp(airDrag, underWaterDrag, underwaterRatio);
            m_Rigidbody.angularDrag = Mathf.Lerp(airAngularDrag, underWaterAngularDrag, underwaterRatio);
        }
    }
}