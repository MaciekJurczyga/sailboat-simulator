using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FloatingObjectV3 : MonoBehaviour
{
    public Transform[] floaters;
    
    public float underWaterDrag = 3f;
    public float underWaterAngularDrag = 1f;
    public float airDrag = 0f;
    public float airAngularDrag = 0.05f;

    private Rigidbody m_Rigidbody;

    public float floatingPower = 15f;
    private bool underwater;
    
    public float timeOffset;
    public Material waterMaterial;

    private int floatersUnderWater;
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        floatersUnderWater = 0;
        for (int i = 0; i < floaters.Length; i++)
        {
            float waveHeight = getWaveHeightAtPoint(floaters[i].position);
            float diff = floaters[i].position.y - waveHeight;
            Debug.Log("Wave height: " + waveHeight + " position: " + floaters[i].position );
            if (diff < 0)
            {
                m_Rigidbody.AddForceAtPosition(floatingPower * Math.Abs(diff) * Vector3.up  , floaters[i].position, ForceMode.Force);
                floatersUnderWater += 1;
                if (!underwater)
                {
                    underwater = true;
                    SwitchState(true);
                }
            }
            if (underwater && floatersUnderWater == 0)
            {
                underwater = false;
                SwitchState(false);
            }   
        }
    }

    void SwitchState(bool isUnderwater)
    {
        if (isUnderwater)
        {
            m_Rigidbody.drag = underWaterDrag;
            m_Rigidbody.angularDrag = underWaterAngularDrag;
        }
        else
        {
            m_Rigidbody.drag = airDrag;
            m_Rigidbody.angularDrag = airAngularDrag;
        }
    }

    private float getWaveHeightAtPoint(Vector3 position)
    { 
        LuxWaterUtils.GersterWavesDescription waveDescription = new LuxWaterUtils.GersterWavesDescription();
        
        LuxWaterUtils.GetGersterWavesDescription(ref waveDescription, waterMaterial);

        Vector3 displacement = LuxWaterUtils.GetGestnerDisplacement(position, waveDescription, timeOffset);

   
        return displacement.y;
    }
}
