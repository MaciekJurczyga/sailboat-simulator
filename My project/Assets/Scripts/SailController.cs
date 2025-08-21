using UnityEngine;

public class SailController : MonoBehaviour
{
    [Header("Ustawienia Obrotu Poziomego (Jib/Fok)")]
    public float jibRotationSpeed = 50f;
    public float fokMinAngle = -83f;
    public float fokMaxAngle = 83f;

    [Header("Ustawienia Obrotu Pionowego (Bom)")]
    public Transform bomTransform;
    public float bomRotationSpeed = 50f;
    public float bomMinAngle = -83f;
    public float bomMaxAngle = 83f;
    
    private float currentFokAngle = 0f;
    private Quaternion fokStartRotation;
    private float currentBomAngle = 0f;
    private Quaternion bomStartRotation;

    void Start()
    {
        if (bomTransform == null)
        {
            Debug.LogError("Pole 'Bom Transform' nie zostało przypisane w Inspektorze! Skrypt nie będzie działał poprawnie.");
            this.enabled = false;
            return;
        }

        fokStartRotation = transform.localRotation;
        bomStartRotation = bomTransform.localRotation;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.J))
        {
            currentFokAngle += jibRotationSpeed * Time.deltaTime;
            currentBomAngle += bomRotationSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.L))
        {
            currentFokAngle -= jibRotationSpeed * Time.deltaTime;
            currentBomAngle -= bomRotationSpeed * Time.deltaTime;
        }
        
        currentFokAngle = Mathf.Clamp(currentFokAngle, fokMinAngle, fokMaxAngle);
        Quaternion fokRotation = Quaternion.Euler(0, currentFokAngle, 0);
        transform.localRotation = fokStartRotation * fokRotation;

        currentBomAngle = Mathf.Clamp(currentBomAngle, bomMinAngle, bomMaxAngle);
        Quaternion bomRotation = Quaternion.Euler(0, 0, currentBomAngle);
        bomTransform.localRotation = bomStartRotation * bomRotation;
    }
}