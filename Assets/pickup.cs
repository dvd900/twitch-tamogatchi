using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickup : MonoBehaviour
{
    Animator animController;
    public DynamicBone spine, l_arm, r_arm;
    // Start is called before the first frame update
    void Start()
    {
        animController = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            //l_arm.enabled = false;
            //r_arm.enabled = false;
           //spine.enabled = false;
            //spine.m_Damping = 0.229f;
            //spine.m_Stiffness = 0.869f;
            l_arm.m_Damping = 1f;
            l_arm.m_Stiffness = 1f;
            r_arm.m_Damping = 1f;
            r_arm.m_Stiffness = 1f;
            r_arm.UpdateParameters();
            l_arm.UpdateParameters();
            animController.SetInteger("action", 1);



        }
        else
        {
            //animController.SetInteger("action", 0);
            //l_arm.enabled = true;
           //r_arm.enabled = true;
        }
    }

}
