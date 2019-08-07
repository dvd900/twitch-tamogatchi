using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendShapeScaler : MonoBehaviour
{
    private float mSize = 100f;
    public SkinnedMeshRenderer rend;
    public DynamicBone dBone;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void BlendShape()
    {
        InvokeRepeating("Scale", 0.0f, 0.0001f);
    }
    void Scale()
    {
        if(mSize <= 0f)
        {
            CancelInvoke("Scale");
        }
        rend.SetBlendShapeWeight(0, mSize = mSize-10);
    }
    public void DynamicBoneDisable()
    {
        
        dBone.m_Damping = 1f;
        dBone.m_Stiffness = 1f;
        dBone.UpdateParameters();
    }
    public void DynamicBoneEnable()
    {

        dBone.m_Damping = 0.1f;
        dBone.m_Stiffness = 0.5f;
        dBone.UpdateParameters();
    }
}
