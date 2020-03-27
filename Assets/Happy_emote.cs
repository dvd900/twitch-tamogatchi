using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Happy_emote : MonoBehaviour
{
    public Renderer Eye1, Eye2;
    public Material Default, Happy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void MakeHappy()
    {
        Eye1.material = Happy;
        Eye2.material = Happy;
    }
    public void MakeDefault()
    {
        Eye1.material = Default;
        Eye2.material = Default;
    }
}
