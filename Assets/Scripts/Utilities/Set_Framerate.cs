using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Set_Framerate : MonoBehaviour
{
    bool fSet = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(fSet == false)
        {
            Application.targetFrameRate = 300;
            fSet = true;
        }
    }
}
